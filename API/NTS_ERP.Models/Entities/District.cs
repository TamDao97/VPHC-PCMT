using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class District
{
    public string Id { get; set; } = null!;

    public string ProvinceId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;
}
