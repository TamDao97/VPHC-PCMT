using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.ChungChiGiayPhep
{
    public class ChungChiGiayPhepModifyModel
    {
        public int Index { get; set; } = 0;
        public string? IdChungChiGiayPhep { get; set; }

        public string? IdViPhamHC { get; set; }
        [Required]
        public string? Ten { get; set; }
        [Required]
        public int SoLuong { get; set; } = 0;   

        public string? GhiChu { get; set; }

        public int XuLy { get; set; } = 0;

        public string? TinhTrangDacDiem { get; set; }
    }
}