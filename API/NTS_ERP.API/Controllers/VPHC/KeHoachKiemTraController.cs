using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Api.Controllers.Cores;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;
using NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao;
using NTS_ERP.Services.VPHC.KeHoachKiemTra;

namespace NTS_ERP.API.Controllers.VPHC
{
    [Route("api/ke-hoach-kiem-tra")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class KeHoachKiemTraController : BaseApiController
    {
        private readonly IKeHoachKiemTraService _keHoachKiemTraService;

        public KeHoachKiemTraController(IKeHoachKiemTraService keHoachKiemTraService)
        {
            _keHoachKiemTraService = keHoachKiemTraService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<KeHoachKiemTraBaseResultModel>> SearchTraCuuNguoiVP([FromBody] KeHoachKiemTraSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _keHoachKiemTraService.SearchAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> CreateAsync([FromBody] KeHoachKiemTraCreateRequestModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.CreateAsync(model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateAsync([FromBody] KeHoachKiemTraUpdateRequestModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.UpdateAsync(model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<KeHoachKiemTraDetailResponseModel>>> GetByIdAsnyc([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.GetByIdAsnyc(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("soft-delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> SoftDeleteAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa tạm
            await _keHoachKiemTraService.DeleteEntityAsync(id, CurrentUser.UserId, true);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("hard-delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _keHoachKiemTraService.DeleteEntityAsync(id, CurrentUser.UserId, false);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("update-status")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusAsync(KeHoachKiemTraUpdateStatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.UpdateStatusAsync(model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost()]
        [Route("assignee-task")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult<ApiResultModel>> AssigneeTaskAsync(KeHoachKiemTraPhanGiaoCreateRequestModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.AssigneeTaskAsync(model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-detail-assignee-task-by-idKeHoach/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<List<KeHoachKiemTraPhanGiaoModel>>>> GetDetailAssigneeTaskByIdKeHoachAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _keHoachKiemTraService.GetDetailAssigneeTaskByIdKeHoachAsync(id, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
