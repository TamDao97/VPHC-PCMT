using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TraCuu
{
    public class ItemExportModel
    {
        public int Index { get; set; } = 0;
        public string HoVaTen { get; set; }
        public string GioiTinh { get; set; }
        public string NgaySinh { get; set; }
        public string? QueQuan { get; set; }
        public string DonVi { get; set; } = null!;
        public string NgayViPham { get; set; }
        public string? LinhVuc { get; set; }
        public string TienTrinhHoSo { get; set; }
        public int SoLanViPham { get; set; }
        public string XuLy { get; set; }
        public string TongTienPhat { get; set; }
    }
}
