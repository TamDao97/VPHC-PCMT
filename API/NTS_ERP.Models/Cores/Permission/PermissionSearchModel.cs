using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Permission
{
    public class PermissionSearchModel
    {
        public string Code { get; set; } = "";
        public string UserName { get; set; } = "";
        /// <summary>
        /// Số bán ghi trên trang
        /// </summary>
        public int PageSize { get; set; } = 15;

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int PageNumber { get; set; } = 1;
    }
}
