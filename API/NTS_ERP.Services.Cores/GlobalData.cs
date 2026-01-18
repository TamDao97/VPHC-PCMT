using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.Cores
{
    public class GlobalData
    {
        /// <summary>
        /// Danh sách cấu hình chức năng tự động
        /// </summary>
        public static List<Nts_SystemFunctionConfig> ListFunctionConfig = new List<Nts_SystemFunctionConfig>();

        /// <summary>
        /// Danh sách cache all đơn vị
        /// </summary>
        public static List<DonVi> ListAllDonVi = new List<DonVi>();
    }
}
