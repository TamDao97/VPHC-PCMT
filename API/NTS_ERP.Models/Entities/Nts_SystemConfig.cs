using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nts_SystemConfig
{
    public string Id { get; set; } = null!;

    public string SoftwareName { get; set; } = null!;

    public bool IsLoginCaptcha { get; set; }

    public bool IsMultiLanguage { get; set; }

    public bool ShowLogoTopBar { get; set; }

    public string Logo { get; set; } = null!;

    public string LogoFolded { get; set; } = null!;

    public string FaviconIcon { get; set; } = null!;

    public int MenuType { get; set; }
}
