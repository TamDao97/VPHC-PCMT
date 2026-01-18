using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhienDich;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/phien-dich")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class PhienDichController : BaseApiController
    {
        private readonly IPhienDichService _chungChiGiayPhepService;

        public PhienDichController(IPhienDichService _chungChiGiayPhepService)
        {
            _chungChiGiayPhepService = _chungChiGiayPhepService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<PhienDichResultModel>>> SearchPhienDich([FromBody] PhienDichSearchModel searchModel)
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
        public async Task<ActionResult<ApiResultModel>> CreatePhienDich([FromBody] PhienDichModifyModel model)
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
        public async Task<ActionResult<ApiResultModel>> UpdatePhienDich([FromRoute] string id, [FromBody] PhienDichModifyModel model)
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
        public async Task<ActionResult<ApiResultModel>> HardDeletePhienDich(string id)
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
        public async Task<ActionResult<ApiResultModel<PhienDichModifyModel>>> GetPhienDichById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _chungChiGiayPhepService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}