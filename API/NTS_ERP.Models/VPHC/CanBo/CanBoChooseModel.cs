using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.CanBo
{
    public class CanBoChooseModel
    {
        public int Index { get; set; }
        public bool Checked { get; set; } = false;
        public string? Id { get; set; }
        public string? Ten { get; set; }
        public string? GioiTinh { get; set; } 
        public string? CapBac { get; set; }
        public string? ChucVu { get; set; }
        public string? DonVi { get; set; }
    }
}
