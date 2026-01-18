using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TraCuu
{
    public class TraCuuQueryModel
    {
        public string Id { get; set; } = null!;

        public string IdDonVi { get; set; } = null!;

        public string MaHoSo { get; set; }

        public DateTime NgayViPham { get; set; }
        public string IdNguoi { get; set; }
        public string HoVaTen { get; set; }
        public int GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? QueQuan { get; set; }
        public string? LinhVuc { get; set; }
        public int TienTrinhHoSo { get; set; }
        public string XuLy { get; set; }
        public string SoLanViPham { get; set; }
        public decimal TongTienPhat { get; set; }
    }
}
