using NTS_ERP.Models.Cores.GroupUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Menu
{
    public class MenuViewModel
    {
        public string Id { get; set; }
        public string TitleKeyTranslate { get; set; }
        public string TitleDefault { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public bool IsDisable { get; set; }
        public bool IsDefaultMenu { get; set; }
        public int Index { get; set; }
        public int Match { get; set; } = 0;
        public List<MenuViewModel> Children { get; set; }
        public List<PermissionModel> ListPermission { get; set; }
        public bool FunctionAuto { get; set; }
        public string SystemFunctionConfigId { get; set; }
        public MenuViewModel()
        {
            ListPermission = new List<PermissionModel>();
        }
    }
}
