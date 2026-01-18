using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Services.Cores.UserHistorys;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/user-history")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Logging]
    [Authorize]
    public class UserHistoryController : BaseApiController
    {
        private readonly IUserHistoryService _userHistoryService;
        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        /// <summary>
        /// Tìm kiếm lịch sử
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<ActionResult<SearchBaseResultModel<UserHistorySearchResultModel>>> SearchHistory([FromBody] UserHistorySearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _userHistoryService.SearchHistoryAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-excel")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportExcel([FromBody] UserHistorySearchModel searchModel)
        {
            var file = await _userHistoryService.ExportFileAsync(searchModel, NTSConstants.TemplateHistory, NTSConstants.OptionExport.Excel);

            return File(file.ToArray(), FileHelper.GetContentType(".xlsx"), "LichSuThaoTac.xlsx");
        }

        /// <summary>
        /// Xuất pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-pdf")]
        [ActionName(TextResourceKey.Action_Export_Pdf)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportPDF([FromBody] UserHistorySearchModel searchModel)
        {
            var file = await _userHistoryService.ExportFileAsync(searchModel, NTSConstants.TemplateHistory, NTSConstants.OptionExport.Pdf);
            return File(file.ToArray(), FileHelper.GetContentType(".pdf"), "LichSuThaoTac.pdf");
        }
    }
}
