using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class ItemExportModel
    {
        public int Index { get; set; } = 0;
        public string? MaHoSo { get; set; }

        public string DonVi { get; set; } = null!;

        public string ThoiGianTiepNhan { get; set; }

        public string? DiaChiDayDu { get; set; }

        public int TongNguoiVP { get; set; }

        public int TongToChucVP { get; set; }
        public string XuLy { get; set; }

        public decimal TongTienPhat { get; set; }

        public string TienTrinhHoSo { get; set; }
    }
}
