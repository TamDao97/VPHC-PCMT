using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.FileUpload;
using NTS_ERP.Services.Cores.FileUpload;

namespace NTS_ERP.Api.Controllers.Cores
{

    [Route("api/filetemplate")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [Authorize]
    [ApiHandleExceptionSystem]
    public class FileTemplateController : BaseApiController
    {
        private readonly IFileTemplateService _fileTemplateService;

        public FileTemplateController(IFileTemplateService fileTemplateService)
        {
            _fileTemplateService = fileTemplateService;
        }

        /// <summary>
        /// Tìm kiếm biểu mẫu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0131;F0132")]
        public async Task<ActionResult<SearchBaseResultModel<TemplateSearchResultModel>>> SearchFileTemplate([FromBody] TemplateSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _fileTemplateService.SearchFileTemplate(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xem biểu mẫu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0132")]
        public async Task<ActionResult<ApiResultModel>> GetFileTemplateById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _fileTemplateService.GetFileTemplateById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0131")]
        public async Task<ActionResult<ApiResultModel>> CreateFileTemplate([FromBody] FileUploadCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _fileTemplateService.CreateFileTemplate("1", model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0132")]
        public async Task<ActionResult<ApiResultModel>> UpdateFileTemplate([FromRoute] string id, [FromBody] FileUploadCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _fileTemplateService.UpdateFileTemplate("1", id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

    }
}
