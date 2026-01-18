using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Document.Excel;
using NTS_ERP.Models.Cores.Combobox;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.TraCuu;
using NTS_ERP.Models.VPHC.TrangChu;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.VPHC.ChungChiGiayPhep;
using NTS_ERP.Services.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhuongTien;
using NTS_ERP.Services.VPHC.TangVat;
using NTS_ERP.Services.VPHC.ToChucVP;
using TrafficControl.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NTS_ERP.Services.VPHC.TraCuu
{
    public class TrangChuService : ITrangChuService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IComboboxService _comboboxService;
        public TrangChuService(NTS_ERPContext sqlContext, IComboboxService comboboxService
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
        public async Task<TrangChuResultModel> TinhHinhChung(TrangChuSearchModel searchModel)
        {
            DateTime dateFrom = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value;
            DateTime dateTo = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value;

            TrangChuResultModel result = new TrangChuResultModel();
            result.NamBaoCao = searchModel.NamBaoCao;
            result.NamSoSanh = searchModel.NamSoSanh;

            var dataTinh = (from a in _sqlContext.Province.AsNoTracking()
                            join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo) on a.Id equals b.IdTinhPhatHien into ab
                            select new TinhHinhChungQueryModel()
                            {
                                TenTinh = a.Name,
                                IdTinh = a.Id,
                                HCKey = a.HCKey,
                                SoVu = ab.Count(),
                                SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                            }).ToList();

            result.ListDataMap = new List<object[]>();
            foreach (var item in dataTinh)
            {
                result.ListDataMap.Add(new object[] { item.HCKey, item.SoVu });
            }

            string idDonViCapDuoiList = string.Join(";", _comboboxService.GetDonViByIdDonVi(searchModel.IdDonVi).Result.Select(r => r.Id).ToList());

            DateTime dateToThang, dateFromThang;
            List<string> listThangNam = new List<string>();
            //Năm bằng năm hiện tại
            if (searchModel.NamBaoCao == DateTime.Now.Year)
            {
                dateToThang = $"01/{DateTime.Now.ToString("MM/yyyy")}".ToEndDate().Value.AddMonths(1).AddDays(-1);
                dateFromThang = $"01/{dateFrom.AddMonths(-10).ToString("MM/yyyy")}".ToStartDate().Value;
                for (int i = 0; i < 12; i++)
                {
                    listThangNam.Add(dateFromThang.AddMonths(i).ToString("MM/yyyy"));
                }
            }
            else
            {
                dateFromThang = $"01/01/{searchModel.NamBaoCao}".ToStartDate().Value;
                dateToThang = $"31/12/{searchModel.NamBaoCao}".ToEndDate().Value;
                for (int i = 0; i < 12; i++)
                {
                    listThangNam.Add(dateFromThang.AddMonths(i).ToString("MM/yyyy"));
                }
            }

            //Theo tháng
            var dataThang = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromThang && a.ThoiGianTiepNhan <= dateToThang).ToList();
            var dataResult = dataThang.Select(s => new { ThoiGianTiepNhan = s.ThoiGianTiepNhan.ToString("MM/yyyy"), s.Id, s.TongTienPhat }).GroupBy(s => s.ThoiGianTiepNhan).Select(s => new { Thang = s.Key, SoVu = s.Count(), TongTienPhat = s.Sum(sm => sm.TongTienPhat) }).ToList();

            foreach (var thangNam in listThangNam)
            {
                result.ListThang.Add($"Tháng {thangNam}".Replace('/', ' '));
                var data = dataResult.Where(s => s.Thang.Equals(thangNam)).FirstOrDefault();
                result.ListThangSoVu.Add(data?.SoVu ?? 0);
                result.ListThangTongTien.Add(data?.TongTienPhat ?? 0);
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

            foreach (var item in listTuoi)
            {
                result.ListDoTuoi.Add(item.DoTuoiText);
                result.ListDoTuoiBaoCao.Add(doTuoiVPBaoCao.Where(s => s.Tuoi >= item.Min && s.Tuoi <= item.Max).Count());
                result.ListDoTuoiSoSanh.Add(doTuoiVPSoSanh.Where(s => s.Tuoi >= item.Min && s.Tuoi <= item.Max).Count());
            }

            //Theo Lĩnh vực
            var dataLinhVucBaoCao = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo).Select(s => new { s.IdLinhVucBCTH }).ToList();
            var dataLinhVucSoSanh = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromSoSanh && a.ThoiGianTiepNhan <= dateToSoSanh).Select(s => new { s.IdLinhVucBCTH, s.Id }).ToList();
            var listLinhVuc = _sqlContext.LinhVucThongKeBCTH.AsNoTracking().ToList();
            foreach (var item in listLinhVuc)
            {
                result.ListLinhVuc.Add(item.ShortName);
                result.ListLinhVucBaoCao.Add(dataLinhVucBaoCao.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).Count());
                result.ListLinhVucSoSanh.Add(dataLinhVucSoSanh.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).Count());
            }

            int tongVu = dataTinh.Sum(s => s.SoVu);
            //Top 5 tỉnh
            var top5Tinh = dataTinh.OrderByDescending(o => o.SoVu).Take(5).ToList();
            var donVi = _sqlContext.DonVi.ToList();
            TopTinhModel topTinhModel;
            int index = 1;
            foreach (var item in top5Tinh)
            {

                topTinhModel = new TopTinhModel()
                {
                    Index = index,
                    DonVi = $"Bộ đội biên phòng {item.TenTinh}",
                    VuNguoi = $"{item.SoVu}/{item.SoNguoi}",
                    TyLe = Math.Round(((decimal)item.SoVu / (decimal)tongVu) * (decimal)100, 0)
                };
                result.ListTopTinh.Add(topTinhModel);
                index++;
            }

            //Top tang vật
            var tangVat = (from a in _sqlContext.TangVatVPHC.AsNoTracking()
                           join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo) on a.IdViPhamHC equals b.Id
                           join c in _sqlContext.DonViTinh.AsNoTracking() on a.IdDonViTinh equals c.IdDonViTinh
                           join d in _sqlContext.LoaiTangVat.AsNoTracking() on a.IdLoaiTangVat equals d.Id
                           select new { a.IdLoaiTangVat, TenTangVat = d.Name, a.IdTangVatVPHC, a.SoLuong, a.IdDonViTinh, DonViTinh = c.Ten }).ToList();
            result.ListTopTangVat = tangVat.GroupBy(s => s.IdLoaiTangVat).Select(s => new TopTangVatModel { TenTangVat = s.First().TenTangVat, SoLuongDonVi = $"{s.Sum(su => su.SoLuong)?.DoubleFomatString()} {s.First().DonViTinh}" }).Take(5).ToList();

            //Tổng quan
            var dataTong = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFrom && a.ThoiGianTiepNhan <= dateTo).ToList();
            result.TongVu = dataTong.Count();
            result.TongNguoiVP = dataTong.Sum(s => s.TongNguoiVP);
            result.TongToChucVP = dataTong.Sum(s => s.TongToChucVP);
            result.TongTienPhat = dataTong.Sum(s => s.TongTienPhat);

            var dataTiepNhan = dataTong.Where(s => s.TienTrinhHoSo == 0);
            result.VuTiepNhan = dataTiepNhan.Count();
            result.NguoiTiepNhan = dataTiepNhan.Sum(s => s.TongNguoiVP) + dataTiepNhan.Sum(s => s.TongToChucVP);
            var dataLapBB = dataTong.Where(s => s.TienTrinhHoSo == 1);
            result.VuLBB = dataLapBB.Count();
            result.NguoiLBB = dataLapBB.Sum(s => s.TongNguoiVP) + dataLapBB.Sum(s => s.TongToChucVP);
            var dataXacMinh = dataTong.Where(s => s.TienTrinhHoSo == 2);
            result.VuXM = dataXacMinh.Count();
            result.NguoiXM = dataXacMinh.Sum(s => s.TongNguoiVP) + dataXacMinh.Sum(s => s.TongToChucVP);
            var dataXuLy = dataTong.Where(s => s.TienTrinhHoSo == 3);
            result.VuXuLy = dataXuLy.Count();
            result.NguoiXuLy = dataXuLy.Sum(s => s.TongNguoiVP) + dataXuLy.Sum(s => s.TongToChucVP);
            var dataKetThuc = dataTong.Where(s => s.TienTrinhHoSo == 4);
            result.VuKetThuc = dataKetThuc.Count();
            result.NguoiKetThuc = dataKetThuc.Sum(s => s.TongNguoiVP) + dataKetThuc.Sum(s => s.TongToChucVP);

            var dataTongSoSanh = _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan >= dateFromSoSanh && a.ThoiGianTiepNhan <= dateToSoSanh).ToList();

            var xuLy = _sqlContext.XuLyVPHC.AsNoTracking().OrderBy(o => o.Order).ToList();
            xuLy.Insert(0, new XuLyVPHC() { Id = "0", Name = "Đang xử lý" });
            int tongXuLy = dataTong.Count();
            XuLyModel xuLyModel;
            string[] colors = ["success", "danger", "warning", "info", "primary", "dark"];
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
                    TyLe = tongXuLy > 0 ? Math.Round(((decimal)tong / (decimal)tongXuLy) * (decimal)100, 0) : 0,
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
            foreach (var item in listLinhVuc)
            {
                var dataLVBC = dataTong.Where(s => item.Id.Equals(s.IdLinhVucBCTH)).ToList();
                result.ListLinhVucDonut.Add(item.ShortName);
                result.ListLinhVucDonutSoVu.Add(dataLVBC.Count);
            }

            //Theo giờ (4h gần nhất)
            DateTime dateFromHour = DateTime.Now;
            DateTime dateToHour = DateTime.Now;
            var dataDonViHour = (from a in _sqlContext.Province.AsNoTracking().Where(s => s.Border)
                                 join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a =>
                                 idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan.Date == dateFromHour.Date) on a.Id equals b.IdTinhPhatHien into ab
                                 select new TinhHinhChungQueryModel()
                                 {
                                     TenTinh = a.Name,
                                     IdTinh = a.Id,
                                     HCKey = a.HCKey,
                                     SoVu = ab.Count(),
                                     SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                 }).Where(s => s.SoVu > 0).ToList();

            var dataDonViTangVat = (from a in _sqlContext.TangVatVPHC.AsNoTracking()
                                    join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && idDonViCapDuoiList.Contains(a.IdDonVi) && a.ThoiGianTiepNhan.Date == dateFromHour.Date) on a.IdViPhamHC equals b.Id
                                    join c in _sqlContext.LoaiTangVat.AsNoTracking() on a.IdLoaiTangVat equals c.Id
                                    join d in _sqlContext.DonViTinh.AsNoTracking() on a.IdDonViTinh equals d.IdDonViTinh
                                    select new
                                    {
                                        b.IdTinhPhatHien,
                                        a.IdLoaiTangVat,
                                        a.SoLuong,
                                        TenTangVat = c.Name,
                                        a.IdDonViTinh,
                                        DonViTinh = d.Ten
                                    }).ToList();
            var dataTangVatGroup = dataDonViTangVat.GroupBy(a => new { a.IdTinhPhatHien, a.IdLoaiTangVat, a.IdDonViTinh }).Select(s => new
            {
                IdTinhPhatHien = s.Key.IdTinhPhatHien,
                TenTangVat = s.First().TenTangVat,
                SoLuong = s.Sum(su => su.SoLuong),
                DonViTinh = s.First().DonViTinh
            }).ToList();

            result.TieuDiemHour.SoVu = dataDonViHour.Sum(s => s.SoVu);
            result.TieuDiemHour.SoNguoi = dataDonViHour.Sum(s => s.SoNguoi);
            foreach (var item in dataDonViHour)
            {
                result.TieuDiemHour.ListTieuDiemDonVi.Add(new TieuDiemItemModel()
                {
                    TenDonVi = $"Bộ đội biên phòng tỉnh {item.TenTinh}",
                    SoVu = item.SoVu,
                    SoNguoi = item.SoNguoi,
                    TangVat = string.Join("; ", dataTangVatGroup.Where(s => item.IdTinh.Equals(s.IdTinhPhatHien)).Select(s => $"{s.SoLuong} {s.DonViTinh.ToLower()} {s.TenTangVat}").ToList())
                });
            }

            //Theo ngày (Ngày hôm nay)
            DateTime dateFromDay = DateTime.Now.AddDays(-1).ToStartDate();
            DateTime dateToDay = DateTime.Now.AddDays(-1).ToEndDate();
            var dataDonViDay = (from a in _sqlContext.Province.AsNoTracking().Where(s => s.Border)
                                join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => !a.IsDelete && a.ThoiGianTiepNhan.Date == dateFromDay.Date) on a.Id equals b.IdTinhPhatHien into ab
                                select new TinhHinhChungQueryModel()
                                {
                                    TenTinh = a.Name,
                                    IdTinh = a.Id,
                                    HCKey = a.HCKey,
                                    SoVu = ab.Count(),
                                    SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                }).Where(s => s.SoVu > 0).ToList();

            DateTime dateFromDaySS = dateFromDay.AddDays(-1).ToStartDate();
            DateTime dateToDaySS = dateFromDay.AddDays(-1).ToEndDate();
            var dataDonViDaySS = (from a in _sqlContext.Province.AsNoTracking().Where(s => s.Border)
                                  join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => a.ThoiGianTiepNhan.Date >= dateFromDaySS.Date && a.ThoiGianTiepNhan.Date <= dateToDaySS.Date) on a.Id equals b.IdTinhPhatHien into ab
                                  select new TinhHinhChungQueryModel()
                                  {
                                      TenTinh = a.Name,
                                      IdTinh = a.Id,
                                      HCKey = a.HCKey,
                                      SoVu = ab.Count(),
                                      SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                  }).Where(s => s.SoVu > 0).ToList();

            result.TieuDiemDay.SoVu = dataDonViDay.Sum(s => s.SoVu);
            result.TieuDiemDay.SoNguoi = dataDonViDay.Sum(s => s.SoNguoi);
            foreach (var item in dataDonViDay)
            {
                var dataSS = dataDonViDaySS.Where(r => item.IdTinh.Equals(r.IdTinh)).FirstOrDefault();
                int soVu = item.SoVu - (dataSS?.SoVu ?? 0);
                int soNguoi = item.SoNguoi - (dataSS?.SoNguoi ?? 0);
                result.TieuDiemDay.ListTieuDiemDonVi.Add(new TieuDiemItemModel()
                {
                    TenDonVi = $"Bộ đội biên phòng {item.TenTinh}",
                    SoVu = item.SoVu,
                    SoNguoi = item.SoNguoi,
                    TangGiamVu = soVu > 0 ? 1 : (soVu < 0 ? -1 : 0),
                    GiaTriTangGiamVu = soVu > 0 ? soVu : (soVu < 0 ? soVu * (-1) : 0),
                    TangGiamNguoi = soNguoi > 0 ? 1 : (soNguoi < 0 ? -1 : 0),
                    GiaTriTangGiamNguoi = soNguoi > 0 ? soVu : (soNguoi < 0 ? soNguoi * (-1) : 0)
                });
            }

            //Theo tuần (7 ngày trước ngày hôm nay)
            DateTime dateFromWeek = DateTime.Now.AddDays(-7).ToStartDate();
            DateTime dateToWeek = DateTime.Now.AddDays(-1).ToEndDate();
            var dataDonViWeek = (from a in _sqlContext.Province.AsNoTracking()
                                 join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => a.ThoiGianTiepNhan.Date >= dateFromWeek.Date && a.ThoiGianTiepNhan.Date <= dateToWeek.Date) on a.Id equals b.IdTinhPhatHien into ab
                                 select new TinhHinhChungQueryModel()
                                 {
                                     TenTinh = a.Name,
                                     IdTinh = a.Id,
                                     HCKey = a.HCKey,
                                     SoVu = ab.Count(),
                                     SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                 }).Where(s => s.SoVu > 0).ToList();

            DateTime dateFromWeekSS = dateFromWeek.AddDays(-7).ToStartDate();
            DateTime dateToWeekSS = dateFromWeek.AddDays(-1).ToEndDate();
            var dataDonViWeekSS = (from a in _sqlContext.Province.AsNoTracking()
                                   join b in _sqlContext.VuViecVPHC.AsNoTracking().Where(a => a.ThoiGianTiepNhan.Date >= dateFromWeekSS.Date && a.ThoiGianTiepNhan.Date <= dateToWeekSS.Date) on a.Id equals b.IdTinhPhatHien into ab
                                   select new TinhHinhChungQueryModel()
                                   {
                                       TenTinh = a.Name,
                                       IdTinh = a.Id,
                                       HCKey = a.HCKey,
                                       SoVu = ab.Count(),
                                       SoNguoi = ab.Sum(s => s.TongNguoiVP + s.TongToChucVP)
                                   }).Where(s => s.SoVu > 0).ToList();

            result.TieuDiemWeek.SoVu = dataDonViWeek.Sum(s => s.SoVu);
            result.TieuDiemWeek.SoNguoi = dataDonViWeek.Sum(s => s.SoNguoi);
            foreach (var item in dataDonViWeek)
            {
                var dataSS = dataDonViWeekSS.Where(r => item.IdTinh.Equals(r.IdTinh)).FirstOrDefault();
                int soVu = item.SoVu - (dataSS?.SoVu ?? 0);
                int soNguoi = item.SoNguoi - (dataSS?.SoNguoi ?? 0);
                result.TieuDiemWeek.ListTieuDiemDonVi.Add(new TieuDiemItemModel()
                {
                    TenDonVi = $"Bộ đội biên phòng {item.TenTinh}",
                    SoVu = item.SoVu,
                    SoNguoi = item.SoNguoi,
                    TangGiamVu = soVu > 0 ? 1 : (soVu < 0 ? -1 : 0),
                    GiaTriTangGiamVu = soVu > 0 ? soVu : (soVu < 0 ? soVu * (-1) : 0),
                    TangGiamNguoi = soNguoi > 0 ? 1 : (soNguoi < 0 ? -1 : 0),
                    GiaTriTangGiamNguoi = soNguoi > 0 ? soVu : (soNguoi < 0 ? soNguoi * (-1) : 0)
                });
            }

            return result;
        }
    }
}
