using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.UserHistory
{
    public class UserHistorySearchResultModel
    {
        public int Index { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string ClientIP { get; set; }
        public string OS { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public int Type { get; set; }
    }
}
