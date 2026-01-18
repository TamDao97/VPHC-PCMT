using NTS_ERP.Models.Base;

namespace NTS_ERP.Models.Entities
{
    public class KeHoachKiemTra : BaseEntity
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
}
