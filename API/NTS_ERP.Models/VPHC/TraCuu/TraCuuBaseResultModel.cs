using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TraCuu
{
    public class TraCuuBaseResultModel : SearchBaseResultModel<TraCuuSearchResultModel>
    {
        public int TongNguoiVP { get; set; }
        public int TongLanVP { get; set; }
        public decimal TongTienPhat { get; set; }

        public int NguoiMT { get; set; }
        public int NguoiBL { get; set; }
        public int NguoiMBN { get; set; }
        public int NguoiVKVLN { get; set; }
        public int NguoiXNC { get; set; }
        public int NguoiKhac { get; set; }
    }
}
