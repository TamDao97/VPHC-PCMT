using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TraCuu
{
    public class TraCuuSearchModel : SearchBaseModel
    {
        public string? IdDonVi { get; set; }

        public string? HoVaTen { get; set; }
        public DateTime? NgaySinhTo { get; set; }

        public DateTime? NgaySinhFrom { get; set; }

        public int? GioiTinh { get; set; }

        public string? QueQuan { get; set; }

        public DateTime? NgayViPhamTo { get; set; }

        public DateTime? NgayViPhamFrom { get; set; }

        public string? LinhVuc { get; set; }

        public int? TienTrinh { get; set; }
    }
}
