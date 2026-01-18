using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.BienBan;

namespace NTS_ERP.Services.VPHC.BienBan
{
    public interface IBienBanService
    {
        Task<SearchBaseResultModel<BienBanResultModel>> Search(BienBanSearchModel searchModel);
        Task Create(BienBanModifyModel model, string userId);
        Task<BienBanModifyModel> GetById(string id);
        Task Update(string id, BienBanModifyModel model, string userId);
        Task Delete(string id, string userId);
        List<DanhMucBienBanModle> GetDanhMuc();
        List<BienBanVuViecModel> GetBienBanVuViec(string id);

        MemoryStream ExportFileAsync(string id, string pathTemplate, NTSConstants.OptionExport optionExport);
    }
}
