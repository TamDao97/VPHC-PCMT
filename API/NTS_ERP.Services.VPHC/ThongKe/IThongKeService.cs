using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.VPHC.ThongKe;

namespace NTS_ERP.Services.VPHC.ThongKe
{
    public interface IThongKeService
    {
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<ThongKeResultModel> ThongKe(ThongKeSearchModel searchModel);
    }
}
