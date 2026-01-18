using NTS_ERP.Models.Cores.Common;

namespace NTS_ERP.Models.Cores.GroupUser
{
    public class GroupUserSearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public bool? LockoutEnabled { get; set; }
    }
}