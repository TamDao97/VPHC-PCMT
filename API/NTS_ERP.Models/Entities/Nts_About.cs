using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_About
{
    public string Id { get; set; } = null!;

    public string? Title { get; set; }

    public string Content { get; set; } = null!;
}
