using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.InitData
{
    public class SystemBackupModel
    {
        public int Index { get; set; }
        public string SoftwareName { get; set; }
        public DateTime BackupDate { get; set; }
        public string Version { get; set; }
        public string TenTinh { get; set; }
        public string TenDon { get; set; }
    }
}
