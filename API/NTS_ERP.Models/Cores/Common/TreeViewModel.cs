using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Common
{
    public class TreeViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Id nhóm cha
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// Có con hay không
        /// </summary>
        public bool HasChildren { get; set; }
        /// <summary>
        /// Mở rộng
        /// </summary>
        public bool Expanded { get; set; }
        /// <summary>
        /// Chọn
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
