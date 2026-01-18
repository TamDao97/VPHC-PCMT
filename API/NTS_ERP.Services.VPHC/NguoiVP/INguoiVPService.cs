using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.Cores.Auth;

namespace NTS_ERP.Services.VPHC.NguoiVP
{
    public interface INguoiVPService
    {
        Task<SearchBaseResultModel<NguoiVPResultModel>> Search(NguoiVPSearchModel searchModel);
        Task Create(NguoiVPModifyModel model, string userId);
        Task<NguoiVPModifyModel> GetById(string id);
        Task Update(string id, NguoiVPModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<NguoiVPModifyModel>? models, string idVuViec, CurrentUserModel currentUser);
        Task AddXacMinh(NTS_ERPContext sqlContext, List<NguoiVPModifyModel> models, string idVuViec, CurrentUserModel currentUser);
        List<NguoiVPModifyModel> GetNguoiVP(NTS_ERPContext sqlContext, string idVuViec);
    }
}
