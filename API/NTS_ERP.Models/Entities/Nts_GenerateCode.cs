using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_GenerateCode
{
    public string Id { get; set; } = null!;

    public string MaDuAn { get; set; } = null!;

    public string TenDuAn { get; set; } = null!;

    public string Namespace { get; set; } = null!;

    public string Sql_Server { get; set; } = null!;

    public string Sql_Database { get; set; } = null!;

    public string Sql_User { get; set; } = null!;

    public string Sql_Password { get; set; } = null!;

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }
}
