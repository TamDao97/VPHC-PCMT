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
    public class VuViecTrongQuyetDinhModel
    {
        public string? Id { get; set; }
        public List<NguoiVPModifyModel> ListNguoiVP { get; set; } = new List<NguoiVPModifyModel>();
        public List<ToChucVPModifyModel> ListToChucVP { get; set; } = new List<ToChucVPModifyModel>();
    }
}
