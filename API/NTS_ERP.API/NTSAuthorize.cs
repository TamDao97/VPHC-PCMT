using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Services.Cores.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NTS_ERP.Api
{
    public class NTSAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            //// kiểm tra hạn của token
            //var authHeader = context.HttpContext.Request.Headers["Authorization"];
            //if (string.IsNullOrEmpty(authHeader) || !authHeader.ToString().StartsWith("Bearer "))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}
            ////// Lấy JWT từ header
            //var token = authHeader.ToString().Substring("Bearer ".Length);
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes("bkcontech_api_server");
            //try
            //{
            //    tokenHandler.ValidateToken(token, new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            //        ClockSkew = TimeSpan.Zero
            //    }, out SecurityToken validatedToken);
            //    // nếu token hết hạn or key sai sẽ nhảy vào catch         
            //    var user = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name));
            //    if (user != null)
            //    {
            //        var service = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
            //        var isAuthorized =   service.IsTokenAlive(user.Value, token).Result;
            //        if (!isAuthorized)
            //        {
            //            context.Result = new ObjectResult(apiResultModel)
            //            {
            //                StatusCode = StatusCodes.Status401Unauthorized
            //            };
            //        }
            //    }

                return;
            //}
            //catch
            //{
            //    context.Result = new ObjectResult(apiResultModel)
            //    {
            //        StatusCode = StatusCodes.Status401Unauthorized
            //    };
            //}
        }
    }
}

