using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_FileTemplate
{
    public string Id { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? DefaultFilePath { get; set; }

    public int? Index { get; set; }

    public string? Description { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
