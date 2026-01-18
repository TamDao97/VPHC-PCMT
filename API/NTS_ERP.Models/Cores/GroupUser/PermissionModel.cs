using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.GroupUser
{
    public class PermissionModel
    {

        /// <summary>
        /// Id quyền
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id menu
        /// </summary>
        public string MenuSystemId { get; set; }

        /// <summary>
        /// Mã quyền
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên quyền
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// check
        /// </summary>
        public bool IsChecked { get; set; }

        public int Index { get; set; }
    }
}
