using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.ToChucVP
{
    public class ToChucVPModifyModel
    {
        public int Index { get; set; } = 0;
        public string? IdToChucVP { get; set; }

        public string? IdViPhamHC { get; set; }

        [Required]
        public string? Ten { get; set; }
        [Required]
        public string? DiaChiTruSo { get; set; }

        public string? MaSoDoanhNghiep { get; set; }

        public string? SoDKKD { get; set; }

        public DateTime? NgayCapDKKD { get; set; }

        public string? NoiCapDKKD { get; set; }
        [Required]
        public string? HoTenPhapNhan { get; set; }
        [Required]
        public int? GioiTinh { get; set; }
        public string? TenGioiTinh { get; set; }
        [Required]

        public string? ChucVu { get; set; }

        public bool TiepNhan { get; set; } = false;

        public bool BoSung { get; set; } = false;

        public string? HanhViViPham { get; set; }
        public string? QuyDinhTai { get; set; }

        public string? GhiChu { get; set; }

        public int KetLuanKiemTra { get; set; } = 0;

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}