using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.Excel
{
    public class ExcelSheetDataModel
    {
        public string SheetName { get; set; }
        public List<object> Datas { get; set; }
    }
}
