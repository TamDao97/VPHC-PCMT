using NTS_ERP.Models.Cores.Combobox;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.SystemFunction;
using NTS_ERP.Models.Cores.About;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.SystemFunction
{
    public interface ISystemFunctionService
    {
        /// <summary>
        /// Danh sách bảng dữ liệu trên CSDL
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetTableNameAsync();

        /// <summary>
        /// Danh sách các column trong bảng
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <returns></returns>
        Task<List<SystemFunctionDesignModel>> GetColumnTableAsync(string tableName);

        /// <summary>
        /// Danh sách combobox column trong bảng
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetComboxColumnTable(string tableName);

        /// <summary>
        /// Tìm kiếm cấu hình chức năng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<FuntionConfigSearchResultModel>> SearchConfig(FuntionConfigSearchModel searchModel);

        /// <summary>
        /// Thêm mới cấu hình chức năng tự động
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Id chức năng sau khi thêm mới thành công</returns>
        Task<string> CreateFuntionConfig(SystemFunctionConfigModel model);

        /// <summary>
        /// Lấy thông tin cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <param name="id">Id chức năng tự động</param>
        /// <returns></returns>
        Task<FunctionConfigUpdateModel> GetFuntionConfigUpdateById(string id);

        /// <summary>
        /// Cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <param name="id">Id bản ghi cập nhật</param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdateFuntionConfig(string id, FunctionConfigUpdateModel model);

        /// <summary>
        /// Cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteFuntionConfig(string id);

        /// <summary>
        /// Lấy thông tin cấu hình chức năng tự động để hiển thị giao diện
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<FunctionConfigViewModel> GetFuntionConfigBySlug(string slug);

        /// <summary>
        /// Tìm kiếm dữ liệu trong chức năng tự động
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<object>> SearchDataAsync(string slug, object model);

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="slug">Tên bảng</param>
        /// <param name="model">Model thêm mới</param>
        /// <param name="userId">Id đang nhập</param>
        /// <returns></returns>
        Task<string> CreateDataAsync(string slug, object model, string userId);

        /// <summary>
        /// Lấy bản ghi cập nhật
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        Task<object> GetUpdateDataAsync(string slug, string id);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="slug">Tên bảng</param>
        /// <param name="id">Id bản ghi chỉnh sửa</param>
        /// <param name="model">Model thêm mới</param>
        /// <param name="userId">Id đang nhập</param>
        /// <returns></returns>
        Task<bool> UpdateDataAsync(string slug, string id, object model, string userId);

        /// <summary>
        /// Xem chi tiết bản ghi
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<object> GetDetailDataAsync(string slug, string id);

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="slug">Tên bảng</param>
        /// <param name="id">Id xóa</param>
        /// <param name="userid">Id đang nhập</param>
        /// <returns></returns>
        Task<bool> DeleteDataAsync(string slug, string id, string userid);

        /// <summary>
        /// Lấy dữ liệu cho tree view
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<List<TreeViewModel>> TreeViewDataAsync(string slug);
    }
}
