using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class NguoiVPHC
{
    public string IdNguoiVPHC { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string IdNguoi { get; set; } = null!;

    public int Tuoi { get; set; }

    public bool HoanCanhKhoKhan { get; set; }

    public string? ChiTietHoanCanh { get; set; }

    public bool? TungLaNanNhanMBN { get; set; }

    public bool TiepNhan { get; set; }

    public bool BoSung { get; set; }

    public string? HanhViViPham { get; set; }

    public string? GhiChu { get; set; }

    public bool TreDuoi18 { get; set; }

    public int KetLuanKiemTra { get; set; }

    public string? QuanHeGH { get; set; }

    public string? HoVaTenGH { get; set; }

    public string? DiaChiGH { get; set; }
}
