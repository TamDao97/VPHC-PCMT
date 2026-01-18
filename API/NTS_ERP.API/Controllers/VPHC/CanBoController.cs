using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.ToChucVP;
using NTS_ERP.Services.VPHC.CanBo;
using NTS_ERP.Services.VPHC.ToChucVP;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/can-bo")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class CanBoController : BaseApiController
    {
        private readonly ICanBoService _canBoService;

        public CanBoController(ICanBoService toChucVPService)
        {
            _canBoService = toChucVPService;
        }

        [HttpGet]
        [Route("get-by-id-don-vi/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<ToChucVPModifyModel>>> GetByIdDonVi([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _canBoService.GetByIdDonVi(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}