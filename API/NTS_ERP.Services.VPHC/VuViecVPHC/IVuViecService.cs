using NTS.Common;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Function;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.VuViec;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.Cores.Auth;

namespace NTS_ERP.Services.VPHC.VuViecVPHC
{
    public interface IVuViecService
    {
        /// <summary>
        /// Tìm kiếm vụ việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<VuViecBaseResultModel> SearchAsync(VuViecSearchModel searchModel);

        /// <summary>
        /// Thêm mới vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> CreateAsync(VuViecModifyModel model, CurrentUserModel currentUser);

        /// <summary>
        /// Cập nhật vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateAsync(string id, VuViecModifyModel model, CurrentUserModel currentUser);

        /// <summary>
        /// Lấy thông tin vụ việc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VuViecModifyModel> GetByIdAsnyc(string id);

        /// <summary>
        /// Xóa vụ việc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEntityAsync(string id, string userid, bool isSoftDelete = true);

        /// <summary>
        /// Cập nhật vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task FinishAsync(string id, string userId);

        /// <summary>
        /// Xuất danh sách
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFileAsync(VuViecSearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport);

        /// <summary>
        /// Thống kê nhanh
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<List<TreeItem>> QuickDashboard(VuViecSearchModel searchModel);

        VuViecTrongBienBanModel GetVuViecTrongBienBan(string id);
        VuViecTrongQuyetDinhModel GetVuViecTrongQuyetDinh(string id);

        bool CheckViewNotify(string idViPham, string idDonVi);
    }
}
