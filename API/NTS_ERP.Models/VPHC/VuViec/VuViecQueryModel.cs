using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.VuViec
{
    public class VuViecQueryModel
    {
        public string Id { get; set; } = null!;

        public string IdDonVi { get; set; } = null!;

        public string MaHoSo { get; set; }

        public string IdNguonPhatHien { get; set; } = null!;

        public DateTime ThoiGianTiepNhan { get; set; }

        public string DienBien { get; set; }

        public string GhiChu { get; set; }

        public int LoaiDiaDiem { get; set; }

        public string IdTinhPhatHien { get; set; } = null!;

        public string? IdHuyenPhatHien { get; set; }

        public string? IdXaPhatHien { get; set; }

        public string? DiaChiChiTiet { get; set; }

        public string? DiaChiDayDu { get; set; }

        public int PhanLoai { get; set; }
        public string IdLinhVucTKBC { get; set; }

        public string? CanBoDieuTra { get; set; }

        public string? KetQuaDieuTra { get; set; }

        public int TongNguoiVP { get; set; }

        public int TongToChucVP { get; set; }

        public decimal TongTienPhat { get; set; }

        public string IdXuLy { get; set; } = null!;

        public int TienTrinhHoSo { get; set; }
    }
}
