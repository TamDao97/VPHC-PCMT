using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecSearchResultModel
    {
        public int Index { get; set; } = 0;
        public string Id { get; set; } = null!;

        public string DonVi { get; set; } = null!;

        public string? MaHoSo { get; set; }

        public string NguonPhatHien { get; set; } = null!;

        public DateTime ThoiGianTiepNhan { get; set; }

        public string? DienBien { get; set; }

        public string? GhiChu { get; set; }

        public string? DiaChiDayDu { get; set; }

        public string PhanLoai { get; set; }

        public int TongNguoiVP { get; set; }

        public int TongToChucVP { get; set; }

        public decimal TongTienPhat { get; set; }

        public string XuLy { get; set; } = null!;

        public string TienTrinhHoSo { get; set; }
    }
}
