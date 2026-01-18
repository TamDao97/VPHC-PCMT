using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class ToChucVP
{
    public string IdToChucVP { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? DiaChiTruSo { get; set; }

    public string? MaSoDoanhNghiep { get; set; }

    public string? SoDKKD { get; set; }

    public DateTime? NgayCapDKKD { get; set; }

    public string? NoiCapDKKD { get; set; }

    public string? HoTenPhapNhan { get; set; }

    public int? GioiTinh { get; set; }

    public string? ChucVu { get; set; }

    public bool TiepNhan { get; set; }

    public bool BoSung { get; set; }

    public string? HanhViViPham { get; set; }

    public string? GhiChu { get; set; }

    public int KetLuanKiemTra { get; set; }
}
