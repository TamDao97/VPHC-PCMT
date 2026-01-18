using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class LoaiTangVatDVT
{
    public string IdLoaiTangVatDVT { get; set; } = null!;

    public string IdLoaiTangVat { get; set; } = null!;

    public string IdDonViTinh { get; set; } = null!;

    public int Order { get; set; }
}
