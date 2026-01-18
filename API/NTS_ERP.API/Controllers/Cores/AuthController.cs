using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.User;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.Cores.ConfigInterface;

namespace NTS_ERP.Api.Controllers.Cores
{
    [ApiController]
    [ApiHandleExceptionSystem]
    [Route("api/auth")]
    [Logging]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IConfigInterfaceService _configInterfaceService;

        public AuthController(IAuthService authService, IConfigInterfaceService configInterfaceService)
        {
            _authService = authService;
            _configInterfaceService = configInterfaceService;
        }
        /// <summary>
        /// cấu hình giao diện
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-detail")]
        public async Task<ActionResult<ApiResultModel>> GetConfig()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _configInterfaceService.GetConfigAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="logInModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ActionName(NTSConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<NtsUserTokenModel>>> LogInAsync([FromBody] NtsLogInModel logInModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _authService.LoginAsync(logInModel, Request);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="RefreshTokenModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh/token")]
        [ActionName(NTSConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<NtsUserTokenModel>>> RefreshTokenAsync([FromBody] RefreshTokenModel tokenrefresh)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _authService.RefreshToken(CurrentUser.UserId, tokenrefresh.RefreshToken);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
