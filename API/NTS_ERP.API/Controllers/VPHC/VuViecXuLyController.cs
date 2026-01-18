using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Helpers;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.VPHC.VuViec;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.VPHC.VuViecVPHC;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/xu-ly")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class VuViecXuLyController : BaseApiController
    {
        private readonly IVuViecXuLyService _vuViecService;

        public VuViecXuLyController(IVuViecXuLyService vuViecService)
        {
            _vuViecService = vuViecService;
        }
        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateVuViec([FromRoute] string id, [FromBody] VuViecXuLyModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _vuViecService.UpdateAsync(id, model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<VuViecXuLyModifyModel>>> GetVuViecById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _vuViecService.GetByIdAsnyc(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}