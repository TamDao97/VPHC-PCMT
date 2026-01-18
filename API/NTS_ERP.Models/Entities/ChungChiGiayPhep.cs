using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class ChungChiGiayPhep
{
    public string IdChungChiGiayPhep { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public int SoLuong { get; set; }

    public string? GhiChu { get; set; }

    public int XuLy { get; set; }

    public string? TinhTrangDacDiem { get; set; }
}
