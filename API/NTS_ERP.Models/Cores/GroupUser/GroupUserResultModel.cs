using System;
using System.Linq;

namespace NTS_ERP.Models.Cores.GroupUser
{
    public class GroupUserResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string Description { get; set; }
    }
}