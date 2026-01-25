using NTS_ERP.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Entities
{
    public class KeHoachKiemTraPhanGiao : BaseEntity
    {
        public string IdKeHoachKiemTra { get; set; }
        public string IdDonVi { get; set; }
        public DateTime? NgayNhanPhanGiao { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? SoDoiTuong { get; set; }
        public int? SoVu { get; set; }
        public decimal? TongTienXuPhat { get; set; }
    }
}
