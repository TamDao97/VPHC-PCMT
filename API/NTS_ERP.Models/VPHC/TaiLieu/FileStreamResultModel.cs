using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.TaiLieu
{
    public class FileStreamResultModel
    {
        public string FileName { get; set; } = null!;
        public string Extention { get; set; } = null!;
        public MemoryStream MemoryStreamFile { get; set; } = null!;
    }
}
