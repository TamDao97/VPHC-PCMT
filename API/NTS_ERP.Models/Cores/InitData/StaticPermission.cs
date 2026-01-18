using System.Collections.Generic;

namespace NTS_ERP.Models.Cores.InitData
{
    public class IntDataModel
    {
        public List<StaticFuncfion> Funcfions { get; set; }

        public List<StaticFuncfion> AutoFuncfions { get; set; }

        public List<StaticMenu> Menus { get; set; }

        public List<StaticMenuPermission> MenuPermissions { get; set; }
    }

    public class StaticFuncfion
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }

    public class StaticMenu
    {
        public string Id { get; set; }
        public string TitleDefault { get; set; }
        public string TitleKeyTranslate { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public bool IsDisable { get; set; }
        public int Index { get; set; }
        public bool IsDefaultMenu { get; set; }
        public bool FunctionAuto { get; set; }
    }

    public class StaticMenuPermission
    {
        public string Id { get; set; }
        public string FunctionId { get; set; }
        public string MenuSystemId { get; set; }
    }
}
