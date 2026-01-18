using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.ToChucVP
{
    public class ToChucVPResultModel : SearchBaseModel
    {
        public string Ten { get; set; } = null!;

        public string? DiaChiTruSo { get; set; }

        public string? MaSoDoanhNghiep { get; set; }

        public string? SoDKKD { get; set; }

        public DateTime? NgayCapDKKD { get; set; }

        public string? NoiCapDKKD { get; set; }

        public string? HoTenPhapNhan { get; set; }
        public int GioiTinh { get; set; }

        public string? ChucVu { get; set; }
    }
}