using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class QuyetDinhXuPhat
{
    public string IdQuyetDinh { get; set; } = null!;

    public string? IdDanhMucQuyetDinh { get; set; }

    public string IdVuViec { get; set; } = null!;

    public string So { get; set; } = null!;

    public DateTime NgayRaQD { get; set; }

    public string? CanCu { get; set; }

    public int? DoiTuongViPham { get; set; }

    public string? IdNguoiViPham { get; set; }

    public string? IdToChucViPham { get; set; }

    public string? HanhViViPham { get; set; }

    public string? QuyDinhTai { get; set; }

    public string? TinhTietTangNang { get; set; }

    public string? TinhTietGiamNhe { get; set; }

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

    public DateTime? NgayQDCoHieuLuc { get; set; }

    public int? HanNopPhat { get; set; }

    public string? DiaDiemNopPhat { get; set; }

    public string? DonViThuTienPhat { get; set; }

    public string? DonViThucHien { get; set; }

    public string? DonViPhoiHop { get; set; }
}
