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
    public interface IVuViecXacMinhService
    {
        /// <summary>
        /// Cập nhật vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateAsync(string id, VuViecXacMinhModifyModel model, CurrentUserModel currentUser);

        /// <summary>
        /// Lấy thông tin vụ việc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VuViecXacMinhModifyModel> GetByIdAsnyc(string id);
    }
}
