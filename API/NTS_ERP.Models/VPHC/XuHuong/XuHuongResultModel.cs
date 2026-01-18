using Newtonsoft.Json;
using NTS_ERP.Models.VPHC.TrangChu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.XuHuong
{
    public class XuHuongResultModel
    {
        public int NamBaoCao { get; set; }
        public int NamSoSanh { get; set; }
        public List<object[]> ListDataMap { get; set; }

        public List<string> ListNam { get; set; } = new List<string>();
        public List<int> ListNamSoVu { get; set; } = new List<int>();
        public List<int> ListNamSoNguoi { get; set; } = new List<int>();
        public List<decimal> ListNamTongTien { get; set; } = new List<decimal>();

        public List<string> ListDoTuoi { get; set; } = new List<string>();
        public List<int> ListDoTuoiBaoCao { get; set; } = new List<int>();
        public List<int> ListDoTuoiSoSanh { get; set; } = new List<int>();
        public List<XuHuongModel> ListXuHuongDoTuoi { get; set; } = new List<XuHuongModel>();

        public List<string> ListLinhVuc { get; set; } = new List<string>();
        public List<int> ListLinhVucBaoCao { get; set; } = new List<int>();
        public List<int> ListLinhVucSoSanh { get; set; } = new List<int>();
        public List<XuHuongModel> ListXuHuongLinhVuc { get; set; } = new List<XuHuongModel>();

        public List<VuViecDonViModel> ListVuViecDonVi { get; set; } = new List<VuViecDonViModel>();
        //public List<TopTangVatModel> ListTopTangVat { get; set; } = new List<TopTangVatModel>();

        public int TongVu { get; set; }
        public int TongNguoiVP { get; set; }
        public int TongToChucVP { get; set; }
        public decimal TongTienPhat { get; set; }

        public int VuTiepNhan { get; set; }
        public int NguoiTiepNhan { get; set; }
        public int VuLBB { get; set; }
        public int NguoiLBB { get; set; }
        public int VuXM { get; set; }
        public int NguoiXM { get; set; }
        public int VuXuLy { get; set; }
        public int NguoiXuLy { get; set; }

        //public List<XuLyModel> ListXuLy { get; set; } = new List<XuLyModel>();

        public List<string> ListLinhVucDonut { get; set; } = new List<string>();
        public List<int> ListLinhVucDonutSoVu { get; set; } = new List<int>();

        //public TieuDiemModel TieuDiemHour { get; set; } =new TieuDiemModel();
        //public TieuDiemModel TieuDiemDay{ get; set; } = new TieuDiemModel();
        //public TieuDiemModel TieuDiemWeek { get; set; } = new TieuDiemModel();
    }
}
