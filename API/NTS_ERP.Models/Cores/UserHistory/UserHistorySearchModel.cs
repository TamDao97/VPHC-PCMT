using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.UserHistory
{
    public class UserHistorySearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int? Type { get; set; }
        public bool IsExport { get; set; }
    }
}
