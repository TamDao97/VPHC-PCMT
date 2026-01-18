using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.TangVat
{
    public class TangVatResultModel : SearchBaseModel
    {
        public string? TenLoaiTangVat { get; set; }
        public string? TenDonViTinh { get; set; }

        public double? SoLuong { get; set; }

        public bool TiepNhan { get; set; }

        public bool BoSung { get; set; }

        public string? GhiChu { get; set; }

        public string? IdDonViTinhThuc { get; set; }
        public string? TenDonViTinhThuc { get; set; }

        public double? SoLuongThuc { get; set; }

        public int XuLy { get; set; }

        public string? Name { get; set; }

        public string? ChungLoai { get; set; }

        public string? TinhTrangDacDiem { get; set; }
    }
}