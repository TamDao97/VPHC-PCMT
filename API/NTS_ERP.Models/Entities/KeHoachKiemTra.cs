using NTS_ERP.Models.Base;

namespace NTS_ERP.Models.Entities
{
    public class KeHoachKiemTra : BaseEntity
    {
        public string IdDonVi { get; set; }
        public string NoiDungKiemTra { get; set; }
        public string? SoQuyetDinhBanHanh { get; set; }
        public string CanCu { get; set; }
        public string MucDich { get; set; }
        public string YeuCau { get; set; }
        public DateTime TuNgayThucHienKeHoach { get; set; }
        public DateTime DenNgayThucHienKeHoach { get; set; }
        public int TrangThaiKeHoachKiemTra { get; set; }
    }
}
