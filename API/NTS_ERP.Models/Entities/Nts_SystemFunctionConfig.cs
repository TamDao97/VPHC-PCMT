using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_SystemFunctionConfig
{
    public string Id { get; set; } = null!;

    public string FunctionName { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int Index { get; set; }

    public bool SearchDisplay { get; set; }

    public bool FilterDisplay { get; set; }

    public bool CreateDisplay { get; set; }

    public int CreateWindowType { get; set; }

    public string? CreateWindowWidth { get; set; }

    public bool EditDisplay { get; set; }

    public int EditWindowType { get; set; }

    public string? EditWindowWidth { get; set; }

    public bool DetailDisplay { get; set; }

    public int DetailWindowType { get; set; }

    public string? DetailWindowWidth { get; set; }

    public bool? DeleteDisplay { get; set; }

    public bool ImportDisplay { get; set; }

    public bool ExportDisplay { get; set; }

    public string? LinkTemplate { get; set; }

    public int? DataColumnStart { get; set; }

    public int? DataRowStart { get; set; }

    public int LayoutType { get; set; }

    public string? TreeName { get; set; }

    public string? TreeTableName { get; set; }

    public string? TreeColumnId { get; set; }

    public string? TreeColumnParentId { get; set; }

    public string? TreeColumnsText { get; set; }

    public string? TreeFunctionConfigId { get; set; }

    public string? ModuleName { get; set; }

    public string? FunctionGroup { get; set; }

    public string? SearchPermission { get; set; }

    public string? CreatePermission { get; set; }

    public string? EditPermission { get; set; }

    public string? DeletePermission { get; set; }

    public string? DetailPermission { get; set; }

    public string? ImportPermission { get; set; }

    public string? ExportPermission { get; set; }
}
