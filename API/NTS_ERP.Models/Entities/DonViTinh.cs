using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class DonViTinh
{
    public string IdDonViTinh { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public int Type { get; set; }

    public int Order { get; set; }

    public bool DonViCoBan { get; set; }

    public string? IdDonViCoBan { get; set; }

    public double? TyLeDonViCB { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
