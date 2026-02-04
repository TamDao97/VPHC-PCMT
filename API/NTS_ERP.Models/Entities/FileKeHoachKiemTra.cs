using NTS_ERP.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Entities
{
    public class FileKeHoachKiemTra : BaseEntity
    {
        public string IdKeHoachKiemTra { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public decimal? FileSize { get; set; }

        /// <summary>
        /// 1:Soạn thảo
        /// 2:Trình ký
        /// 3:Ban hành
        /// 4:Bổ sung / điều chỉnh
        /// </summary>
        public int? LoaiFile { get; set; }

        /// <summary>
        /// Version ban hành
        /// </summary>
        public int? LanBanHanh { get; set; }

        /// <summary>
        /// 1:Hiệu lực, 9:Hết hiệu lực
        /// </summary>
        public int? TrangThai { get; set; }
    }

}
