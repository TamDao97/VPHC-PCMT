using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_MenuSystem
{
    public string Id { get; set; } = null!;

    public string TitleDefault { get; set; } = null!;

    public string? TitleKeyTranslate { get; set; }

    public string? Icon { get; set; }

    public string? Url { get; set; }

    public string? ParentId { get; set; }

    public bool IsDisable { get; set; }

    public int Index { get; set; }

    public bool IsDefaultMenu { get; set; }

    public bool FunctionAuto { get; set; }

    public string? SystemFunctionConfigId { get; set; }
}
