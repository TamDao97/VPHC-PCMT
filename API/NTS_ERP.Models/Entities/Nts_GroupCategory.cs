using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_GroupCategory
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Index { get; set; }
}
