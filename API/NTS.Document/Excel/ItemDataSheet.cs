using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.Excel
{
    public class ItemDataSheet<T>
    {
        public List<T> Datas { get; set; }
        public int SheetIndex { get; set; }
        public string KeyIndexStartData { get; set; }
        public int Columns { get; set; }
        public ItemDataSheet()
        {
            Datas = new List<T>();
            SheetIndex = 0;
            KeyIndexStartData = "<Data>";
        }
    }
}
