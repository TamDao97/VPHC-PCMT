using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.Cores.User;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.Cores.Users;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Tìm kiếm user
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<UserSearchResultModel>>> SearchUser([FromBody] UserSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _userService.SearchUserAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateUser([FromBody] UserCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.CreateUserAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateUser([FromRoute] string id, [FromBody] UserCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.UpdateUserAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> DeleteUser(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _userService.DeleteUserAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy dữ liệu tài khoản admin theo id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-id/{userId}")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<UserCreateModel>>> GetUserById([FromRoute] string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.GetUserByIdAsnyc(userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy quyền 
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-permission")]
        [AllowPermission(Permissions = "F0101;F0102")]
        [ActionName(NTSConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<List<GroupFunctionModel>>>> GetPermission([FromQuery] string groupUserId = "")
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.GetPermissionAsync(groupUserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Khóa tài khoản
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("lock/{userId}")]
        [ActionName(TextResourceKey.Action_User_Lock)]
        [AllowPermission(Permissions = "F0106")]
        public async Task<ActionResult<ApiResultModel>> UserLock(string userId, [FromQuery] bool isunlock)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.UserLockAsync(userId, isunlock);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-pass")]
        [ActionName(TextResourceKey.Action_User_ChangePass)]
        [AllowPermission(Permissions = "F0105")]
        public async Task<ActionResult<ApiResultModel>> ResetPassword([FromBody] ChangePasswordModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            model.Id = CurrentUser.UserId;
            await _userService.ChangePasswordAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-info/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult<ApiResultModel>> UpdateUserInfo([FromRoute] string id, [FromBody] UserInfoModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _userService.UpdateUserInfoAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy dữ liệu tài khoản admin theo id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-info/{userId}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<UserCreateModel>>> GetUserInfo([FromRoute] string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _userService.GetUserInfoAsnyc(userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy dữ liệu tài khoản admin theo id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-token")]
        [ActionName(TextResourceKey.Action_Get)]
        
        public async Task<ActionResult<ApiResultModel<UserInfoModel>>> GetUserByToken()
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _userService.GetUserInfoAsnyc(CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("logout")]
        [ActionName(NTSConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel>> LogOutAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            var userId = CurrentUser.UserId;
            apiResultModel.Data = await _authService.LogOutAsync(userId, Request);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}