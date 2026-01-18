using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.DanhMuc
{
    public class DanhMucCreateModel
    {
        public string IdDanhMuc { get; set; }
        public string IdHeLoai { get; set; }
        public string Ten { get; set; }
        public int Order { get; set; }
    }
}
