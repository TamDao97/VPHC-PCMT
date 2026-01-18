using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.TangVat;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.TangVat
{
    public interface ITangVatService
    {
        Task<SearchBaseResultModel<TangVatResultModel>> Search(TangVatSearchModel searchModel);
        Task Create(TangVatModifyModel model, string userId);
        Task<TangVatModifyModel> GetById(string id);
        Task Update(string id, TangVatModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<TangVatModifyModel> models, string idVuViec, string userId);
        Task AddXuLy(NTS_ERPContext sqlContext, List<TangVatModifyModel> models, string idVuViec, string userId);
        List<TangVatModifyModel> GetTangVat(NTS_ERPContext sqlContext, string idVuViec);
    }
}
