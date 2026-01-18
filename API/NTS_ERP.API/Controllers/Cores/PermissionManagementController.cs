using NTS_ERP.Services.Cores.ConfigInterface;
using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.Permission;
using Microsoft.AspNetCore.Authorization;

namespace NTS_ERP.Api.Controllers.Cores
{

    [Route("api/permission")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class PermissionManagementController : BaseApiController
    {
        private readonly IPermissionService _permissionService;
        public PermissionManagementController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        [Route("user-info")]
        [AllowPermission(Permissions = "F0171")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<SearchBaseResultModel<UserPermissionModel>>> GetUserInfo([FromBody] PermissionSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _permissionService.GetUserInfoAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("user-delete/{userid}")]
        [AllowPermission(Permissions = "F0172")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult> DeletePermission(string userid)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _permissionService.DeletePermissionAsync(userid);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
