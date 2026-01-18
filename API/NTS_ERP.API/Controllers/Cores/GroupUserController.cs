using NTS_ERP.Api.Attributes;
using NTS_ERP.Services.Cores.GroupUsers;
using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using Microsoft.AspNetCore.Authorization;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/group-users")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [Authorize]
    [ApiHandleExceptionSystem]
    public class GroupUserController : BaseApiController
    {
        private readonly IGroupUserService groupUser;
        public GroupUserController(IGroupUserService groupUserService)
        {
            groupUser = groupUserService;
        }
        /// <summary>
        /// Tìm kiếm nhóm người dùng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0114")]
        public async Task<ActionResult<SearchBaseResultModel<GroupUserResultModel>>> SearchGroupUser([FromBody] GroupUserSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            //this.SetRequestInfoToModel(modelSearch);
            apiResultModel.Data = await groupUser.SearchGroupUser(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin nhóm người dùng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-group-user")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<GroupFunctionInfoModel>> GetGroupUserById([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await groupUser.GetGroupUserById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm nhóm người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0111")]
        public async Task<ActionResult<ApiResultModel>> CreateGroupUser([FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            string userId = CurrentUser.UserId;
            await groupUser.CreateGroupUser(model, userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập nhóm người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0112")]
        public async Task<ActionResult<ApiResultModel>> UpdateGroupUser([FromRoute] string id, [FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            string userId = CurrentUser.UserId;
            await groupUser.UpdateGroupUser(id, model, userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa nhóm người dùng
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> DeleteGroupUser([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            string userId = CurrentUser.UserId;
            await groupUser.DeleteGroupUserById(id, userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


    }
}