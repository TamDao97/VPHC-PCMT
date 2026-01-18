using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.XuHuong;
using NTS_ERP.Services.VPHC.XuHuong;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/phan-tich")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class XuHuongController : BaseApiController
    {
        private readonly IXuHuongService _xuHuongService;

        public XuHuongController(IXuHuongService xuHuongService)
        {
            _xuHuongService = xuHuongService;
        }

        [HttpPost]
        [Route("xu-huong")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult> SearchTraCuuNguoiVP([FromBody] XuHuongSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            searchModel.IdDonVi = CurrentUser.DonViId;
            apiResultModel.Data = await _xuHuongService.PhanTichXuHuong(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}