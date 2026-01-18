using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Helpers;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.VPHC.VuViec;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.VPHC.VuViecVPHC;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/vu-viec")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class VuViecController : BaseApiController
    {
        private readonly IVuViecService _vuViecService;

        public VuViecController(IVuViecService vuViecService)
        {
            _vuViecService = vuViecService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<VuViecSearchResultModel>>> SearchVuViec([FromBody] VuViecSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            if(string.IsNullOrEmpty( searchModel.IdDonVi))
            {
                searchModel.IdDonVi = CurrentUser.DonViId;
            }    
            apiResultModel.Data = await _vuViecService.SearchAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("quick-dashboard")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<VuViecSearchResultModel>>> QuickDashboard([FromBody] VuViecSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            if (string.IsNullOrEmpty(searchModel.IdDonVi))
            {
                searchModel.IdDonVi = CurrentUser.DonViId;
            }
            apiResultModel.Data = await _vuViecService.QuickDashboard(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateVuViec([FromBody] VuViecModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _vuViecService.CreateAsync(model, CurrentUser).Result;
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateVuViec([FromRoute] string id, [FromBody] VuViecModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _vuViecService.UpdateAsync(id, model, CurrentUser);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("soft-delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> SoftDeleteVuViec(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa tạm
            await _vuViecService.DeleteEntityAsync(id, CurrentUser.UserId, true);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("hard-delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteVuViec(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _vuViecService.DeleteEntityAsync(id, CurrentUser.UserId, false);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<VuViecModifyModel>>> GetVuViecById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _vuViecService.GetByIdAsnyc(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("finish/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> FinishVuViec([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _vuViecService.FinishAsync(id, CurrentUser.UserId);
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
        public async Task<IActionResult> ExportExcel([FromBody] VuViecSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.IdDonVi))
            {
                searchModel.IdDonVi = CurrentUser.DonViId;
            }

            var file = await _vuViecService.ExportFileAsync(searchModel, NTSConstants.TemplateVuViec, NTSConstants.OptionExport.Excel);

            return File(file.ToArray(), FileHelper.GetContentType(".xlsx"), "VuViec.xlsx");
        }


        [HttpGet]
        [Route("get-vu-viec-trong-bien-ban/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<VuViecTrongBienBanModel>> GetVuViecTrongBienBan([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  _vuViecService.GetVuViecTrongBienBan(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-vu-viec-trong-quyet-dinh/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<VuViecTrongBienBanModel>> GetVuViecTrongQuyetDinh([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _vuViecService.GetVuViecTrongQuyetDinh(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("check-view-notify/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public ActionResult<ApiResultModel<VuViecTrongBienBanModel>> CheckViewNotify([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _vuViecService.CheckViewNotify(id,CurrentUser.DonViId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}