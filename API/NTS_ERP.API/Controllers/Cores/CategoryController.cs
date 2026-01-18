using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Category;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Services.Cores.Categorys;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/category")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Logging]
    [Authorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<ApiResultModel>> SearchCategory()
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _categoryService.SearchGroupCategoryAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("table/search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0121;F0122;F0123")]
        public async Task<ActionResult<ApiResultModel>> SearchCategoryTable(CategorySearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _categoryService.SearchCategoryTableAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới danh mục
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0121")]
        public async Task<ActionResult<ApiResultModel>> CreateCategory([FromBody] CategoryCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _categoryService.CreateCategoryAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <param name="model">Dữ liệu cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0122")]
        public async Task<ActionResult<ApiResultModel>> UpdateCategory([FromRoute] string id, [FromBody] CategoryCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _categoryService.UpdateCategoryAsync(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0123")]
        public async Task<ActionResult<ApiResultModel>> DeleteCategory([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _categoryService.DeleteCategoryByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin danh mục theo id
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0122")]
        public async Task<ActionResult<CategoryCreateModel>> GetCategoryById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _categoryService.GetCategoryByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách thứ tự hiển thị
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-oder")]
        [AllowPermission(Permissions = "F0121;F0122")]
        public async Task<ActionResult<ApiResultModel>> GetListOrder(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _categoryService.GetListOrderAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới danh mục theo tên bảng
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("table")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0121")]
        public async Task<ActionResult<ApiResultModel>> CreateCategoryTableAsync([FromBody] CategoryModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _categoryService.CreateCategoryTableAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật danh mục theo tên bảng
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <param name="model">Dữ liệu cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        [Route("table/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0122")]
        public async Task<ActionResult<ApiResultModel>> UpdateCategoryTable([FromRoute] string id, [FromBody] CategoryModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _categoryService.UpdateCategoryTableAsync(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa danh mục theo tên bảng
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("table/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0123")]
        public async Task<ActionResult<ApiResultModel>> DeleteCategoryTableAsync([FromRoute] string id, string tableName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _categoryService.DeleteCategoryTableAsync(id, tableName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin danh mục theo id
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        [HttpGet]
        [Route("table/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0122")]
        public async Task<ActionResult<CategoryCreateModel>> GetCategoryTableById([FromRoute] string id, string tableName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _categoryService.GetCategoryTableByIdAsync(id, tableName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách thứ tự hiển thị
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("table/get-list-oder")]
        [AllowPermission(Permissions = "F0121;F0122")]
        public async Task<ActionResult<ApiResultModel>> GetListOrderTable(string id, string tableName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _categoryService.GetListOrderTableAsync(id, tableName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
