using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Auth
{
    public class CurrentUserModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; }
        public string DonViId { get; set; }
    }
}
