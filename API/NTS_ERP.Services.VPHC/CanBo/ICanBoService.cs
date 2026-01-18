using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.CanBo;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.CanBo
{
    public interface ICanBoService
    {
        Task<List<CanBoChooseModel>> GetByIdDonVi(string id);
    }
}
