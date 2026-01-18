using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TraCuu;
using NTS_ERP.Services.VPHC.TraCuu;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/tra-cuu-nguoi-vp")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class TraCuuNguoiVPController : BaseApiController
    {
        private readonly ITraCuuNguoiVPService _vuViecService;

        public TraCuuNguoiVPController(ITraCuuNguoiVPService vuViecService)
        {
            _vuViecService = vuViecService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<TraCuuBaseResultModel>> SearchTraCuuNguoiVP([FromBody] TraCuuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _vuViecService.SearchAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("quick-dashboard")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<TraCuuSearchModel>> QuickDashboard([FromBody] TraCuuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _vuViecService.QuickDashboard(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-excel")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]vi
        public async Task<IActionResult> ExportExcel([FromBody] TraCuuSearchModel searchModel)
        {
            var file = await _vuViecService.ExportFileAsync(searchModel, NTSConstants.TemplateNguoiVP, NTSConstants.OptionExport.Excel);

            return File(file.ToArray(), FileHelper.GetContentType(".xlsx"), "TraCuuNguoiVP.xlsx");
        }
    }
}