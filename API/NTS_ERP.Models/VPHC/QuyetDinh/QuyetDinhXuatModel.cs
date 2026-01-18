using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;

namespace NTS_ERP.Models.VPHC.QuyetDinh
{
    public class QuyetDinhXuatModel
    {
        public string DonViCapTren { get; set; }
        public string DonVi { get; set; }
        public string So { get; set; }
        public string TinhQD { get; set; }
        public string NgayQD { get; set; }
        public string ThangQD { get; set; }
        public string NamQD { get; set; }
        public string? ThamQuyenXuPhat { get; set; }
        public string? CanCu { get; set; }
        public string? SoBB { get; set; }
        public string? NgayBB { get; set; }
        /// <summary>
        /// Cá nhân hoặc Tổ chức
        /// </summary>
        public string DoiTuongVP { get; set; }
        public string? TenNguoiVP { get; set; }
        /// <summary>
        /// Giới tính vi phạm
        /// </summary>
        public string? GioiVP { get; set; }
        public string? NgaySinhNguoiVP { get; set; }
        /// <summary>
        /// Quốc tịch người vi phạm
        /// </summary>
        public string? QTichVP { get; set; }
        public string? NgheNghiepNguoiVP { get; set; }
        public string? NoiONguoiVP { get; set; }
        public string? SoCCCDNguoiVP { get; set; }
        public string? NgayCapCCCDNguoiVP { get; set; }
        public string? NoiCapCCCD { get; set; }
        public string? TenToChucVP { get; set; }
        public string? DiaChiTruSoToChucVP { get; set; }
        public string? MaSoToChucVP { get; set; }
        public string? GiayPhepTCVP { get; set; }
        public string? NgayCapGiayPTCVP { get; set; }
        public string? NoiCapGiayPTCVP { get; set; }
        public string? TenNguoiTCVP { get; set; }
        public string? GioiDD { get; set; }
        public string? ChucDanhNguoiDaiDienTCVP { get; set; }
        public string? HanhViViPham { get; set; }
        public string? QuyDinhTai { get; set; }

       
        /// <summary>
        /// Ông hoặc bà
        /// </summary>
        public string? OngBa { get; set; }
        /// <summary>
        /// Cá nhân hoặc Người đại diện của tổ chức
        /// </summary>
        public string? CaNhanDaiDien { get; set; }

        public string? TinhTietTangNang { get; set; }
        public string? TinhTietGiamNhe { get; set; }
        public string? PhatChinh { get; set; }
        public string CuThePC { get; set; }
        public string? PhatBoSung { get; set; }
        public string CuThePBS { get; set; }
        public string SoNgayThucHienPBS { get; set; }
        public string KhacPhucHauQua { get; set; }
        public string? CuTheKPHQ { get; set; }
        public string? SoNgayThucHienKPHQ { get; set; }
        public string? NoiDungLienQuanKPHQ { get; set; }
        public string? ChiPhiKPHQ { get; set; }
        public string? ChiPhiKPHQText { get; set; }
        public string? CoQuanThucHienKPHQ { get; set; }

        public string? DiaDiemNopPhat { get; set; }

        public string? HanNopPhat { get; set; }

        public string? DonViThuTienPhat { get; set; }
        public string? DonViThucHien { get; set; }
        public string? DonViPhoiHop { get; set; }
        public string? ChucVuKy { get; set; }
    }
}