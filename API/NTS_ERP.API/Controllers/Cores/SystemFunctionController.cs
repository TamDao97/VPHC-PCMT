using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.SystemFunction;
using NTS_ERP.Services.Cores.SystemFunction;
using NTS_ERP.Models.Cores.Common;
using System.Threading.Tasks;

namespace NTS_ERP.Api.Controllers.Cores
{
    [ApiController]
    [ApiHandleExceptionSystem]
    [Route("api/system-function")]
    [ValidateModel]
    //[Logging]
    //[Authorize]
    public class SystemFunctionController : BaseApiController
    {
        private readonly ISystemFunctionService _systemFunctionService;
        public SystemFunctionController(ISystemFunctionService systemFunctionService)
        {
            _systemFunctionService = systemFunctionService;
        }

        /// <summary>
        /// Lấy anh sách bảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("table")]
        public async Task<ActionResult<object>> GetTableNameAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _systemFunctionService.GetTableNameAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách column của bảng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("column-table/{table}")]
        public async Task<ActionResult<object>> GetColumnTableAsync([FromRoute] string table)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetColumnTableAsync(table);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách column của bảng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("combox-column-table/{table}")]
        public async Task<ActionResult<object>> GetComboxColumnTable([FromRoute] string table)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetComboxColumnTable(table);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm cấu hình chức năng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-config")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<SearchBaseResultModel<FuntionConfigSearchResultModel>>> SearchConfigAsync([FromBody] FuntionConfigSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.SearchConfig(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [ActionName(TextResourceKey.Action_Create)]
        [Route("create-config")]
        public async Task<ActionResult<string>> CreateDataAsync([FromBody] SystemFunctionConfigModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.CreateFuntionConfig(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("get-update-config/{id}")]
        public async Task<ActionResult<bool>> UpdateFuntionConfigAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetFuntionConfigUpdateById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [ActionName(TextResourceKey.Action_Update)]
        [Route("update-config/{id}")]
        public async Task<ActionResult<bool>> UpdateFuntionConfigAsync([FromRoute] string id, [FromBody] FunctionConfigUpdateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.UpdateFuntionConfig(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [ActionName(TextResourceKey.Action_Delete)]
        [Route("delete-config/{id}")]
        public async Task<ActionResult<bool>> DeleteFuntionConfigAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.DeleteFuntionConfig(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("get-config-design/{slugorid}")]
        public async Task<ActionResult<FunctionConfigViewModel>> GetConfigDesign([FromRoute] string slugorid)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetFuntionConfigBySlug(slugorid);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [ActionName(TextResourceKey.Action_Search)]
        [Route("search/{slug}")]
        public async Task<ActionResult<object>> SearchDataAsync([FromRoute] string slug, [FromBody] object model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.SearchDataAsync(slug, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [ActionName(TextResourceKey.Action_Create)]
        [Route("create/{slug}")]
        public async Task<ActionResult<object>> CreateDataAsync([FromRoute] string slug, [FromBody] object model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.CreateDataAsync(slug, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("get-update/{slug}/{id}")]
        public async Task<ActionResult<object>> GetUpdateDataAsync([FromRoute] string slug, [FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetUpdateDataAsync(slug, id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPut]
        [ActionName(TextResourceKey.Action_Update)]
        [Route("update/{slug}/{id}")]
        public async Task<ActionResult<object>> UpdateDataAsync([FromRoute] string slug, [FromRoute] string id, [FromBody] object model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.UpdateDataAsync(slug, id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [ActionName(TextResourceKey.Action_Get)]
        [Route("get-detail/{slug}/{id}")]
        public async Task<ActionResult<object>> GetDetailDataAsync([FromRoute] string slug, [FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.GetDetailDataAsync(slug, id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [ActionName(TextResourceKey.Action_Delete)]
        [Route("delete/{slug}/{id}")]
        public async Task<ActionResult<object>> DeleteDataAsync([FromRoute] string slug, [FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.DeleteDataAsync(slug, id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [ActionName(TextResourceKey.Action_Delete)]
        [Route("get-tree-data/{slug}")]
        public async Task<ActionResult<object>> GetTreeViewData([FromRoute] string slug)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _systemFunctionService.TreeViewDataAsync(slug);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
