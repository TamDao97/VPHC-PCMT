using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiChungKien;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/chung-kien")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class NguoiChungKienController : BaseApiController
    {
        private readonly INguoiChungKienService _chungChiGiayPhepService;

        public NguoiChungKienController(INguoiChungKienService _chungChiGiayPhepService)
        {
            _chungChiGiayPhepService = _chungChiGiayPhepService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<NguoiChungKienResultModel>>> SearchNguoiChungKien([FromBody] NguoiChungKienSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _chungChiGiayPhepService.Search(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateNguoiChungKien([FromBody] NguoiChungKienModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _chungChiGiayPhepService.Create(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateNguoiChungKien([FromRoute] string id, [FromBody] NguoiChungKienModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _chungChiGiayPhepService.Update(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteNguoiChungKien(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _chungChiGiayPhepService.Delete(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<NguoiChungKienModifyModel>>> GetNguoiChungKienById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _chungChiGiayPhepService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}