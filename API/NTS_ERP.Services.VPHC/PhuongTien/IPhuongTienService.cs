using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.VPHC.PhuongTien;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.VPHC.PhuongTien
{
    public interface IPhuongTienService
    {
        Task<SearchBaseResultModel<PhuongTienResultModel>> Search(PhuongTienSearchModel searchModel);
        Task Create(PhuongTienModifyModel model, string userId);
        Task<PhuongTienModifyModel> GetById(string id);
        Task Update(string id, PhuongTienModifyModel model, string userId);
        Task Delete(string id, string userId);
        Task Add(NTS_ERPContext sqlContext, List<PhuongTienModifyModel> models, string idVuViec, string userId);
        Task AddXuLy(NTS_ERPContext sqlContext, List<PhuongTienModifyModel> models, string idVuViec, string userId);
        List<PhuongTienModifyModel> GetPhuongTien(NTS_ERPContext sqlContext, string idVuViec);
    }
}
