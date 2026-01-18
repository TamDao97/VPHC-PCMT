using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FunctionDesignCreateViewModel
    {
        public string Id { get; set; }
        public string SystemFunctionConfigId { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }
        public bool Required { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool? IsUnicode { get; set; }
        public int? MaxLength { get; set; }
        public bool CreateDisplay { get; set; }
        public bool CreateRequired { get; set; }
        public int DivCreateIndex { get; set; }
        public string DivCreateWidth { get; set; }
        public int CreateControlType { get; set; }
        public int CreateControlHeight { get; set; }
        public bool IsLink { get; set; }
        public string LinkTable { get; set; }
        public string LinkDataJson { get; set; }
        public string LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkOrder { get; set; }
        public List<ComboboxBaseModel> LinkData { get; set; }
    }
}
