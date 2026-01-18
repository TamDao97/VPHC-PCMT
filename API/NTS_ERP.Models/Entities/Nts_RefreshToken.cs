using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_RefreshToken
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime IssueAt { get; set; }

    public DateTime ExpireAt { get; set; }
}
