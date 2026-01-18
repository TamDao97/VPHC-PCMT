using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.ThongKe;
using NTS_ERP.Models.VPHC.XuHuong;
using NTS_ERP.Services.VPHC.ThongKe;
using NTS_ERP.Services.VPHC.TraCuu;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/thong-ke")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class ThongKeController : BaseApiController
    {
        private readonly IThongKeService _thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        [HttpPost]
        [Route("thong-ke")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult> ThongKe([FromBody] ThongKeSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            searchModel.IdDonVi = CurrentUser.DonViId;
            apiResultModel.Data = await _thongKeService.ThongKe(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}