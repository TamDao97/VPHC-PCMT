using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class TangVatVPHC
{
    public string IdTangVatVPHC { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string? IdLoaiTangVat { get; set; }

    public string? IdDonViTinh { get; set; }

    public double? SoLuong { get; set; }

    public bool TiepNhan { get; set; }

    public bool BoSung { get; set; }

    public string? GhiChu { get; set; }

    public string? IdDonViTinhThuc { get; set; }

    public double? SoLuongThuc { get; set; }

    public int XuLy { get; set; }

    public string? Name { get; set; }

    public string? ChungLoai { get; set; }

    public string? TinhTrangDacDiem { get; set; }
}
