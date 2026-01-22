using NTS_ERP.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTra
{
    public class FileKeHoachKiemTraModel : BaseModel
    {
        public string FileName { get; set; } = null!;

        public string FileUrl { get; set; } = null!;

        public decimal FileSize { get; set; }
        public int KieuLoai { get; set; }
    }
}
