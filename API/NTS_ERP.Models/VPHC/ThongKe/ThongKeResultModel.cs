using Newtonsoft.Json;
using NTS_ERP.Models.VPHC.TrangChu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.VPHC.ThongKe
{
    public class ThongKeResultModel
    {
        public int NamBaoCao { get; set; }
        public int NamSoSanh { get; set; }

        public List<string> ListNam { get; set; } = new List<string>();
        public List<int> ListNamSoVu { get; set; } = new List<int>();
        public List<int> ListNamSoNguoi { get; set; } = new List<int>();
        public List<decimal> ListNamTongTien { get; set; } = new List<decimal>();

        public List<TopTangVatModel> ListTopTangVat { get; set; } = new List<TopTangVatModel>();

        public List<XuLyModel> ListXuLy { get; set; } = new List<XuLyModel>();
        public List<string> ListLinhVucDonut { get; set; } = new List<string>();
        public List<int> ListLinhVucDonutSoVu { get; set; } = new List<int>();

        public List<VuViecDonViModel> ListVuViecDonVi { get; set; } = new List<VuViecDonViModel>();
    }
}
