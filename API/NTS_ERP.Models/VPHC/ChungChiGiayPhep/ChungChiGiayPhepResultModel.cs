using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.ChungChiGiayPhep
{
    public class ChungChiGiayPhepResultModel : SearchBaseModel
    {
        public string? Ten { get; set; }

        public int SoLuong { get; set; }

        public string? GhiChu { get; set; }

        public int XuLy { get; set; }

        public string? TinhTrangDacDiem { get; set; }
    }
}