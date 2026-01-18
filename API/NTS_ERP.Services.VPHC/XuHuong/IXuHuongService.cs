using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.VPHC.XuHuong;

namespace NTS_ERP.Services.VPHC.XuHuong
{
    public interface IXuHuongService
    {
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<XuHuongResultModel> PhanTichXuHuong(XuHuongSearchModel searchModel);
    }
}
