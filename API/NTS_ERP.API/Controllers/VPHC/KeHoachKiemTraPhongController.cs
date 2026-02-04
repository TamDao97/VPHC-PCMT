//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NTS.Common.Resource;
//using NTS_ERP.Api.Attributes;
//using NTS_ERP.Api.Controllers.Cores;
//using NTS_ERP.Models.Cores.Common;
//using NTS_ERP.Models.VPHC.KeHoachKiemTra;
//using NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao;
//using NTS_ERP.Services.VPHC.KeHoachKiemTraPhong;

//namespace NTS_ERP.API.Controllers.VPHC
//{
//    [Route("api/ke-hoach-kiem-tra-phong")]
//    [ApiController]
//    [Authorize]
//    [ValidateModel]
//    [Logging]
//    [ApiHandleExceptionSystem]
//    public class KeHoachKiemTraPhongController : BaseApiController
//    {
//        private readonly IKeHoachKiemTraPhongService _keHoachKiemTraPhongService;

//        public KeHoachKiemTraPhongController(IKeHoachKiemTraPhongService keHoachKiemTraPhongService)
//        {
//            _keHoachKiemTraPhongService = keHoachKiemTraPhongService;
//        }

//        [HttpPost]
//        [Route("search")]
//        [ActionName(TextResourceKey.Action_Search)]
//        //[AllowPermission(Permissions = "F0104")]
//        public async Task<ActionResult<KeHoachKiemTraBaseResultModel>> SearchTraCuuNguoiVP([FromBody] KeHoachKiemTraSearchModel searchModel)
//        {
            
//            ApiResultModel apiResultModel = new ApiResultModel();
//            // Sét các thông tin login vào model
//            // this.SetRequestInfoToModel(searchModel);
//            apiResultModel.Data = await _keHoachKiemTraPhongService.SearchAsync(searchModel);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpPut]
//        [Route("update/{id}")]
//        [ActionName(TextResourceKey.Action_Update)]
//        //[AllowPermission(Permissions = "F0102")]
//        public async Task<ActionResult<ApiResultModel>> UpdateAsync([FromBody] KeHoachKiemTraUpdateRequestModel model)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();
//            apiResultModel.Data = await _keHoachKiemTraPhongService.UpdateAsync(model, CurrentUser);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpGet]
//        [Route("get-by-id/{id}")]
//        [ActionName(TextResourceKey.Action_Get)]
//        //[AllowPermission(Permissions = "F0104;F0102")]
//        public async Task<ActionResult<ApiResultModel<KeHoachKiemTraDetailResponseModel>>> GetByIdAsnyc([FromRoute] string id)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();
//            apiResultModel.Data = await _keHoachKiemTraPhongService.GetByIdAsnyc(id);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpDelete]
//        [Route("soft-delete/{id}")]
//        [ActionName(TextResourceKey.Action_Delete)]
//        //[AllowPermission(Permissions = "F0103")]
//        public async Task<ActionResult<ApiResultModel>> SoftDeleteAsync(string id)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();

//            //Thực hiện xóa tạm
//            await _keHoachKiemTraPhongService.DeleteEntityAsync(id, CurrentUser.UserId, true);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpDelete]
//        [Route("hard-delete/{id}")]
//        [ActionName(TextResourceKey.Action_Delete)]
//        //[AllowPermission(Permissions = "F0103")]
//        public async Task<ActionResult<ApiResultModel>> HardDeleteAsync(string id)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();

//            //Thực hiện xóa trong CSDL
//            await _keHoachKiemTraPhongService.DeleteEntityAsync(id, CurrentUser.UserId, false);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpPost]
//        [Route("update-status")]
//        [ActionName(TextResourceKey.Action_Update)]
//        //[AllowPermission(Permissions = "F0102")]
//        public async Task<ActionResult<ApiResultModel>> UpdateStatusAsync(KeHoachKiemTraUpdateStatusModel model)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();
//            apiResultModel.Data = await _keHoachKiemTraPhongService.UpdateStatusAsync(model, CurrentUser);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }

//        [HttpPost()]
//        [Route("assignee-task")]
//        [ActionName(TextResourceKey.Action_Update)]
//        public async Task<ActionResult<ApiResultModel>> AssigneeTaskAsync(KeHoachKiemTraPhanGiaoCreateRequestModel model)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();
//            apiResultModel.Data = await _keHoachKiemTraPhongService.AssigneeTaskAsync(model, CurrentUser);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }
//        [HttpGet]
//        [Route("get-detail-assignee-task-by-idKeHoach/{id}")]
//        [ActionName(TextResourceKey.Action_Get)]
//        //[AllowPermission(Permissions = "F0104;F0102")]
//        public async Task<ActionResult<ApiResultModel<List<KeHoachKiemTraPhanGiaoModel>>>> GetDetailAssigneeTaskByIdKeHoachAsync([FromRoute] string id)
//        {
//            ApiResultModel apiResultModel = new ApiResultModel();
//            apiResultModel.Data = await _keHoachKiemTraPhongService.GetDetailAssigneeTaskByIdKeHoachAsync(id, CurrentUser);
//            apiResultModel.IsStatus = true;
//            return Ok(apiResultModel);
//        }
//    }
//}
