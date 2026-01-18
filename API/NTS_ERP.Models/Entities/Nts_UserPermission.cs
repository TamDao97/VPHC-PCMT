using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_UserPermission
{
    public string Id { get; set; } = null!;

    public string FunctionId { get; set; } = null!;

    public string UserId { get; set; } = null!;
}
