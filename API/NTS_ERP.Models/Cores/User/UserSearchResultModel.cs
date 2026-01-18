using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.User
{
    public class UserSearchResultModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Description { get; set; }
    }
}
