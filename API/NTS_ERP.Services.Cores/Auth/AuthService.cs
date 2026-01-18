using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS.Redis;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.Entities;
using NTS_ERP.Services.Cores.Log;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NTS_ERP.Services.Cores.Auth
{
    public class AuthService : IAuthService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly AppSettingModel _appSettingModel;
        private readonly RedisCacheService _redisCacheService;
        private readonly RedisCacheSettingModel _redisCacheSettings;
        private readonly ILogEventService _logEventService;

        public AuthService(NTS_ERPContext sqlContext, IOptions<AppSettingModel> options, IOptions<RedisCacheSettingModel> redisOptions,
            RedisCacheService redisCacheService, ILogEventService logEventService)
        {
            _sqlContext = sqlContext;
            _appSettingModel = options.Value;
            _redisCacheService = redisCacheService;
            _redisCacheSettings = redisOptions.Value;
            _logEventService = logEventService;
        }

        public async Task<NtsUserTokenModel> LoginAsync(NtsLogInModel loginModel, HttpRequest request)
        {
            var user = (from u in _sqlContext.Nts_User.AsNoTracking()
                        where u.UserName.Equals(loginModel.Username)
                        select new NtsUserLoginModel
                        {
                            UserId = u.Id,
                            UserName = u.UserName,
                            FullName = u.FullName,
                            PasswordHash = u.PasswordHash,
                            SecurityStamp = u.SecurityStamp,
                            LockoutEnabled = u.LockoutEnabled,
                            Avatar = u.Avatar,
                            IdDonVi = u.IdDonVi
                        }).FirstOrDefault();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001);
            }

            if (!user.LockoutEnabled.HasValue || user.LockoutEnabled.Value)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002);
            }

            user.Permission = GetListPermission(user.UserId);
            user.Password = loginModel.Password;
            user.ExpireDateAfter = _appSettingModel.ExpireDateAfter;
            user.Secret = _appSettingModel.Secret;
            var refreshtokenold = _sqlContext.Nts_RefreshToken.FirstOrDefault(e => e.UserId.Equals(user.UserId));
            if (refreshtokenold != null) _sqlContext.Nts_RefreshToken.Remove(refreshtokenold);
            var userToken = await this.NtsJwtLogin(user);
            SaveRefreshToken(user.UserId, userToken.RefreshToken);

            loginModel.Password = "";//không lưu password vào lịch sử
            UserHistoryModel userHistoryModel = new UserHistoryModel()
            {
                Name = "Đăng nhập hệ thống",
                Content = JsonConvert.SerializeObject(loginModel),
                Type = NTSConstants.UserHistory_Type_Login
            };
            _logEventService.LogEventAsync(request, userHistoryModel, user.UserId);
            _sqlContext.SaveChanges();
            return userToken;
        }
        public async Task<NtsUserTokenModel> RefreshToken(string userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003);
            }
            var refreshtoken = _sqlContext.Nts_RefreshToken.FirstOrDefault(e => e.UserId.Equals(userId));

            if (refreshtoken == null || refreshtoken.Token != refreshToken || refreshtoken.ExpireAt <= DateTime.Now)
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003);
            var user = new NtsUserLoginModel()
            {
                Permission = GetListPermission(userId),
                ExpireDateAfter = _appSettingModel.ExpireDateAfter,
                Secret = _appSettingModel.Secret,
                UserId = userId,
            };
            var userToken = await this.NtsJwtLogin(user, true);
            _sqlContext.Nts_RefreshToken.Remove(refreshtoken);
            SaveRefreshToken(user.UserId, userToken.RefreshToken);
            _sqlContext.SaveChanges();

            return userToken;
        }
        private void SaveRefreshToken(string userid, string tokenrefresh)
        {
            Nts_RefreshToken refreshtoken = new Nts_RefreshToken()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userid,
                Token = tokenrefresh,
                IssueAt = DateTime.Now.AddDays(_appSettingModel.IssuedAt),
                ExpireAt = DateTime.Now.AddDays(_appSettingModel.ExpiresAt),
            };
            _sqlContext.Nts_RefreshToken.Add(refreshtoken);

        }

        public List<string> GetListPermission(string userId)
        {
            List<string> listPermission = new List<string>();

            var userPermission = (from a in _sqlContext.Nts_UserPermission.AsNoTracking()
                                  join b in _sqlContext.Nts_Function.AsNoTracking() on a.FunctionId equals b.Id
                                  where a.UserId.Equals(userId) || a.UserId.Equals("UFR01")
                                  select b.Code).ToList();

            listPermission = userPermission.ToList();
            listPermission.AddRange(userPermission.ToList());

            return listPermission;
        }

        /// <summary>
        /// Đăng xuất hệ thống
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> LogOutAsync(string userId, HttpRequest request)
        {
            await this.Logout(userId);

            UserHistoryModel userHistoryModel = new UserHistoryModel()
            {
                Name = "Đăng xuất hệ thống",
                Type = NTSConstants.UserHistory_Type_Login
            };
            var refreshtoken = _sqlContext.Nts_RefreshToken.FirstOrDefault(e => e.UserId.Equals(userId));
            if (refreshtoken != null) _sqlContext.Nts_RefreshToken.Remove(refreshtoken);
            _logEventService.LogEventAsync(request, userHistoryModel, userId);
            _sqlContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Check token userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsTokenAlive(string userId, string token)
        {
            // Key lưu cache login
            string keyLogin = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            bool isToken = false;
            NtsUserTokenModel cachedValue = await _redisCacheService.GetAsync<NtsUserTokenModel>(keyLogin);
            if (cachedValue != null)
            {
                try
                {
                    isToken = token.Equals(cachedValue.Token);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return isToken;
        }

        /// <summary>
        /// Xóa rediscache theo userId
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveRedis(string userId)
        {
            // Key lưu cache login
            string keyLogin = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            if (_redisCacheService.ExistsAsync(keyLogin).Result)
            {
                var IsCheck = _redisCacheService.RemoveAsync(keyLogin).Result;
            }
        }

        #region Xử lý JwtSecurity
        /// <summary>
        /// Đăng suất
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task Logout(string userId)
        {
            // Tạo key cache
            string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            //key menu
            string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            // Kiểm tra cache tồn tại
            var keymenuExist = await _redisCacheService.ExistsAsync(keymenu);
            if (keymenuExist)
            {
                await _redisCacheService.RemoveAsync(keymenu);
            }
            var keyExist = await _redisCacheService.ExistsAsync(key);
            if (keyExist)
            {
                await _redisCacheService.RemoveAsync(key);
            }

        }

        /// <summary>
        /// Đăng nhập JWT
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<NtsUserTokenModel> NtsJwtLogin(NtsUserLoginModel loginModel, bool refresh = false)
        {
            NtsUserTokenModel userTokenModel = new NtsUserTokenModel();
            if (!refresh)
            {
                var passwordHash = PasswordUtils.ComputeHash(loginModel.Password + loginModel.SecurityStamp);
                if (!loginModel.PasswordHash.Equals(passwordHash))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001);
                }
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(loginModel.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginModel.UserId.ToString()),
                    new Claim("Permissions",string.Join(",", loginModel.Permission)),
                    new Claim("DonVi",loginModel.IdDonVi??"")
                }),
                Expires = DateTime.UtcNow.AddDays(loginModel.ExpireDateAfter),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            userTokenModel.Name = loginModel.UserName;
            userTokenModel.FullName = loginModel.FullName;
            userTokenModel.UserId = loginModel.UserId;
            userTokenModel.Avatar = loginModel.Avatar;
            userTokenModel.Token = tokenString;
            userTokenModel.ExpireDateAfter = loginModel.ExpireDateAfter;
            userTokenModel.Permissions = loginModel.Permission;
            userTokenModel.RefreshToken = await GenerateRefreshToken();
            userTokenModel.IdDonVi = loginModel.IdDonVi;

            // Key lưu cache login
            string keyLogin = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userTokenModel.UserId}";

            // Ghi thông tin vào cache
            await _redisCacheService.RemoveAsync(keyLogin);
            _redisCacheService.Add<NtsUserTokenModel>(keyLogin, userTokenModel, new TimeSpan(loginModel.ExpireDateAfter, 0, 0, 0));
            return userTokenModel;
        }

        public async Task<string> GenerateRefreshToken()
        {
            // generate token that is valid for 7 days
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string ValidateJwtToken(string token, string secret)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        #endregion
    }
}
