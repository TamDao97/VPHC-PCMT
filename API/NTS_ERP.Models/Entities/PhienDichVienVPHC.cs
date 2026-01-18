using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class PhienDichVienVPHC
{
    public string IdPhienDichVienVPHC { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string HoVaTen { get; set; } = null!;

    public int GioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? CMND { get; set; }

    public string? DiaChi { get; set; }

    public string? GhiChu { get; set; }

    public string? SoDienThoai { get; set; }

    public string? TrinhDoChuyenMon { get; set; }

    public DateTime? NgayCap { get; set; }

    public string? NoiCap { get; set; }

    public string? NgheNghiep { get; set; }
}
