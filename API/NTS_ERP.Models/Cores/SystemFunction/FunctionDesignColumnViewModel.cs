using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FunctionDesignColumnViewModel
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
        public int ColumnIndex { get; set; }
        public bool IsLink { get; set; }
        public string LinkTable { get; set; }
        public string LinkDataJson { get; set; }
        public string LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkOrder { get; set; }
    }
}
