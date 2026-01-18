using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.VPHC.TrangChu;

namespace NTS_ERP.Services.VPHC.TraCuu
{
    public interface ITrangChuService
    {
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<TrangChuResultModel> TinhHinhChung(TrangChuSearchModel searchModel);
    }
}
