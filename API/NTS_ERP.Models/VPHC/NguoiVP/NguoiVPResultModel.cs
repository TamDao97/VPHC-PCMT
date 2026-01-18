using NTS_ERP.Models.Cores.Common;
using System;
using System.Linq;

namespace NTS_ERP.Models.VPHC.NguoiVP
{
    public class NguoiVPResultModel
    {
        public string HoVaTen { get; set; } = null!;

        public DateTime? NgaySinh { get; set; }

        public int GioiTinh { get; set; }

        public string? Cmnd { get; set; }

        public string? IdQuocTich { get; set; }

        public string? IdDanToc { get; set; }

        public string? IdTinh { get; set; }

        public string? IdHuyen { get; set; }

        public string? IdXa { get; set; }

        public string? IdNgheNghiep { get; set; }

        public string? IdTinhHienNay { get; set; }

        public string? IdHuyenHienNay { get; set; }

        public string? IdXaHienNay { get; set; }

        public string? DiaChi { get; set; }

        public string? TrinhDoVanHoa { get; set; }

        public string? SoDienThoai { get; set; }

        public string? HinhAnh { get; set; }

        public string? DiaChiDayDu { get; set; }

        public string? DiaChiHienNayDayDu { get; set; }

        public DateTime? NgayCap { get; set; }

        public string? IdTonGiao { get; set; }

        public string? NoiCap { get; set; }
    }
}