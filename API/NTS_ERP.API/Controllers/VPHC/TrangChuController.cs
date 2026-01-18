using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TraCuu;
using NTS_ERP.Models.VPHC.TrangChu;
using NTS_ERP.Services.VPHC.TraCuu;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/trang-chu")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class TrangChuController : BaseApiController
    {
        private readonly ITrangChuService _trangChuService;

        public TrangChuController(ITrangChuService trangChuService)
        {
            _trangChuService = trangChuService;
        }

        [HttpPost]
        [Route("tinh-hinh-chung")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult> SearchTraCuuNguoiVP([FromBody] TrangChuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            searchModel.IdDonVi = CurrentUser.DonViId;
            apiResultModel.Data = await _trangChuService.TinhHinhChung(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}