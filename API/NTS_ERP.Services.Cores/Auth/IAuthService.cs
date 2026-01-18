using Microsoft.AspNetCore.Http;
using NTS_ERP.Models.Cores.Auth;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Auth
{
    public interface IAuthService
    {
        Task<NtsUserTokenModel> LoginAsync(NtsLogInModel loginModel, HttpRequest request);

        Task<NtsUserTokenModel> RefreshToken(string userId, string refreshToken);
        //string GetById(string id);

        Task<bool> LogOutAsync(string userId, HttpRequest request);

        /// <summary>
        /// Check token userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsTokenAlive(string userId, string token);

        /// <summary>
        /// Xóa rediscache theo userId
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveRedis(string userId);

        List<string> GetListPermission(string userId);
    }
}
