using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class PhuongTienVPHC
{
    public string IdPhuongTienVPHC { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string IdLoaiPhuongTien { get; set; } = null!;

    public string BienSo { get; set; } = null!;

    public bool TiepNhan { get; set; }

    public bool BoSung { get; set; }

    public string? GhiChu { get; set; }

    public int XuLy { get; set; }

    public string? NhanHieu { get; set; }

    public string? XuatXu { get; set; }

    public string? TinhTrangDacDiem { get; set; }

    public string? Name { get; set; }

    public virtual LoaiPhuongTien IdLoaiPhuongTienNavigation { get; set; } = null!;
}
