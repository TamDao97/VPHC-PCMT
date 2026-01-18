using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.PhienDich;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.PhienDich
{
    public interface IPhienDichService
    {
        Task<SearchBaseResultModel<PhienDichResultModel>> Search(PhienDichSearchModel searchModel);
        Task Create(PhienDichModifyModel model, string userId);
        Task<PhienDichModifyModel> GetById(string id);
        Task Update(string id, PhienDichModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<PhienDichModifyModel> models, string idVuViec, string userId);
        List<PhienDichModifyModel> GetPhienDich(NTS_ERPContext sqlContext, string idVuViec);
    }
}
