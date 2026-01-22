using NTS_ERP.Models.Base;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TraCuu;
using System.ComponentModel;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTra
{
    public class KeHoachKiemTraModel : BaseModel
    {
        public string? IdDonVi { get; set; }
        public string? NoiDungKiemTra { get; set; }
        public string? SoQuyetDinhBanHanh { get; set; }
        public string? CanCu { get; set; }
        public string? MucDich { get; set; }
        public string? YeuCau { get; set; }
        public DateTime TuNgayThucHienKeHoach { get; set; }
        public DateTime DenNgayThucHienKeHoach { get; set; }
        public int TrangThaiKeHoachKiemTra { get; set; } = 1;
    }

    public class KeHoachKiemTraCreateRequestModel : KeHoachKiemTraModel
    {
        public List<FileKeHoachKiemTraModel> DataFileChoDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
        public List<FileKeHoachKiemTraModel> DataFileDaDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();

        public List<string>? ListPath { get; set; }
    }

    public class KeHoachKiemTraUpdateRequestModel : KeHoachKiemTraModel
    {
        public List<FileKeHoachKiemTraModel> DataFileChoDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
        public List<FileKeHoachKiemTraModel> DataFileDaDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
    }

    public class KeHoachKiemTraDetailResponseModel : KeHoachKiemTraModel
    {
        public List<FileKeHoachKiemTraModel> DataFileChoDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
        public List<FileKeHoachKiemTraModel> DataFileDaDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
    }

    public class KeHoachKiemTraSearchModel : SearchBaseModel
    {
        public string? SoQuyetDinhBanHanh { get; set; }
        public string? IdDonVi { get; set; }
        public DateTime? NgayBanHanhKeHoachFrom { get; set; }
        public DateTime? NgayBanHanhKeHoachTo { get; set; }
        public string? CanCu { get; set; }
        public string? MucDich { get; set; }
        public string? YeuCau { get; set; }
        public DateTime? TuNgayThucHienKeHoach { get; set; }
        public DateTime? DenNgayThucHienKeHoach { get; set; }
    }

    public class KeHoachKiemTraSearchResultModel : KeHoachKiemTraModel
    {
        public int Index { get; set; } = 0;
        public string? DonVi { get; set; }
    }

    public class KeHoachKiemTraBaseResultModel : SearchBaseResultModel<KeHoachKiemTraSearchResultModel>
    {

    }
    public enum TrangThaiKHKTEnum
    {
        [Description("Chờ duyệt")] ChoDuyet = 1,
        [Description("Đã duyệt")] DaDuyet = 2,
    }
}
