using NTS_ERP.Models.VPHC.ChungChiGiayPhep;
using NTS_ERP.Models.VPHC.Nguoi;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.PhienDich;
using NTS_ERP.Models.VPHC.PhuongTien;
using NTS_ERP.Models.VPHC.TangVat;
using NTS_ERP.Models.VPHC.ToChucVP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecModifyModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Đơn vị mở hồ sơ là bắt buộc.")]
        public string? IdDonVi { get; set; }

        public string? MaHoSo { get; set; }
        [Required(ErrorMessage = "Nguồn phát hiện là bắt buộc.")]
        public string? IdNguonPhatHien { get; set; }

        [Required(ErrorMessage = "Thời gian tiếp nhận là bắt buộc.")]
        public DateTime? ThoiGianTiepNhan { get; set; }
        [Required(ErrorMessage = "Tóm tắt diễn biến là bắt buộc.")]
        public string? DienBien { get; set; }

        public string? GhiChu { get; set; }
        [Required(ErrorMessage = "Loại địa điểm là bắt buộc.")]
        public int LoaiDiaDiem { get; set; } = 0;
        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc.")]
        public string? IdTinhPhatHien { get; set; }
        [Required(ErrorMessage = "Quận/Huyện là bắt buộc.")]
        public string? IdHuyenPhatHien { get; set; }

        public string? IdXaPhatHien { get; set; }

        public string? DiaChiChiTiet { get; set; }

        public string? DiaChiDayDu { get; set; }
        [Required(ErrorMessage = "Phân loại là bắt buộc.")]

        public int? PhanLoai { get; set; }
        [Required(ErrorMessage = "Lĩnh vực thống kê báo cáo là bắt buộc.")]

        public string? IdLinhVucBCTH { get; set; }

        public string? CanBoDieuTra { get; set; }

        public string? KetQuaDieuTra { get; set; }

        public int TongNguoiVP { get; set; } = 0;

        public int TongToChucVP { get; set; } = 0;
        public int TongBienBan { get; set; } = 0;

        public int TongQuyetDinh { get; set; } = 0;

        public int TongTaiLieu { get; set; } = 0;

        public decimal TongTienPhat { get; set; } = 0;

        public string? IdXuLy { get; set; }

        public int TienTrinhHoSo { get; set; } = 0;
        public DateTime? UpdateDate { get; set; }

        public List<NguoiVPModifyModel> ListNguoiVP { get; set; } = new List<NguoiVPModifyModel>();
        public List<ToChucVPModifyModel> ListToChucVP { get; set; } = new List<ToChucVPModifyModel>();
        public List<PhienDichModifyModel> ListPhienDich { get; set; } = new List<PhienDichModifyModel>();
        public List<NguoiChungKienModifyModel> ListChungKien { get; set; } = new List<NguoiChungKienModifyModel>();
        public List<TangVatModifyModel> ListTangVat { get; set; } = new List<TangVatModifyModel>();
        public List<PhuongTienModifyModel> ListPhuongTien { get; set; } = new List<PhuongTienModifyModel>();
        public List<ChungChiGiayPhepModifyModel> ListGiayPhepChungChi { get; set; } = new List<ChungChiGiayPhepModifyModel>();
    }
}
