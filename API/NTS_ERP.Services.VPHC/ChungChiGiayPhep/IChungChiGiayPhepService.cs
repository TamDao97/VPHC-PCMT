using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.ChungChiGiayPhep;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.ChungChiGiayPhep
{
    public interface IChungChiGiayPhepService
    {
        Task<SearchBaseResultModel<ChungChiGiayPhepResultModel>> Search(ChungChiGiayPhepSearchModel searchModel);
        Task Create(ChungChiGiayPhepModifyModel model, string userId);
        Task<ChungChiGiayPhepModifyModel> GetById(string id);
        Task Update(string id, ChungChiGiayPhepModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<ChungChiGiayPhepModifyModel> models, string idVuViec, string userId);
        Task AddXuLy(NTS_ERPContext sqlContext, List<ChungChiGiayPhepModifyModel> models, string idVuViec, string userId);
        List<ChungChiGiayPhepModifyModel> GetChungChiGiayPhep(NTS_ERPContext sqlContext, string idVuViec);
    }
}
