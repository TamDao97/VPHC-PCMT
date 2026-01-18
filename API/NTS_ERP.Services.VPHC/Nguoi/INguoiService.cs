using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.Nguoi;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.Cores.Auth;

namespace NTS_ERP.Services.VPHC.Nguoi
{
    public interface INguoiService
    {
        Task<SearchBaseResultModel<NguoiResultModel>> Search(NguoiSearchModel searchModel);
        Task Create(NguoiModifyModel model, string userId);
        Task<NguoiModifyModel> GetById(string id);
        Task Update(string id, NguoiModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task<string> Add(NTS_ERPContext sqlContext, NguoiModifyModel model, CurrentUserModel currentUser);
    }
}
