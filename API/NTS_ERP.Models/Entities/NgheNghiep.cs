using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class NgheNghiep
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
