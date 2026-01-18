using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TrangChu
{
    public class TieuDiemModel
    {
        public int SoVu { get; set; }
        public int SoNguoi { get; set; }
        public List<TieuDiemItemModel> ListTieuDiemDonVi { get; set; } = new List<TieuDiemItemModel>();
    }
}
