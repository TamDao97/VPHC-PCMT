using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TraCuu
{
    public class TraCuuExportModel
    {
        public string NgayXuat { get; set; }

        public List<ItemExportModel> Data { get; set; }
    }
}
