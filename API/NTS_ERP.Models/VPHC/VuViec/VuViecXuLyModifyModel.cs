using NTS_ERP.Models.VPHC.CanBo;
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
    public class VuViecXuLyModifyModel
    {
        public string? Id { get; set; }
        public string? MaHoSo { get; set; }
        public int PhanLoai { get; set; }
        [Required]
        public string IdXuLy { get; set; } = null!;
        public int TienTrinhHoSo { get; set; }
        public decimal TongTienPhat { get; set; }
        public string? DonViTiepNhanHS { get; set; }

        public string? SoBienBanHS { get; set; }

        public DateTime? NgayBanGiaoHS { get; set; }

        public string? DonViKhacXuLy { get; set; }

        public string? SoBienBanDVKhac { get; set; }

        public DateTime? NgayBanGiaoDVKhac { get; set; }

        public string? TaiLieuChuyenGiao { get; set; }

        public string? NoiDungNhacNho { get; set; }

        public string? LyDoKhongQDXP { get; set; }
        public int TongQDXuPhat { get; set; } = 0;

        public int TongQDThiHanh { get; set; } = 0;

        public int TongQDKhieuKien { get; set; } = 0;

        public int TongQDChuyen { get; set; } = 0;

        public int TongQDDangXuLy { get; set; } = 0;

        public int TongQDMienGiam { get; set; } = 0;

        public int TongQDCuongChe { get; set; } = 0;
        public DateTime? UpdateDate { get; set; }
        public List<TangVatModifyModel> ListTangVat { get; set; } = new List<TangVatModifyModel>();
        public List<PhuongTienModifyModel> ListPhuongTien { get; set; } = new List<PhuongTienModifyModel>();
        public List<ChungChiGiayPhepModifyModel> ListGiayPhepChungChi { get; set; } = new List<ChungChiGiayPhepModifyModel>();
    }
}
