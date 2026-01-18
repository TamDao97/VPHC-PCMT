using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.PhienDich
{
    public class PhienDichModifyModel
    {
        public int Index { get; set; } = 0;
        public string? IdPhienDichVienVPHC { get; set; }

        public string? IdViPhamHC { get; set; }
        [Required]
        public string? HoVaTen { get; set; }
        [Required]
        public int GioiTinh { get; set; } = 0;
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