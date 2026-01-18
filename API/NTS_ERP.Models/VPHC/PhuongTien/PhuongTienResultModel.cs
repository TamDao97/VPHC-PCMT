using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.PhuongTien
{
    public class PhuongTienResultModel : SearchBaseModel
    {
        public string? TenLoaiPhuongTien { get; set; }

        public string? BienSo { get; set; }

        public bool TiepNhan { get; set; }

        public bool BoSung { get; set; }

        public string? GhiChu { get; set; }

        public int XuLy { get; set; }

        public string? NhanHieu { get; set; }

        public string? XuatXu { get; set; }

        public string? TinhTrangDacDiem { get; set; }

        public string? Name { get; set; }
    }
}