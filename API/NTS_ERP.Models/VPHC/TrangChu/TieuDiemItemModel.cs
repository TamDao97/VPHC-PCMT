using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TrangChu
{
    public class TieuDiemItemModel
    {
        public string TenDonVi {  get; set; }
        public int SoVu { get; set; }
        public int SoNguoi { get; set; }
        public string TangVat { get; set; }
        public int TangGiamVu { get; set; }
        public int GiaTriTangGiamVu { get; set; }
        public int TangGiamNguoi { get; set; }
        public int GiaTriTangGiamNguoi { get; set; }
    }
}
