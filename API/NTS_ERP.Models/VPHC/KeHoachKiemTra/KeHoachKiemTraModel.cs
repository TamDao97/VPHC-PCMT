using NTS_ERP.Models.Base;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TraCuu;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTra
{
    public class KeHoachKiemTraModel : BaseModel
    {
        public string IdDonVi { get; set; }
        public string NoiDungKiemTra { get; set; }
        public int NamThucHienKeHoach { get; set; }
        public string SoQuyetDinhBanHanh { get; set; }
        public DateTime NgayBanHanhKeHoach { get; set; }
        public string CanCu { get; set; }
        public string MucDich { get; set; }
        public string YeuCau { get; set; }
        public DateTime TuNgayThucHienKeHoach { get; set; }
        public DateTime DenNgayThucHienKeHoach { get; set; }
        public string DiaBanKiemTraTheoKeHoach { get; set; }
        public string ThanhPhanLucLuongKiemTra { get; set; }
        public string PhanCongNhiemVu { get; set; }
        public string DieuKienPhucVuKiemTra { get; set; }
        public string CheDoBaoCao { get; set; }
    }

    public class KeHoachKiemTraCreateRequestModel : KeHoachKiemTraModel
    {

    }

    public class KeHoachKiemTraUpdateRequestModel : KeHoachKiemTraModel
    {

    }

    public class KeHoachKiemTraDetailResponseModel : KeHoachKiemTraModel
    {

    }

    public class KeHoachKiemTraSearchModel : SearchBaseModel
    {
        public string? SoQuyetDinhBanHanh { get; set; }
        public string? IdDonVi { get; set; }
        public int? NamThucHienKeHoach { get; set; }
        public DateTime? NgayBanHanhKeHoachFrom { get; set; }
        public DateTime? NgayBanHanhKeHoachTo { get; set; }
        public string? CanCu { get; set; }
        public string? MucDich { get; set; }
        public string? YeuCau { get; set; }
        public DateTime? NgayBanHanhKeHoach { get; set; }
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
}
