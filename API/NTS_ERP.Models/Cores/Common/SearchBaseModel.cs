using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.Common
{
    public class SearchBaseModel
    {
        public SearchBaseModel()
        {
            PageSize = 10;
            PageNumber = 1;
        }
       public bool NoPagination { get; set; } = false;
        /// <summary>
        /// Số bán ghi trên trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int PageNumber { get; set; }

        public string? OrderBy { get; set; }

        public string? OrderType { get; set; }

        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}
