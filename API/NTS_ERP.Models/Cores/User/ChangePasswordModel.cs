using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.User
{
    public class ChangePasswordModel
    {
        public string Id { get; set; }
        public string MatKhauCu { get; set; }
        public string MatKhauMoi { get; set; }
        public string XacNhanMatKhauMoi { get; set; }
        public bool IsChange { get; set; } = false;
    }
}
