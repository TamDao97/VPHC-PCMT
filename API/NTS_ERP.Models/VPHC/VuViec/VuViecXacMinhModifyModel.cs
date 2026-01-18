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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecXacMinhModifyModel
    {
        public string? Id { get; set; }
        public string? IdDonVi { get; set; }
        public string? MaHoSo { get; set; }
        public string? NguoiUyQuyen { get; set; }
        public string? SoQDUyQuyen { get; set; }
        public DateTime? NgayUyQuyen { get; set; }
        public string? CanBoDieuTra { get; set; }
        public string? ThietHai { get; set; }
        public string? MucDoThietHai { get; set; }
        public string? GiamNhe { get; set; }
        public string? TangNang { get; set; }
        public string? YKienNguoiViPham { get; set; }
        public string? YKienNguoiLamChung { get; set; }
        public string? YKienNguoiBiThietHai { get; set; }
        public string? XacMinhKhac { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<CanBoChooseModel> ListCanBoPhanCong { get; set; } = new List<CanBoChooseModel>();
        public List<NguoiVPModifyModel> ListNguoiVP { get; set; } = new List<NguoiVPModifyModel>();
        public List<ToChucVPModifyModel> ListToChucVP { get; set; } = new List<ToChucVPModifyModel>();
        public List<PhienDichModifyModel> ListPhienDich { get; set; } = new List<PhienDichModifyModel>();
        public List<NguoiChungKienModifyModel> ListChungKien { get; set; } = new List<NguoiChungKienModifyModel>();
        public List<TangVatModifyModel> ListTangVat { get; set; } = new List<TangVatModifyModel>();
        public List<PhuongTienModifyModel> ListPhuongTien { get; set; } = new List<PhuongTienModifyModel>();
        public List<ChungChiGiayPhepModifyModel> ListGiayPhepChungChi { get; set; } = new List<ChungChiGiayPhepModifyModel>();
    }
}
