using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.ToChucVP;
using NTS_ERP.Services.VPHC.ToChucVP;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/to-chuc-vp")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [Logging]
    [ApiHandleExceptionSystem]
    public class ToChucVPController : BaseApiController
    {
        private readonly IToChucVPService _toChucVPService;

        public ToChucVPController(IToChucVPService toChucVPService)
        {
            _toChucVPService = toChucVPService;
        }

        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<ToChucVPResultModel>>> SearchToChucVP([FromBody] ToChucVPSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            // Sét các thông tin login vào model
            // this.SetRequestInfoToModel(searchModel);
            apiResultModel.Data = await _toChucVPService.Search(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateToChucVP([FromBody] ToChucVPModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _toChucVPService.Create(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateToChucVP([FromRoute] string id, [FromBody] ToChucVPModifyModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _toChucVPService.Update(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> HardDeleteToChucVP(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            //Thực hiện xóa trong CSDL
            await _toChucVPService.Delete(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<ToChucVPModifyModel>>> GetToChucVPById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _toChucVPService.GetById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}