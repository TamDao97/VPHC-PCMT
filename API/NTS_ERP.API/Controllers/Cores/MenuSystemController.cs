using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Menu;
using NTS_ERP.Services.Cores.Menu;

namespace NTS_ERP.Api.Controllers.Cores
{

    [Route("api/menu")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class MenuSystemController : BaseApiController
    {
        private readonly IMenuSystemService _menuService;
        public MenuSystemController(IMenuSystemService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// search menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        //[AllowPermission(Permissions = "F0005")]
        public async Task<ActionResult<ApiResultModel>> SearchMenu([FromBody] MenuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            //apiResultModel.Data = await _menuService.GetMenuAsync(GetUserIdByRequest());
            apiResultModel.Data = await _menuService.SearchMenuAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


        /// <summary>
        /// lấy danh sách menu cho left bar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<ApiResultModel>> GetMenu()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetMenuByUserAsync(CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xem chi tiết menu theo id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id/{id}")]
        [AllowPermission(Permissions = "F0005")]
        public async Task<ActionResult<ApiResultModel>> GetMenuById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetMenuByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// thêm menu
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [AllowPermission(Permissions = "F0001")]
        public async Task<ActionResult<ApiResultModel>> CreateMenu(CreateMenuModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.CreateMenuAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// sửa menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        [AllowPermission(Permissions = "F0002")]
        public async Task<ActionResult<ApiResultModel>> UpdateMenu([FromRoute] string id, [FromBody] CreateMenuModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.UpdateMenuAsync(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Ẩn hiện menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        [HttpPut]
        [Route("disable/{id}")]
        [AllowPermission(Permissions = "F0004")]
        public async Task<ActionResult<ApiResultModel>> DisableMenu([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.DisbaleMenuAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0003")]
        public async Task<ActionResult<ApiResultModel>> DeletemENUy([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _menuService.DeleteMenuByIdAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// thêm menu
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-index")]
        [AllowPermission(Permissions = "F0002")]
        public async Task<ActionResult<ApiResultModel>> UpdateIndexMenu(List<UpdateIndexMenuModel> model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.UpdateIndexMenu(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list-funtion-auto")]
        public async Task<ActionResult<ApiResultModel>> GetListFuntionAuto()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetListFuntionAuto();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
