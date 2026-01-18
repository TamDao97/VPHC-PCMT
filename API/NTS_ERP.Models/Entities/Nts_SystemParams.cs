using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_SystemParams
{
    public string Id { get; set; } = null!;

    public string? SystemParamGroupId { get; set; }

    public string ParamName { get; set; } = null!;

    public string? ParamValue { get; set; }

    public string? DisplayName { get; set; }

    public int Index { get; set; }

    public int? ControlType { get; set; }
}
