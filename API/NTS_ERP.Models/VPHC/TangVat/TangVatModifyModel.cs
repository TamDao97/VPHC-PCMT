using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.TangVat
{
    public class TangVatModifyModel
    {
        public int Index { get; set; } = 0;
        public string IdTangVatVPHC { get; set; } = "";

        public string? IdViPhamHC { get; set; }
        [Required]
        public string? IdLoaiTangVat { get; set; }
        public string? TenLoaiTangVat { get; set; }
        [Required]
        public string? IdDonViTinh { get; set; }
        public string? TenDonViTinh { get; set; }
        [Required]
        public double? SoLuong { get; set; }

        public bool TiepNhan { get; set; } = false;

        public bool BoSung { get; set; } = false;

        public string? GhiChu { get; set; }

        public string? IdDonViTinhThuc { get; set; }
        public string? TenDonViTinhThuc { get; set; }

        public double? SoLuongThuc { get; set; }

        public int XuLy { get; set; } = 0;  

        public string? Name { get; set; }

        public string? ChungLoai { get; set; }

        public string? TinhTrangDacDiem { get; set; }
    }
}