using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Controls
{
    public class FilterSearchModel : SearchBaseModel
    {
        public string IdTinh { get; set; }
        public string IdHuyen { get; set; }
        public string IdXa { get; set; }
        public string IdTuyenBG { get; set; }
        public string IdDon { get; set; }
        public string Name { get; set; }
        public string BiDanh { get; set; }
        public string BiSo { get; set; }
        public string IdLoai { get; set; }
        public bool IsExport { get; set; }
        public string IdTinhChat { get; set; }
        public string IdDonVi { get; set; }
        public string IdKetThuc { get; set; }
        public string IdDanToc { get; set; }
        public string IdNgheNghiep { get; set; }
        public string IdTinhTrang { get; set; }
        public string IdPhuongPhapXayDung { get; set; }
        public string IdSoTruong { get; set; }
        public string IdMoiQuanHe { get; set; }
        public string IdChatLuong { get; set; }
        public string IdCapBac { get; set; }
        public string IdChucVu { get; set; }
        public string IdTienAn { get; set; }
        public string IdCoSoCaiNghien { get; set; }
        public int? GioiTinh { get; set; }
        public int? TinhTrang { get; set; }
        public string IdMucDichMBN { get; set; }
        public string IdNoiChuyenTuyen { get; set; }
        public string IdPhanLoaiQLNV { get; set; }
        public string IdKetThucQLNV { get; set; }
        public int? Tuoi { get; set; }
        public int? TuoiType { get; set; }

        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime? DateEndFrom { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime? DateEndTo { get; set; }
    }
}
