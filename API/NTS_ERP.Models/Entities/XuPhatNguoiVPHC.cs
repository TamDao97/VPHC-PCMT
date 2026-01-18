using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class XuPhatNguoiVPHC
{
    public string IdXuPhatNguoiVPHC { get; set; } = null!;

    public string IdViPhamHC { get; set; } = null!;

    public string IdNguoiVPHC { get; set; } = null!;

    public string IdLinhVucViPham { get; set; } = null!;

    public string IdNghiDinh { get; set; } = null!;

    public string? DieuKhoanDiem { get; set; }

    public string? HanhViViPham { get; set; }

    public string IdThamQuyenXPVPHC { get; set; } = null!;

    public string IdHinhThucPhat { get; set; } = null!;

    public string? SoQuyetDinh { get; set; }

    public DateTime? NgayQuyetDinh { get; set; }

    public decimal? MucPhat { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool KhongThuDuocTienPhat { get; set; }
}
