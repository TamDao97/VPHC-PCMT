using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TrangChu
{
    public  class VuViecDonViModel
    {
        public int Index { get; set; }
        public string DonVi { get; set; }
        public string DonViNgan { get; set; }
        public int SoVu { get; set; }
        public int SoNguoiVP { get; set; }
        public int SoVuSS { get; set; }
        public int SoNguoiVPSS { get; set; }
        public decimal TyLe { get; set; }
        public int TangGiamVu { get; set; }
        public int TangGiamNguoi { get; set; }
    }
}
