using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class Nguoi
{
    public string IdNguoi { get; set; } = null!;

    public string IdDonVi { get; set; } = null!;

    public string HoVaTen { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public int GioiTinh { get; set; }

    public string? CMND { get; set; }

    public string? IdQuocTich { get; set; }

    public string? IdDanToc { get; set; }

    public string? IdTinh { get; set; }

    public string? IdHuyen { get; set; }

    public string? IdXa { get; set; }

    public string? IdNgheNghiep { get; set; }

    public string? IdTinhHienNay { get; set; }

    public string? IdHuyenHienNay { get; set; }

    public string? IdXaHienNay { get; set; }

    public string? DiaChi { get; set; }

    public string? TrinhDoVanHoa { get; set; }

    public string? SoDienThoai { get; set; }

    public string? HinhAnh { get; set; }

    public bool DoiTuongHS { get; set; }

    public bool LaoDongTraiPhep { get; set; }

    public bool Nghien { get; set; }

    public bool DoiTuongMBN { get; set; }

    public bool DoiTuongKTNV { get; set; }

    public bool DoiTuongQLNV { get; set; }

    public bool LLM { get; set; }

    public bool DoiTuongBiBat { get; set; }

    public bool NguoiVPHC { get; set; }

    public bool NguoiVPHS { get; set; }

    public bool NanNhanMBN { get; set; }

    public bool ConSong { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? DiaChiDayDu { get; set; }

    public string? DiaChiHienNayDayDu { get; set; }

    public DateTime? NgayCap { get; set; }

    public string? IdTonGiao { get; set; }

    public string? NoiCap { get; set; }

    public bool DoiTuongCanChuY { get; set; }

    public bool DoiTuongXNC { get; set; }

    public bool DoiTuongTruyNa { get; set; }

    public string? AnhNghiengTrai { get; set; }

    public string? AnhNghiengPhai { get; set; }
}
