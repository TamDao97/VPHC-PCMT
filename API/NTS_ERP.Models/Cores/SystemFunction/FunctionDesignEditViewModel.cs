using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FunctionDesignEditViewModel
    {
        public string Id { get; set; }
        public string SystemFunctionId { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool? IsUnicode { get; set; }
        public int? MaxLength { get; set; }
        public bool EditDisplay { get; set; }
        public bool EditRequired { get; set; }
        public int DivEditIndex { get; set; }
        public string DivEditWidth { get; set; }
        public int EditControlType { get; set; }
        public int EditControlHeight { get; set; }
        public bool IsLink { get; set; }
        public string LinkTable { get; set; }
        public string LinkDataJson { get; set; }
        public string LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkOrder { get; set; }
        public List<ComboboxBaseModel> LinkData { get; set; }
    }
}
