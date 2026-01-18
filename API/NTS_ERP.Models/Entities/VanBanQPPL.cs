using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class VanBanQPPL
{
    public string IdVanBanQPPL { get; set; } = null!;

    public string IdCoQuanBanHanh { get; set; } = null!;

    public string IdLoaiVanBan { get; set; } = null!;

    public string DonViNhapLieu { get; set; } = null!;

    public string So { get; set; } = null!;

    public DateTime ThoiGianBanHanh { get; set; }

    public string? LinhVucVanBan { get; set; }

    public string? TrichYeu { get; set; }

    public bool HieuLuc { get; set; }

    public string? VanBanLienQuan { get; set; }

    public string? AttachFile { get; set; }

    public string? GhiChu { get; set; }

    public string? IdLinhVuc { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsDelete { get; set; }

    public virtual LoaiVanBan IdLoaiVanBanNavigation { get; set; } = null!;
}
