using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class DonVi
{
    public string IdDonVi { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? IdLoaiDonVi { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public int? BienCheSQ { get; set; }

    public int? HienCoSQ { get; set; }

    public int? BienCheQNCN { get; set; }

    public int? HienCoQNCN { get; set; }

    public int? BienCheHSQCS { get; set; }

    public int? HienCoHSQCS { get; set; }

    public int? BienCheCNV { get; set; }

    public int? HienCoCNV { get; set; }

    public string? IdTinh { get; set; }

    public string? IdHuyen { get; set; }

    public string? IdXa { get; set; }

    public string? IdDonViCha { get; set; }

    public int Order { get; set; }

    public int Level { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
