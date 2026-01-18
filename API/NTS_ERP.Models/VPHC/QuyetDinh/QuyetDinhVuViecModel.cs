using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.QuyetDinh
{
    public class QuyetDinhVuViecModel
    {
        public int Index { get; set; } = 0;
        public string IdQuyetDinh { get; set; } = "";
        public string? So { get; set; }
        public string? TenQuyetDinh { get; set; }
        public DateTime? NgayRaQD { get; set; }
        public string? TenDoiTuongVP { get; set; }
        public string? HanhViViPham { get; set; }
        public string? CanCu { get; set; }
        public string? PhatChinh { get; set; }
        public decimal? MucPhat { get; set; }
    }
}
