using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.BienBan
{
    public class BienBanModifyModel
    {
        public string IdBienBan { get; set; } = "";
        public string IdDanhMucBienBan { get; set; } = "";
        public string IdVuViec { get; set; } = "";
        [Required]
        public string So { get; set; }
        [Required]
        public DateTime ThoiGianLap { get; set; }
        [Required]
        public string LinhVuc { get; set; } = "";
        public int LapTai { get; set; }
        public string? DiaDiemLap { get; set; }
        public string? LyDoLapChoKhac { get; set; }
        public string? CanCu { get; set; }
        [Required]
        public string? IdCanBoLap { get; set; }
        public int? ChuTheChungKien { get; set; }
        public string? IdNguoiChungKien { get; set; }
        public string? IdPhienDichVien { get; set; }
        public int? DoiTuongViPham { get; set; }
        public string? IdNguoiViPham { get; set; }
        public string? IdToChucViPham { get; set; }
        [Required]
        public string? HanhViViPham { get; set; }
        [Required]
        public string? QuyDinhTai { get; set; }

        public string? YKienNguoiChungKien { get; set; }

        public string? YKienBenThietHai { get; set; }
        public string? ThietHai { get; set; }
        public string? YKienViPham { get; set; }
        public string? BienPhapNganChan { get; set; }
        public string? CoQuanGiaiTrinh { get; set; }
        [Required]
        public DateTime? ThoiGianGiaiQuyet { get; set; }
        [Required]
        public string? CoQuanGiaiQuyet { get; set; }
        [Required]
        public DateTime? ThoiGianLapXong { get; set; }
        public int? SoTo { get; set; }
        public int? SoBanIn { get; set; }
        public string? LyDoViPhamKhongKy { get; set; }
        public string? LyDoChungKienKhongKy { get; set; }
    }
}