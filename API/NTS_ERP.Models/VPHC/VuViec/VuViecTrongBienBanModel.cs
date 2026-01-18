using NTS_ERP.Models.VPHC.CanBo;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.PhienDich;
using NTS_ERP.Models.VPHC.ToChucVP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecTrongBienBanModel
    {
        public string? Id { get; set; }
        public string? DiaChiDayDu { get; set; }
        public List<CanBoChooseModel> ListCanBo { get; set; } = new List<CanBoChooseModel>();
        public List<NguoiVPModifyModel> ListNguoiVP { get; set; } = new List<NguoiVPModifyModel>();
        public List<ToChucVPModifyModel> ListToChucVP { get; set; } = new List<ToChucVPModifyModel>();
        public List<PhienDichModifyModel> ListPhienDich { get; set; } = new List<PhienDichModifyModel>();
        public List<NguoiChungKienModifyModel> ListChungKien { get; set; } = new List<NguoiChungKienModifyModel>();
    }
}
