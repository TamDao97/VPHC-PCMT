using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    /// <summary>
    /// Cá thuộc tính của một trường dữ liệu trong bảng sql
    /// </summary>
    public class ColumnPropertiesModel
    {
        public string ColumnName { get; set; }
        public bool Required { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool? IsUnicode { get; set; }
        public int? MaxLength { get; set; }
        public string DataType { get; set; }
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Giá trị của trường
        /// </summary>
        public object Value { get; set; }
    }
}
