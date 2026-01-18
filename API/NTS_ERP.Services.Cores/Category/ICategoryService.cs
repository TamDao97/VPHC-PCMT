using NTS_ERP.Models.Cores.Category;
using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Categorys
{
    public interface ICategoryService
    {
        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        Task<List<GroupCategoryModel>> SearchGroupCategoryAsync();

        /// <summary>
        /// Thêm danh mục
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <param name="userId">Id người tạo</param>
        /// <returns></returns>
        Task CreateCategoryAsync(CategoryCreateModel model);

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <param name="model">Dữ liệu cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        Task UpdateCategoryAsync(string id, CategoryCreateModel model);

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        Task DeleteCategoryByIdAsync(string id);

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        Task<CategoryCreateModel> GetCategoryByIdAsync(string id);

        /// <summary>
        /// Lấy list order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<int>> GetListOrderAsync(string id);

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<CategoryTableDataModel>> SearchCategoryTableAsync(CategorySearchModel searchModel);

        /// <summary>
        /// Thêm mới dữ liệu theo tên bảng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateCategoryTableAsync(CategoryModel model);

        /// <summary>
        /// Cập nhật dữ liệu theo tên bảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateCategoryTableAsync(string id, CategoryModel model);

        /// <summary>
        /// Xóa dữ liệu theo tên bảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task DeleteCategoryTableAsync(string id, string tableName);

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        Task<CategoryModel> GetCategoryTableByIdAsync(string id, string tableName);

        /// <summary>
        /// Lấy list order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<int>> GetListOrderTableAsync(string id, string tableName);
    }
}
