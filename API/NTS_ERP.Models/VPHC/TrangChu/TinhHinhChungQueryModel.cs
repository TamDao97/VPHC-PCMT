using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TrangChu
{
    public class TinhHinhChungQueryModel
    {
        public string Id { get; set; }
        public DateTime ThoiGianTiepNhan { get; set; }
        public string IdTinh { get; set; }
        public string HCKey { get; set; }
        public int SoVu { get; set; }
        public int SoNguoi { get; set; }
        public string TenTinh { get; set; }
    }
}
