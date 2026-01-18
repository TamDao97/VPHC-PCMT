using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_MenuSystemPermission
{
    public string Id { get; set; } = null!;

    public string FunctionId { get; set; } = null!;

    public string MenuSystemId { get; set; } = null!;
}
