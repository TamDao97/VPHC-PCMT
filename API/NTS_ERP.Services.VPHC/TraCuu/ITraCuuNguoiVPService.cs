using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.VPHC.TraCuu;

namespace NTS_ERP.Services.VPHC.TraCuu
{
    public interface ITraCuuNguoiVPService
    {
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<TraCuuBaseResultModel> SearchAsync(TraCuuSearchModel searchModel);

        /// <summary>
        /// Xuất danh sách
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFileAsync(TraCuuSearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport);

        /// <summary>
        /// Thống kê nhanh
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<List<TreeItem>> QuickDashboard(TraCuuSearchModel searchModel);
    }
}
