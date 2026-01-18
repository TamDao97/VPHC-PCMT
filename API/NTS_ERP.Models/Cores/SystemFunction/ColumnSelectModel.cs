using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class ColumnSelectModel
    {
        public ColumnPropertiesModel TypeColumn { get; set; }
        public bool IsPrimaryKey { get; set; }
        public List<ComboboxBaseModel> LinkData { get; set; }
    }
}
