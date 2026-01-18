using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.TrangChu;
using NTS_ERP.Models.VPHC.ThongKe;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.VPHC.ThongKe;

namespace NTS_ERP.Services.VPHC.ThongKe
{
    public class ThongKeService : IThongKeService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IComboboxService _comboboxService;
        public ThongKeService(NTS_ERPContext sqlContext, IComboboxService comboboxService
            )
        {
            this._sqlContext = sqlContext;
            this._comboboxService = comboboxService;
        }

        /// <summary>
        /// Tìm kiếm vụ việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<ThongKeResultModel> ThongKe(ThongKeSearchModel searchModel)
        {
            DateTime dateFrom = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value;
            DateTime dateTo = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value;
            string idDonViCapDuoiList = string.Join(";", _comboboxService.GetDonViByIdDonVi(searchModel.IdDonVi).Result.Select(r => r.Id).ToList());

            ThongKeResultModel result = new ThongKeResultModel();
            result.NamBaoCao = searchModel.NamBaoCao;
            result.NamSoSanh = searchModel.NamSoSanh;

            var dataTinh = (from a in _sqlContext.Province.AsNoTracking().Where(s => s.Border)
                            join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo) on a.Id equals b.IdTinhPhatHien into ab
                            select new TinhHinhChungQueryModel()
                            {
                                TenTinh = a.Name,
                                IdTinh = a.Id,
                                HCKey = a.HCKey,
                                SoVu = ab.Count(),
                                SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                            }).ToList();

            ///Báo cáo theo 5 năm của năm báo cáo
            DateTime dateToNam = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value;
            DateTime dateFromNam = $"01/01/{searchModel.NamBaoCao - 5}".ToStartDate().Value;
            //Theo năm
            var dataNam = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromNam && a.ThoiGianTiepNhan <= dateToNam).ToList();
            var dataNamResult = dataNam.Select(s => new { ThoiGianTiepNhan = s.ThoiGianTiepNhan.ToString("yyyy"), s.Id, s.TongTienPhat, SoNguoi = s.TongNguoiVP + s.TongToChucVP }).GroupBy(s => s.ThoiGianTiepNhan).Select(s => new { Nam = s.Key, SoVu = s.Count(), TongTienPhat = s.Sum(sm => sm.TongTienPhat), SoNguoi = s.Sum(sm => sm.SoNguoi) }).ToList();

            for (int nam = searchModel.NamBaoCao - 5; nam <= searchModel.NamBaoCao; nam++)
            {
                result.ListNam.Add($"Năm {nam}");
                var data = dataNamResult.Where(s => s.Nam.Equals(nam.ToString())).FirstOrDefault();
                result.ListNamSoVu.Add(data?.SoVu ?? 0);
                result.ListNamTongTien.Add(data?.TongTienPhat ?? 0);
                result.ListNamSoNguoi.Add(data?.SoNguoi ?? 0);
            }

            //Top tang vật
            var tangVat = (from a in _sqlContext.TangVatVPHC.AsNoTracking()
                           join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo) on a.IdViPhamHC equals b.Id
                           join c in _sqlContext.DonViTinh.AsNoTracking() on a.IdDonViTinh equals c.IdDonViTinh
                           join d in _sqlContext.LoaiTangVat.AsNoTracking() on a.IdLoaiTangVat equals d.Id
                           select new { a.IdLoaiTangVat, TenTangVat = d.Name, a.IdTangVatVPHC, a.SoLuong, a.IdDonViTinh, DonViTinh = c.Ten }).ToList();
            result.ListTopTangVat = tangVat.Where(w => w.SoLuong > 0).GroupBy(s => s.IdLoaiTangVat).Select(s => new TopTangVatModel { TenTangVat = s.First().TenTangVat, SoLuongDonVi = $"{s.Sum(su => su.SoLuong)?.DoubleFomatString()} {s.First().DonViTinh}" }).ToList();

            var dataTong = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo).ToList();
            //var dataXuLy = dataTong.Where(s => !string.IsNullOrEmpty(s.IdXuLy));
            DateTime dateFromSoSanh = dateFrom.AddYears(-1);
            DateTime dateToSoSanh = dateTo.AddYears(-1); ;
            var dataTongSoSanh = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromSoSanh && a.ThoiGianTiepNhan <= dateToSoSanh && a.TienTrinhHoSo == 3).ToList();
            var xuLy = _sqlContext.XuLyVPHC.AsNoTracking().OrderBy(o => o.Order).ToList();
            xuLy.Insert(0, new XuLyVPHC() { Id = "0", Name = "Đang xử lý" });
            int tongXuLy = dataTong.Count();
            XuLyModel xuLyModel;
            string[] colors = ["success","danger", "warning", "info", "primary", "dark"];
            int indexC = 0;
            foreach (var item in xuLy)
            {
                var dataXLBC = dataTong.Where(s => s.IdXuLy.Equals(item.Id)).ToList();
                var dataXLSS = dataTongSoSanh.Where(s => s.IdXuLy.Equals(item.Id)).ToList();
                int tong = dataXLBC.Count();
                int tongSS = dataXLSS.Count();
                xuLyModel = new XuLyModel()
                {
                    TenXuLy = item.Name,
                    SoVu = tong,
                    SoNguoi = dataXLBC.Sum(s => s.TongNguoiVP) + dataXLBC.Sum(s => s.TongToChucVP),
                    TyLe = tongXuLy>0? Math.Round(((decimal)tong / (decimal)tongXuLy) * (decimal)100, 0):0,
                    Color = colors[indexC]
                };
                decimal tangGiam = tong - tongSS;
                if (tangGiam > 0)
                {
                    xuLyModel.TangGiam = 1;
                    xuLyModel.PhanTramTG = Math.Round(tongSS > 0 ? (tangGiam / (decimal)tongSS) * (decimal)100 : 100, 0);
                }
                else if (tangGiam < 0)
                {
                    xuLyModel.TangGiam = -1;
                    xuLyModel.PhanTramTG = Math.Round((tangGiam * -1 / (decimal)tongSS) * (decimal)100, 0);
                }
                else
                {
                    xuLyModel.TangGiam = 0;
                    xuLyModel.PhanTramTG = 0;
                }
                result.ListXuLy.Add(xuLyModel);
                indexC++;
            }
            //Lĩnh vực
            var listLinhVuc = _sqlContext.LinhVucThongKeBCTH.AsNoTracking().ToList();
            foreach (var item in listLinhVuc)
            {
                var dataLVBC = dataTong.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).ToList();
                result.ListLinhVucDonut.Add(item.ShortName);
                result.ListLinhVucDonutSoVu.Add(dataLVBC.Count);
            }

            //Dữ liệu tỉnh
            int tongVu = dataTinh.Sum(s => s.SoVu);
            //Top 5 tỉnh
            var vuViecTinh = dataTinh.OrderByDescending(o => o.SoVu).ToList();

            DateTime dateFromSS = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value.AddYears(-1);
            DateTime dateToSS = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value.AddYears(-1);
            var vuViecTinhSS = (from a in _sqlContext.Province.AsNoTracking().Where(s => s.Border)
                                join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromSS && a.ThoiGianTiepNhan <= dateToSS) on a.Id equals b.IdTinhPhatHien into ab
                                select new TinhHinhChungQueryModel()
                                {
                                    TenTinh = a.Name,
                                    IdTinh = a.Id,
                                    HCKey = a.HCKey,
                                    SoVu = ab.Count(),
                                    SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                }).ToList();

            VuViecDonViModel vuViecDVModel;
            int index = 1;
            foreach (var item in vuViecTinh)
            {
                var itemSoSanh = vuViecTinhSS.Where(s => item.IdTinh.Equals(s.IdTinh)).FirstOrDefault();
                vuViecDVModel = new VuViecDonViModel()
                {
                    Index = index,
                    DonVi = $"Bộ Đội Biên Phòng {item.TenTinh}",
                    DonViNgan = $"BĐBP {item.TenTinh}",
                    SoVu = item.SoVu,
                    SoNguoiVP = item.SoNguoi,
                    SoVuSS = itemSoSanh?.SoVu ?? 0,
                    SoNguoiVPSS = itemSoSanh?.SoNguoi ?? 0,
                    TyLe = tongVu>0? Math.Round(((decimal)item.SoVu / (decimal)tongVu) * (decimal)100, 0):0,
                    TangGiamVu = item.SoVu - (itemSoSanh?.SoVu ?? 0),
                    TangGiamNguoi = item.SoNguoi - (itemSoSanh?.SoNguoi ?? 0),
                };
                result.ListVuViecDonVi.Add(vuViecDVModel);
                index++;
            }

            return result;
        }
    }
}
