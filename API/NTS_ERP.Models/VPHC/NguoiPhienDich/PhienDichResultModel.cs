using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.PhienDich
{
    public class PhienDichResultModel : SearchBaseModel
    {
        public string HoVaTen { get; set; }

        public int GioiTinh { get; set; }
        public string? TenGioiTinh { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string? Cmnd { get; set; }

        public string? DiaChi { get; set; }

        public string? GhiChu { get; set; }

        public string? SoDienThoai { get; set; }

        public string? TrinhDoChuyenMon { get; set; }

        public DateTime? NgayCap { get; set; }

        public string? NoiCap { get; set; }

        public string? NgheNghiep { get; set; }
    }
}