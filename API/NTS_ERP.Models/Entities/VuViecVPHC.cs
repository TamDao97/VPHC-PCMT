using System;
using System.Collections.Generic;

namespace NTS_ERP.Models.Entities;

public partial class VuViecVPHC
{
    public string Id { get; set; } = null!;

    public string IdDonVi { get; set; } = null!;

    public string? MaHoSo { get; set; }

    public string IdNguonPhatHien { get; set; } = null!;

    public DateTime ThoiGianTiepNhan { get; set; }

    public string? DienBien { get; set; }

    public string? GhiChu { get; set; }

    public int LoaiDiaDiem { get; set; }

    public string IdTinhPhatHien { get; set; } = null!;

    public string? IdHuyenPhatHien { get; set; }

    public string? IdXaPhatHien { get; set; }

    public string? DiaChiChiTiet { get; set; }

    public string? DiaChiDayDu { get; set; }

    public int PhanLoai { get; set; }

    public string? IdLinhVucBCTH { get; set; }

    public string? CanBoDieuTra { get; set; }

    public string? KetQuaDieuTra { get; set; }

    public int TongNguoiVP { get; set; }

    public int TongToChucVP { get; set; }

    public int TongBienBan { get; set; }

    public int TongQuyetDinh { get; set; }

    public int TongTaiLieu { get; set; }

    public decimal TongTienPhat { get; set; }

    public string IdXuLy { get; set; } = null!;

    public int TienTrinhHoSo { get; set; }

    public bool IsDelete { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? ThietHai { get; set; }

    public string? MucDoThietHai { get; set; }

    public string? GiamNhe { get; set; }

    public string? TangNang { get; set; }

    public string? YKienNguoiViPham { get; set; }

    public string? YKienNguoiLamChung { get; set; }

    public string? YKienNguoiBiThietHai { get; set; }

    public string? XacMinhKhac { get; set; }

    public string? NguoiUyQuyen { get; set; }

    public string? SoQDUyQuyen { get; set; }

    public DateTime? NgayUyQuyen { get; set; }

    public string? DonViTiepNhanHS { get; set; }

    public string? SoBienBanHS { get; set; }

    public DateTime? NgayBanGiaoHS { get; set; }

    public string? DonViKhacXuLy { get; set; }

    public string? SoBienBanDVKhac { get; set; }

    public DateTime? NgayBanGiaoDVKhac { get; set; }

    public string? TaiLieuChuyenGiao { get; set; }

    public string? NoiDungNhacNho { get; set; }

    public string? LyDoKhongQDXP { get; set; }

    public int TongQDXuPhat { get; set; }

    public int TongQDThiHanh { get; set; }

    public int TongQDKhieuKien { get; set; }

    public int TongQDChuyen { get; set; }

    public int TongQDDangXuLy { get; set; }

    public int TongQDMienGiam { get; set; }

    public int TongQDCuongChe { get; set; }
}
