using NTS_ERP.Models.Cores.Function;
using NTS_ERP.Models.Cores.Menu;
using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Menu
{
    public interface IMenuSystemService
    {   /// <summary>
        /// search menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<MenuViewModel>> SearchMenuAsync(MenuSearchModel searchModel);

        /// <summary>
        /// lấy danh sách menu cho left bar
        /// </summary>
        /// <returns></returns>
        Task<List<MenuViewModel>> GetMenuByUserAsync(string userId);

        /// <summary>
        /// Xem chi tiết menu theo id
        /// </summary>
        /// <returns></returns>
        Task<MenuViewModel> GetMenuByIdAsync(string id);
        Task CreateMenuAsync(CreateMenuModel model);

        /// <summary>
        /// sửa menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        Task UpdateMenuAsync(string id, CreateMenuModel model);

        /// <summary>
        /// Ẩn hiện menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        Task DisbaleMenuAsync(string id, string userId);

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        Task DeleteMenuByIdAsync(string id, string userId);

        Task UpdateIndexMenu(List<UpdateIndexMenuModel> listMenu);

        /// <summary>
        /// Danh sách các quyền cho chức năng động
        /// </summary>
        /// <returns></returns>
        Task<List<FunctionAutoModel>> GetListFuntionAuto();
    }
}
