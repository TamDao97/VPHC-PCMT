using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.TaiLieu;

namespace NTS_ERP.Services.VPHC.TaiLieu
{
    public interface ITaiLieuService
    {
        Task<SearchBaseResultModel<TaiLieuResultModel>> Search(TaiLieuSearchModel searchModel);
        Task Create(TaiLieuModifyModel model, string userId);
        Task<TaiLieuModifyModel> GetById(string id);
        Task Delete(string id, string userId);
        List<DanhMucTaiLieuModle> GetDanhMuc();
        List<DanhMucTaiLieuModle> GetDanhMucTotal(string idVuViec);
        List<TaiLieuVuViecModel> GetTaiLieuVuViec(string id,string? idDanhMuc);

        FileStreamResultModel GetFile(string id);
    }
}
