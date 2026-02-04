using NTS_ERP.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTra
{
    public class FileKeHoachKiemTraModel : BaseModel
    {
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public decimal? FileSize { get; set; }

        /// <summary>
        /// 1:Soạn thảo
        /// 2:Trình ký
        /// 3:Ban hành
        /// 4:Bổ sung / điều chỉnh
        /// </summary>
        public int LoaiFile { get; set; }

        public int? LanBanHanh { get; set; }     // version
        public int? TrangThai { get; set; }      // 1:Hiệu lực, 9:Hết hiệu lực
    }

}
