using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;

namespace NTS_ERP.Models.VPHC.BienBan
{
    public class BienBanXuatModel
    {
        public string DonViCapTren { get; set; }
        public string DonVi { get; set; }
        public string So { get; set; }
        public string GioLap { get; set; }
        public string PhutLap { get; set; }
        public string NgayLap { get; set; }
        public string LinhVuc { get; set; } = "";
        public string? DiaDiemLap { get; set; }
        public string? LyDoLapChoKhac { get; set; }
        public string? CanCu { get; set; }
        public string? TenCanBoLap { get; set; }
        public string? ChucVuCBL { get; set; }
        public string? DonViCBL { get; set; }
        public string? IdNguoiChungKien { get; set; }
        public string? TenNguoiCK { get; set; }
        /// <summary>
        /// Lable Nghề nghiệp, chức vụ
        /// </summary>
       public string? NgheChuc { get; set; }
        public string? NgheNguoiCK { get; set; }
        /// <summary>
        /// Lable Địa chỉ, cơ quan
        /// </summary>
        public string? DiaChiCoQuan { get; set; }
        public string? DiaChiNguoiCK { get; set; }
        public string? TenPhienDich { get; set; }
        public string? NgheNghiepPD { get; set; }
        public string? DiaChiPD { get; set; }
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

        public string? YKienNguoiChungKien { get; set; }

        public string? YKienBenThietHai { get; set; }
        public string? ThietHai { get; set; }
        public string? YKienViPham { get; set; }
        public string? BienPhapNganChan { get; set; }
        /// <summary>
        /// Ông hoặc bà
        /// </summary>
        public string? OngBa { get; set; }
        /// <summary>
        /// Cá nhân hoặc Người đại diện của tổ chức
        /// </summary>
        public string? CaNhanDaiDien { get; set; }
        public string? CoQuanGiaiTrinh { get; set; }
        public string? GioGiaiQuyet { get; set; }
        public string? PhutGiaiQuyet { get; set; }
        public string NgayGiaiQuyet { get; set; }
        public string? CoQuanGiaiQuyet { get; set; }
        public string GioLapXong { get; set; }
        public string PhutLapXong { get; set; }
        public string NgayLapXong { get; set; }
        public int? SoTo { get; set; }
        public int? SoBanIn { get; set; }
        public string? TenNguoiGiamHo { get; set; }
        public string? LyDoViPhamKhongKy { get; set; }
        public string? LyDoChungKienKhongKy { get; set; }
        /// <summary>
        /// Ông/ba chứng kiến
        /// </summary>
        public string? OngBaCK { get; set; }
        /// <summary>
        /// Người chứng kiến/ đại diện chính quyền cấp xã
        /// </summary>
        public string? NguoiDaiDienCK { get; set; }
    }
}