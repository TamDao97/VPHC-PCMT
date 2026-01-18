using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FunctionDesignDetailViewModel
    {
        public string Id { get; set; }
        public string SystemFunctionId { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public bool DetailDisplay { get; set; }
        public int DivDetailIndex { get; set; }
        public int DetailControlType { get; set; }
        public string DivDetailWidth { get; set; }
        public int DetailControlHeight { get; set; }
        public bool IsLink { get; set; }
        public string LinkTable { get; set; }
        public string LinkDataJson { get; set; }
        public string LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkOrder { get; set; }
        public List<ComboboxBaseModel> LinkData { get; set; }
    }
}
