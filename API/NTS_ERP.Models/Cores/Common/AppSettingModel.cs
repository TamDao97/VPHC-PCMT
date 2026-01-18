using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Common
{
    public class AppSettingModel
    {
        public string Secret { get; set; }
        public int ExpireDateAfter { get; set; }
        public int IssuedAt { get; set; }
        public int ExpiresAt { get; set; }
    }
}
