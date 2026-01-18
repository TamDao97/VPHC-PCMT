using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.QuyetDinh
{
    public class QuyetDinhModifyModel
    {
        public string IdQuyetDinh { get; set; } = "";
        public string IdDanhMucQuyetDinh { get; set; } = "";
        public string IdVuViec { get; set; } = "";
        [Required]
        public string So { get; set; } = null!;
        [Required]
        public DateTime NgayRaQD { get; set; }
        [Required]
        public string? CanCu { get; set; }

        public int? DoiTuongViPham { get; set; }

        public string? IdNguoiViPham { get; set; }

        public string? IdToChucViPham { get; set; }
        [Required]
        public string? HanhViViPham { get; set; }
        [Required]
        public string? QuyDinhTai { get; set; }

        public string? TinhTietTangNang { get; set; }

        public string? TinhTietGiamNhe { get; set; }
        [Required]
        public string? PhatChinh { get; set; }

        public string? CuThePC { get; set; }

        public decimal? MucPhat { get; set; }

        public string? MucPhatText { get; set; }

        public string? PhatBoSung { get; set; }

        public string? CuThePBS { get; set; }

        public int? SoNgayThucHienPBS { get; set; }

        public string? KhacPhucHauQua { get; set; }

        public string? CuTheKPHQ { get; set; }

        public int? SoNgayThucHienKPHQ { get; set; }

        public string? NoiDungLienQuanKPHQ { get; set; }

        public decimal? ChiPhiKPHQ { get; set; }

        public string? ChiPhiKPHQText { get; set; }

        public string? CoQuanThucHienKPHC { get; set; }
        [Required]
        public DateTime? NgayQDCoHieuLuc { get; set; }

        public int? HanNopPhat { get; set; }

        public string? DiaDiemNopPhat { get; set; }

        public string? DonViThuTienPhat { get; set; }

        public string? DonViThucHien { get; set; }

        public string? DonViPhoiHop { get; set; }
    }
}