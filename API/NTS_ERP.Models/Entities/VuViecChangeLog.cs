using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class VuViecChangeLog
{
    public long Id { get; set; }

    public string ObjectId { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string Data { get; set; } = null!;

    public int Version { get; set; }

    public string UpdateBy { get; set; } = null!;

    public DateTime UpdateTime { get; set; }

    public string HistoryLogId { get; set; } = null!;
}
