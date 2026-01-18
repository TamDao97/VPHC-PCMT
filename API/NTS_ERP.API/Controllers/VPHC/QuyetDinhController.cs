using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.QuyetDinh;
using NTS_ERP.Services.VPHC.QuyetDinh;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/quyet-dinh")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class QuyetDinhController : BaseApiController
    {
        private readonly IQuyetDinhService _quyetDinhService;

        public QuyetDinhController(IQuyetDinhService quyetDinhService)
        {
            _quyetDinhService = quyetDinhService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<QuyetDinhResultModel>>> SearchQuyetDinh([FromBody] QuyetDinhSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _quyetDinhService.Search(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateQuyetDinh([FromBody] QuyetDinhModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _quyetDinhService.Create(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateQuyetDinh([FromRoute] string id, [FromBody] QuyetDinhModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _quyetDinhService.Update(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteQuyetDinh(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _quyetDinhService.Delete(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<QuyetDinhModifyModel>>> GetQuyetDinhById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _quyetDinhService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-danh-muc")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<QuyetDinhModifyModel>> GetQuyetDinhById()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _quyetDinhService.GetDanhMuc();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-quyet-dinh-vu-viec/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<List<QuyetDinhVuViecModel>>> GetQuyetDinhVuViec([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _quyetDinhService.GetQuyetDinhVuViec(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất biên bản doc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("xuat-quyet-dinh-doc/{id}")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportQuyetDinhDoc([FromRoute]string id)
        {
            var file =  _quyetDinhService.ExportFileAsync(id, NTSConstants.TemplateQuyetDinh_VPHC, NTSConstants.OptionExport.Word);

            return File(file.ToArray(), FileHelper.GetContentType(".doc"),Path.GetFileName(NTSConstants.TemplateQuyetDinh_VPHC));
        }

        /// <summary>
        /// Xuất biên bản pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("xuat-quyet-dinh-pdf/{id}")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportQuyetDinhPdf([FromRoute] string id)
        {
            var file = _quyetDinhService.ExportFileAsync(id, NTSConstants.TemplateQuyetDinh_VPHC, NTSConstants.OptionExport.Pdf);

            return File(file.ToArray(), FileHelper.GetContentType(".pdf"), Path.GetFileName(NTSConstants.TemplateQuyetDinh_VPHC));
        }
    }
}