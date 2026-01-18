using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.BienBan;
using NTS_ERP.Services.VPHC.BienBan;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/bien-ban")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class BienBanController : BaseApiController
    {
        private readonly IBienBanService _bienBanService;

        public BienBanController(IBienBanService bienBanService)
        {
            _bienBanService = bienBanService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<BienBanResultModel>>> SearchBienBan([FromBody] BienBanSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _bienBanService.Search(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateBienBan([FromBody] BienBanModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _bienBanService.Create(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateBienBan([FromRoute] string id, [FromBody] BienBanModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _bienBanService.Update(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteBienBan(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _bienBanService.Delete(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<BienBanModifyModel>>> GetBienBanById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _bienBanService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-danh-muc")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<BienBanModifyModel>> GetBienBanById()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _bienBanService.GetDanhMuc();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-bien-ban-vu-viec/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<List<BienBanVuViecModel>>> GetBienBanVuViec([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _bienBanService.GetBienBanVuViec(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất biên bản doc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("xuat-bien-ban-doc/{id}")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportBienBanDoc([FromRoute]string id)
        {
            var file =  _bienBanService.ExportFileAsync(id, NTSConstants.TemplateBienBan_VPHC, NTSConstants.OptionExport.Word);

            return File(file.ToArray(), FileHelper.GetContentType(".doc"),Path.GetFileName(NTSConstants.TemplateBienBan_VPHC));
        }

        /// <summary>
        /// Xuất biên bản pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("xuat-bien-ban-pdf/{id}")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportBienBanPdf([FromRoute] string id)
        {
            var file = _bienBanService.ExportFileAsync(id, NTSConstants.TemplateBienBan_VPHC, NTSConstants.OptionExport.Pdf);

            return File(file.ToArray(), FileHelper.GetContentType(".pdf"), Path.GetFileName(NTSConstants.TemplateBienBan_VPHC));
        }
    }
}