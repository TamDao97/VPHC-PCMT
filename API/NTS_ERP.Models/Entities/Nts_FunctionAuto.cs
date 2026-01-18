using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_FunctionAuto
{
    public string Id { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Index { get; set; }
}
