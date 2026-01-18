using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.VPHC.NguoiVP
{
    public class NguoiVPModifyModel:NguoiModifyModel
    {
        public int Index { get; set; } = 0;
        public string? IdNguoiVPHC { get; set; }

        public string? IdViPhamHC { get; set; }
        [Required]
        public int Tuoi { get; set; } = 0;

        public bool HoanCanhKhoKhan { get; set; } = false;

        public string? ChiTietHoanCanh { get; set; }

        public bool TiepNhan { get; set; } = false;

        public bool BoSung { get; set; } = false;

        public string? HanhViViPham { get; set; }
        public string? QuyDinhTai { get; set; }

        public string? GhiChu { get; set; }

        public bool TreDuoi18 { get; set; } = false;

        public int KetLuanKiemTra { get; set; } = 0;

        public string? QuanHeGH { get; set; }
        public string? HoVaTenGH { get; set; }
        public string? DiaChiGH { get; set; }
    }
}