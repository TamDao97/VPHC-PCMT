using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecSearchModel : SearchBaseModel
    {
        public string? IdTinhPhatHien { get; set; }
        public string? IdDonVi { get; set; }

        public string? MaHoSo { get; set; }

        public string? IdNguonPhatHien { get; set; }

        public DateTime? ThoiGianTiepNhanTo { get; set; }

        public DateTime? ThoiGianTiepNhanFrom { get; set; }

        public string? DiaDiemPhatHien { get; set; }
        public string? DienBien { get; set; }
        public string? LinhVuc { get; set; }

        public int? PhanLoai { get; set; }

        public string? IdXuLy { get; set; }

        public int? TienTrinhHoSo { get; set; }
    }
}
