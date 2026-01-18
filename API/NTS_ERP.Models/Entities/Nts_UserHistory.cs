using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_UserHistory
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Content { get; set; }

    public string? ClientIP { get; set; }

    public string? OS { get; set; }

    public string? BrowserVersion { get; set; }

    public string? BrowserName { get; set; }

    public string? Device { get; set; }

    public DateTime CreateDate { get; set; }

    public int Type { get; set; }
}
