using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.BienBan
{
    public class BienBanVuViecModel
    {
        public int Index { get; set; } = 0;
        public string IdBienBan { get; set; } = "";
        public string? So { get; set; }
        public string? TenBienBan { get; set; }
        public DateTime? ThoiGianLap { get; set; }
        public string? TenDoiTuongVP { get; set; }
        public string? DonViLap { get; set; }
        public string? TenCanBoLap { get; set; }
        public DateTime? ThoiGianLapXong { get; set; }
    }
}
