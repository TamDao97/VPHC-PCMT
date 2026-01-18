using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class CanBo
{
    public string IdCanBo { get; set; } = null!;

    public string IdCapBac { get; set; } = null!;

    public string IdChucVu { get; set; } = null!;

    public string IdDonVi { get; set; } = null!;

    public string HoVaTen { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string? Anh { get; set; }

    public int GioiTinh { get; set; }

    public string? IdTinh { get; set; }

    public string? IdHuyen { get; set; }

    public string? IdXa { get; set; }

    public string? IdTinhHienNay { get; set; }

    public string? IdHuyenHienNay { get; set; }

    public string? IdXaHienNay { get; set; }

    public string? DiaChi { get; set; }

    public string? DiaChiDayDu { get; set; }

    public string? DiaChiHienNayDayDu { get; set; }

    public string IdDanToc { get; set; } = null!;

    public string IdTonGiao { get; set; } = null!;

    public DateTime NgayNhapNgu { get; set; }

    public DateTime? NgayVaoDang { get; set; }

    public string SoCMQD { get; set; } = null!;

    public string? SoCNDN { get; set; }

    public DateTime? NgayCapCNDN { get; set; }

    public int Nghach { get; set; }

    public int TrinhDo { get; set; }

    public string? ChucVuDaQua { get; set; }

    public int TinhTrang { get; set; }

    public DateTime ThoiGianVaoPCMT { get; set; }

    public DateTime? ThoiGianRaPCMT { get; set; }

    public DateTime? ThoiGianNghiHuu { get; set; }

    public DateTime? ThoiGianXuatNgu { get; set; }

    public string? LyLuanChinhTri { get; set; }

    public bool LucLuongTSKT { get; set; }

    public int? BienCheTSKT { get; set; }

    public bool DaoTaoNVTSKT { get; set; }

    public string? GhiChu { get; set; }

    public string? CreateBy { get; set; }

    public string? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsDelete { get; set; }
}
