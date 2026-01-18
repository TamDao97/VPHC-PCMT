using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TaiLieu
{
    public class TaiLieuVuViecModel
    {
        public int Index { get; set; } = 0;
        public string IdTaiLieu { get; set; } = "";
        public string? Ten { get; set; }
        public string? DanhMuc { get; set; }
        public string? DinhDang { get; set; }
        public decimal? DungLuong { get; set; }
        public DateTime? NgayTaiLen { get; set; }
    }
}
