using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class BienBanVPHC
{
    public string IdBienBan { get; set; } = null!;

    public string IdDanhMucBienBan { get; set; } = null!;

    public string IdVuViec { get; set; } = null!;

    public string So { get; set; } = null!;

    public DateTime ThoiGianLap { get; set; }

    public string LinhVuc { get; set; } = null!;

    public int LapTai { get; set; }

    public string? DiaDiemLap { get; set; }

    public string? LyDoLapChoKhac { get; set; }

    public string? CanCu { get; set; }

    public string? IdCanBoLap { get; set; }

    public int? ChuTheChungKien { get; set; }

    public string? IdNguoiChungKien { get; set; }

    public string? IdPhienDichVien { get; set; }

    public int? DoiTuongViPham { get; set; }

    public string? IdNguoiViPham { get; set; }

    public string? IdToChucViPham { get; set; }

    public string? HanhViViPham { get; set; }

    public string? QuyDinhTai { get; set; }

    public string? ThietHai { get; set; }

    public string? YKienViPham { get; set; }

    public string? YKienNguoiChungKien { get; set; }

    public string? YKienBenThietHai { get; set; }

    public string? BienPhapNganChan { get; set; }

    public string? CoQuanGiaiTrinh { get; set; }

    public DateTime? ThoiGianGiaiQuyet { get; set; }

    public string? CoQuanGiaiQuyet { get; set; }

    public DateTime? ThoiGianLapXong { get; set; }

    public int? SoTo { get; set; }

    public int? SoBanIn { get; set; }

    public string? LyDoViPhamKhongKy { get; set; }

    public string? LyDoChungKienKhongKy { get; set; }
}
