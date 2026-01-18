using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.ToChucVP;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.ToChucVP
{
    public interface IToChucVPService
    {
        Task<SearchBaseResultModel<ToChucVPResultModel>> Search(ToChucVPSearchModel searchModel);
        Task Create(ToChucVPModifyModel model, string userId);
        Task<ToChucVPModifyModel> GetById(string id);
        Task Update(string id, ToChucVPModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<ToChucVPModifyModel>? models, string idVuViec, string userId);
        Task AddXacMinh(NTS_ERPContext sqlContext, List<ToChucVPModifyModel> models, string idVuViec, string userId);
        List<ToChucVPModifyModel> GetToChucVP(NTS_ERPContext sqlContext, string idVuViec);
    }
}
