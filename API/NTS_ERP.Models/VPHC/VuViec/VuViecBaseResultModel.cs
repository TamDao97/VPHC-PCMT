using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecBaseResultModel : SearchBaseResultModel<VuViecSearchResultModel>
    {
        public int TongNguoiVP { get; set; }
        public int TongToChucVP { get; set; }
        public decimal TongTienPhat { get; set; }

        public int VuTiepNhan { get; set; }
        public int NguoiTiepNhan { get; set; }
        public int VuLBB { get; set; }
        public int NguoiLBB { get; set; }
        public int VuXM { get; set; }
        public int NguoiXM { get; set; }
        public int VuXuLy { get; set; }
        public int NguoiXuLy { get; set; }
        public int VuKetThuc { get; set; }
        public int NguoiKetThuc { get; set; }
    }
}
