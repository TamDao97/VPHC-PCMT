using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TaiLieu;
using NTS_ERP.Services.VPHC.TaiLieu;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/tai-lieu")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class TaiLieuController : BaseApiController
    {
        private readonly ITaiLieuService _taiLieuService;

        public TaiLieuController(ITaiLieuService quyetDinhService)
        {
            _taiLieuService = quyetDinhService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<TaiLieuResultModel>>> SearchTaiLieu([FromBody] TaiLieuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _taiLieuService.Search(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateTaiLieu([FromBody] TaiLieuModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _taiLieuService.Create(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteTaiLieu(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _taiLieuService.Delete(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<TaiLieuModifyModel>>> GetTaiLieuById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _taiLieuService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-danh-muc")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<TaiLieuModifyModel>> GetDanhMuc()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _taiLieuService.GetDanhMuc();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-danh-muc-total/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<TaiLieuModifyModel>> GetDanhMucTotal([FromRoute] string? id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _taiLieuService.GetDanhMucTotal(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


        [HttpGet]
        [Route("get-tai-lieu-vu-viec/{id}/{iddanhmuc?}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<List<TaiLieuVuViecModel>>> GetTaiLieuVuViec([FromRoute] string id, [FromRoute] string? iddanhmuc)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _taiLieuService.GetTaiLieuVuViec(id, iddanhmuc);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("get-file-tai-lieu/{id}")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        //[AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportQuyetDinhDoc([FromRoute] string id)
        {
            var fileResult = _taiLieuService.GetFile(id);

            return File(fileResult.MemoryStreamFile.ToArray(), FileHelper.GetContentType(fileResult.Extention), fileResult.FileName);
        }
    }
}