using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Province
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool Border { get; set; }

    public string HCKey { get; set; } = null!;
}
