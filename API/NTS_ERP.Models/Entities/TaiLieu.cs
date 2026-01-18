using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class TaiLieu
{
    public string Id { get; set; } = null!;

    public string IdVuViec { get; set; } = null!;

    public string IdCategory { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public decimal FileSize { get; set; }

    public string? Thumbnail { get; set; }

    public string Extention { get; set; } = null!;

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
