using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.TrangChu;
using NTS_ERP.Models.VPHC.XuHuong;
using NTS_ERP.Services.Cores.Combobox;

namespace NTS_ERP.Services.VPHC.XuHuong
{
    public class XuHuongService : IXuHuongService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IComboboxService _comboboxService;
        public XuHuongService(NTS_ERPContext sqlContext, IComboboxService comboboxService
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
        public async Task<XuHuongResultModel> PhanTichXuHuong(XuHuongSearchModel searchModel)
        {
            DateTime dateFrom = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value;
            DateTime dateTo = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value;
            string idDonViCapDuoiList = string.Join(";", _comboboxService.GetDonViByIdDonVi(searchModel.IdDonVi).Result.Select(r => r.Id).ToList());

            XuHuongResultModel result = new XuHuongResultModel();
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

            //result.ListDataMap = new List<object[]>();
            //foreach (var item in dataTinh)
            //{
            //    result.ListDataMap.Add(new object[] { item.HCKey, item.SoVu });
            //}

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

            //Theo độ tuổi
            var doTuoiVPBaoCao = (from a in _sqlContext.NguoiVPHC.AsNoTracking()
                                  join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(w => !w.IsDelete && idDonViCapDuoiList.Contains(w.IdDonVi) && w.ThoiGianTiepNhan >= dateFrom && w.ThoiGianTiepNhan <= dateTo) on a.IdViPhamHC equals b.Id
                                  select new
                                  {
                                      a.IdNguoi,
                                      a.Tuoi
                                  }).ToList();

            DateTime dateFromSoSanh = dateFrom.AddYears(-1);
            DateTime dateToSoSanh = dateTo.AddYears(-1); ;
            var doTuoiVPSoSanh = (from a in _sqlContext.NguoiVPHC.AsNoTracking()
                                  join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(w => !w.IsDelete && idDonViCapDuoiList.Contains(w.IdDonVi) && w.ThoiGianTiepNhan >= dateFromSoSanh && w.ThoiGianTiepNhan <= dateToSoSanh) on a.IdViPhamHC equals b.Id
                                  select new
                                  {
                                      a.IdNguoi,
                                      a.Tuoi
                                  }).ToList();

            var listTuoi = new List<DoTuoiModel>();
            listTuoi.Add(new DoTuoiModel() { DoTuoiText = "Dưới 16 tuổi", Min = 0, Max = 15 });
            listTuoi.Add(new DoTuoiModel() { DoTuoiText = "Từ 16 đến 24 tuổi", Min = 16, Max = 24 });
            listTuoi.Add(new DoTuoiModel() { DoTuoiText = "Từ 25 đến 35 tuổi", Min = 25, Max = 35 });
            listTuoi.Add(new DoTuoiModel() { DoTuoiText = "Từ 36 đến 65 tuổi", Min = 36, Max = 65 });
            listTuoi.Add(new DoTuoiModel() { DoTuoiText = "Trên 65 tuổi", Min = 66, Max = 200 });

            int indexTuoi = 1;
            foreach (var item in listTuoi)
            {
                var dataTuoiBC = doTuoiVPBaoCao.Where(s => s.Tuoi >= item.Min && s.Tuoi <= item.Max).ToList();
                var dataTuoiSS = doTuoiVPSoSanh.Where(s => s.Tuoi >= item.Min && s.Tuoi <= item.Max).ToList();
                int soVuBC = dataTuoiBC.Count();
                int soVuSS = dataTuoiSS.Count();
                result.ListDoTuoi.Add(item.DoTuoiText);
                result.ListDoTuoiBaoCao.Add(soVuBC);
                result.ListDoTuoiSoSanh.Add(soVuSS);
                result.ListXuHuongDoTuoi.Add(new XuHuongModel()
                {
                    Index = indexTuoi,
                    TieuChi = item.DoTuoiText,
                    SoVu = soVuBC,
                    SoVuSS = soVuSS,
                    TangGiamVu = soVuBC - soVuSS
                });
                indexTuoi++;
            }

            //Theo Lĩnh vực
            var dataLinhVucBaoCao = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo).Select(s => new { s.IdLinhVucBCTH }).ToList();
            var dataLinhVucSoSanh = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromSoSanh && a.ThoiGianTiepNhan <= dateToSoSanh).Select(s => new { s.IdLinhVucBCTH, s.Id }).ToList();
            var listLinhVuc = _sqlContext.LinhVucThongKeBCTH.AsNoTracking().ToList();
            foreach (var item in listLinhVuc)
            {
                var dataTuoiBC = dataLinhVucBaoCao.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).ToList();
                var dataTuoiSS = dataLinhVucSoSanh.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).ToList();
                int soVuBC = dataTuoiBC.Count();
                int soVuSS = dataTuoiSS.Count();
                result.ListLinhVuc.Add(item.ShortName);
                result.ListLinhVucBaoCao.Add(soVuBC);
                result.ListLinhVucSoSanh.Add(soVuSS);
                result.ListXuHuongLinhVuc.Add(new XuHuongModel()
                {
                    Index = indexTuoi,
                    TieuChi = item.Name,
                    SoVu = soVuBC,
                    SoVuSS = soVuSS,
                    TangGiamVu = soVuBC - soVuSS
                });
            }

            //Dữ liệu tỉnh
            int tongVu = dataTinh.Sum(s => s.SoVu);
            //Top 5 tỉnh
            var vuViecTinh = dataTinh.OrderByDescending(o => o.SoVu).ToList();

            DateTime dateFromSS = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value.AddYears(-1);
            DateTime dateToSS = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value.AddYears(-1);
            var vuViecTinhSS = (from a in _sqlContext.Province.AsNoTracking().Where(s=>s.Border)
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
                var itemSoSanh = vuViecTinhSS.Where(s=> item.IdTinh.Equals(s.IdTinh)).FirstOrDefault();
                vuViecDVModel = new VuViecDonViModel()
                {
                    Index = index,
                    DonVi = $"Bộ Đội Biên Phòng {item.TenTinh}",
                    DonViNgan = $"BĐBP {item.TenTinh}",
                    SoVu = item.SoVu,
                    SoNguoiVP = item.SoNguoi,
                    SoVuSS = itemSoSanh?.SoVu??0,
                    SoNguoiVPSS = itemSoSanh?.SoNguoi??0,
                    TyLe = tongVu>0? Math.Round(((decimal)item.SoVu / (decimal)tongVu) * (decimal)100, 0):0,
                    TangGiamVu = item.SoVu - (itemSoSanh?.SoVu??0),
                    TangGiamNguoi = item.SoNguoi - (itemSoSanh?.SoNguoi ?? 0),
                };
                result.ListVuViecDonVi.Add(vuViecDVModel);
                index++;
            }

            return result;
        }
    }
}
