using NTS.Common;
using NTS_ERP.Models.Cores.Common;
using System.IO;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.UserHistory;

namespace NTS_ERP.Services.Cores.UserHistorys
{
    public interface IUserHistoryService
    {
        Task<SearchBaseResultModel<UserHistorySearchResultModel>> SearchHistoryAsync(UserHistorySearchModel searchModel);

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFileAsync(UserHistorySearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport);

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFileClientAsync(UserHistorySearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport);
    }
}
