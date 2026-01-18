using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TaiLieu
{
    public class DanhMucTaiLieuModle
    {
        public string Id { get; set; } = "";

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int Total { get; set; } = 0;
    }
}
