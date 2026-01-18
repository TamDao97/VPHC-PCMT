using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.PhuongTien
{
    public class PhuongTienModifyModel
    {
        public int Index { get; set; } = 0;
        public string IdPhuongTienVPHC { get; set; } = "";

        public string? IdViPhamHC { get; set; }
        [Required]
        public string? IdLoaiPhuongTien { get; set; }
        public string? TenLoaiPhuongTien { get; set; }

        public string? BienSo { get; set; }

        public bool TiepNhan { get; set; } = false;

        public bool BoSung { get; set; } = false;

        public string? GhiChu { get; set; }

        public int XuLy { get; set; } = 0;

        public string? NhanHieu { get; set; }

        public string? XuatXu { get; set; }

        public string? TinhTrangDacDiem { get; set; }

        public string? Name { get; set; }
    }
}