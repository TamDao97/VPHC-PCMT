using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.ThongKe
{
    public  class ThongKeModel
    {
        public int Index { get; set; }
        public string TieuChi { get; set; }
        public string TieuChiNgan { get; set; }
        public int SoVu { get; set; }
        public int SoNguoiVP { get; set; }
        public int SoVuSS { get; set; }
        public int SoNguoiVPSS { get; set; }
        public int TangGiamVu { get; set; }
        public int TangGiamNguoi { get; set; }
    }
}
