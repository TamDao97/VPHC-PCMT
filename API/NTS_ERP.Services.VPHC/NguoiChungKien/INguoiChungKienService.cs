using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.NguoiChungKien
{
    public interface INguoiChungKienService
    {
        Task<SearchBaseResultModel<NguoiChungKienResultModel>> Search(NguoiChungKienSearchModel searchModel);
        Task Create(NguoiChungKienModifyModel model, string userId);
        Task<NguoiChungKienModifyModel> GetById(string id);
        Task Update(string id, NguoiChungKienModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<NguoiChungKienModifyModel> models, string idVuViec, string userId);
        List<NguoiChungKienModifyModel> GetNguoiChungKien(NTS_ERPContext sqlContext, string idVuViec);
    }
}
