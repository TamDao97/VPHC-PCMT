using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class SystemFunctionDesignModel
    {
        public string Id { get; set; }
        public string SystemFunctionId { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }
        public bool Required { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool? IsUnicode { get; set; }
        public int? MaxLength { get; set; }
        public bool SearchDisplay { get; set; }
        public int ColumnIndex { get; set; }
        public int ColumnWidth { get; set; }
        public int ColumnWidthMin { get; set; }
        public int ColumnWidthMax { get; set; }
        public bool CreateDisplay { get; set; }
        public bool CreateRequired { get; set; }
        public int DivCreateIndex { get; set; }
        public int CreateControlType { get; set; }
        public string DivCreateWidth { get; set; }
        public int CreateControlHeight { get; set; }
        public bool EditDisplay { get; set; }
        public bool EditRequired { get; set; }
        public int DivEditIndex { get; set; }
        public int EditControlType { get; set; }
        public string DivEditWidth { get; set; }
        public int EditControlHeight { get; set; }
        public bool FilterDisplay { get; set; }
        public int FilterIndex { get; set; }
        public int FilterControlType { get; set; }
        public bool IsLink { get; set; }
        public string LinkTable { get; set; }
        public string LinkDataJson { get; set; }
        public string LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkOrder { get; set; }
        public bool DetailDisplay { get; set; }
        public int DivDetailIndex { get; set; }
        public int DetailControlType { get; set; }
        public string DivDetailWidth { get; set; }
        public int DetailControlHeight { get; set; }
    }
}
