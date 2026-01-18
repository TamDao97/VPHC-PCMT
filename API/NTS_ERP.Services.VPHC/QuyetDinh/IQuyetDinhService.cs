using NTS.Common;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.QuyetDinh;

namespace NTS_ERP.Services.VPHC.QuyetDinh
{
    public interface IQuyetDinhService
    {
        Task<SearchBaseResultModel<QuyetDinhResultModel>> Search(QuyetDinhSearchModel searchModel);
        Task Create(QuyetDinhModifyModel model, string userId);
        Task<QuyetDinhModifyModel> GetById(string id);
        Task Update(string id, QuyetDinhModifyModel model, string userId);
        Task Delete(string id, string userId);
        List<DanhMucQuyetDinhModle> GetDanhMuc();
        QuyetDinhVuViecResultModel GetQuyetDinhVuViec(string id);

        MemoryStream ExportFileAsync(string id, string pathTemplate, NTSConstants.OptionExport optionExport);
    }
}
