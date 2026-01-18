using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.QuyetDinh
{
    public class QuyetDinhVuViecResultModel: SearchBaseResultModel<QuyetDinhVuViecModel>
    {
        public decimal TongTienPhat { get; set; }
    }
}
