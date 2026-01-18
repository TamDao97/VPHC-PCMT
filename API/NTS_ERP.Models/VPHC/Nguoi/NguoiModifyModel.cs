using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.Nguoi
{
    public class NguoiModifyModel
    {
        public string? IdNguoi { get; set; }

        public string? IdDonVi { get; set; }
        [Required]
        public string? HoVaTen { get; set; }

        [Required]
        public DateTime? NgaySinh { get; set; }

        [Required]
        public int GioiTinh { get; set; }
        public string? TenGioiTinh { get; set; }

        [Required]
        public string? Cmnd { get; set; }

        [Required]
        public string? IdQuocTich { get; set; }
        public string? TenQuocTich { get; set; }

        public string? IdDanToc { get; set; }
        public string? TenDanToc { get; set; }

        public string? IdTinh { get; set; }

        public string? IdHuyen { get; set; }

        public string? IdXa { get; set; }

        public string? IdNgheNghiep { get; set; }
        public string? TenNgheNghiep { get; set; }

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
        public string? TenTonGiao { get; set; }

        public string? NoiCap { get; set; }
        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
