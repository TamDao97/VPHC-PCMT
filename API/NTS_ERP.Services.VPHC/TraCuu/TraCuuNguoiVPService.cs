using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Document.Excel;
using NTS_ERP.Models.Cores.Combobox;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.TraCuu;
using NTS_ERP.Services.Cores;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.VPHC.ChungChiGiayPhep;
using NTS_ERP.Services.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhuongTien;
using NTS_ERP.Services.VPHC.TangVat;
using NTS_ERP.Services.VPHC.ToChucVP;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.TraCuu
{
    public class TraCuuNguoiVPService : ITraCuuNguoiVPService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IExcelService _excelService;
        private IComboboxService _comboboxService;
        private readonly INguoiVPService _nguoiVPService;
        private readonly IToChucVPService _toChucVPService;
        private readonly IPhienDichService _phienDichService;
        private readonly ITangVatService _tangVatService;
        private readonly IPhuongTienService _phuongTienService;
        private readonly INguoiChungKienService _chungKienService;
        private readonly IChungChiGiayPhepService _chungChiGiayPhep;
        public TraCuuNguoiVPService(NTS_ERPContext sqlContext, IExcelService excelService, IComboboxService comboboxService, INguoiVPService nguoiVPService, IToChucVPService toChucVPService,
            ITangVatService tangVatService, IPhienDichService phienDichService, IPhuongTienService phuongTienService, INguoiChungKienService chungKienService,
            IChungChiGiayPhepService chungChiGiayPhep
            )
        {
            this._sqlContext = sqlContext;
            this._excelService = excelService;
            this._comboboxService = comboboxService;
            this._nguoiVPService = nguoiVPService;
            this._toChucVPService = toChucVPService;
            this._phienDichService = phienDichService;
            this._tangVatService = tangVatService;
            this._phuongTienService = phuongTienService;
            this._chungKienService = chungKienService;
            this._chungChiGiayPhep = chungChiGiayPhep;
        }

        /// <summary>
        /// Tìm kiếm vụ việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<TraCuuBaseResultModel> SearchAsync(TraCuuSearchModel searchModel)
        {
            TraCuuBaseResultModel searchResult = new TraCuuBaseResultModel();

            var dataQuery = MakeWhereCondition(searchModel);

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            else
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, "MaHoSo");
            }


            searchResult.TongLanVP = dataQuery.Count();
            searchResult.TongTienPhat = dataQuery.Sum(s => s.TongTienPhat);
            searchResult.NguoiMT = dataQuery.Where(w => "1".Equals(w.LinhVuc)).Count();
            searchResult.NguoiBL = dataQuery.Where(w => "2".Equals(w.LinhVuc)).Count();
            searchResult.NguoiMBN = dataQuery.Where(w => "3".Equals(w.LinhVuc)).Count();
            searchResult.NguoiVKVLN = dataQuery.Where(w => "4".Equals(w.LinhVuc)).Count();
            searchResult.NguoiXNC = dataQuery.Where(w => "5".Equals(w.LinhVuc)).Count();
            searchResult.NguoiKhac = dataQuery.Where(w => "6".Equals(w.LinhVuc)).Count();

            var dataGroupQuery = dataQuery.GroupBy(g => g.IdNguoi).Select(s => new TraCuuGroupQueryModel()
            {
                IdNguoiVP = string.Join(';', s.Select(se => se.Id)),
                IdDonVi = string.Join(';', s.Select(se => se.IdDonVi)),
                MaHoSo = string.Join(';', s.Select(se => se.MaHoSo)),
                NgayViPham = s.Select(se => se.NgayViPham),
                IdNguoi = s.Key,
                HoVaTen = s.First().HoVaTen ?? "",
                GioiTinh = s.First().GioiTinh,
                NgaySinh = s.First().NgaySinh,
                QueQuan = s.First().QueQuan ?? "",
                LinhVuc = string.Join(';', s.Select(se => se.LinhVuc)),
                TienTrinhHoSo = s.First().TienTrinhHoSo,
                XuLy = s.First().XuLy,
                SoLanViPham = s.Count(),
                TongTienPhat = s.Sum(su => su.TongTienPhat),
            });

            searchResult.TotalItems = dataGroupQuery.Count();

            var donVi = _sqlContext.DonVi.AsNoTracking().Select(s => new ComboboxModel { Id = s.IdDonVi, Name = s.Ten, ObjectId = s.IdLoaiDonVi }).ToList();
            var linhVuc = _sqlContext.LinhVucThongKeBCTH.AsNoTracking().Select(s => new ComboboxModel { Id = s.Id, Name = s.Name }).ToList();
            var xuLyVPHC = _sqlContext.XuLyVPHC.AsNoTracking().Select(s => new ComboboxModel { Id = s.Id, Name = s.Name }).ToList();
            xuLyVPHC.Add(new ComboboxModel() { Id = "0", Name = "Đang xử lý"});
            searchResult.DataResults = dataGroupQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) => new TraCuuSearchResultModel()
            {
                Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1,
                Id = s.IdNguoi,
                DonVi = string.Join('\n', donVi.Where(w => s.IdDonVi.Contains(w.Id)).Select(se => se.Name)),
                MaHoSo = s.MaHoSo,
                NgayViPham = string.Join('\n', s.NgayViPham.Select(s => s.ToStringDDMMYY())),
                HoVaTen = s.HoVaTen,
                GioiTinh = s.GioiTinh.ToStringGender(),
                NgaySinh = s.NgaySinh,
                QueQuan = s.QueQuan,
                LinhVuc = string.Join('\n', linhVuc.Where(w => s.LinhVuc.Contains(w.Id)).Select(se => se.Name)),
                TienTrinhHoSo = StatusHelper.GetStatusName(GroupsHelper.TienTrinhVuViec, s.TienTrinhHoSo),
                XuLy = xuLyVPHC.FirstOrDefault(a => a.Id.Equals(s.XuLy))?.Name ?? "",
                SoLanViPham = s.SoLanViPham,
                TongTienPhat = s.TongTienPhat,
            }).ToList();
            return searchResult;
        }

        private IQueryable<TraCuuQueryModel> MakeWhereCondition(TraCuuSearchModel searchModel)
        {
            var dataQuery = (from a in _sqlContext.NguoiVPHC.AsNoTracking()
                             join b in _sqlContext.Nguoi.AsNoTracking() on a.IdNguoi equals b.IdNguoi
                             join c in _sqlContext.QuyetDinhXuPhat.AsNoTracking() on a.IdNguoiVPHC equals c.IdNguoiViPham into ac
                             from c in ac.DefaultIfEmpty()
                             join d in _sqlContext.VuViecVPHC.AsNoTracking() on a.IdViPhamHC equals d.Id
                             where !d.IsDelete
                             select new TraCuuQueryModel()
                             {
                                 Id = a.IdNguoiVPHC,
                                 MaHoSo = d.MaHoSo,
                                 IdDonVi = d.IdDonVi,
                                 NgayViPham = d.ThoiGianTiepNhan,
                                 IdNguoi = b.IdNguoi,
                                 HoVaTen = b.HoVaTen,
                                 GioiTinh = b.GioiTinh,
                                 NgaySinh = b.NgaySinh,
                                 QueQuan = b.DiaChiDayDu,
                                 LinhVuc = d.IdLinhVucBCTH,
                                 TienTrinhHoSo = d.TienTrinhHoSo,
                                 XuLy = d.IdXuLy,
                                 TongTienPhat = c.MucPhat ?? 0
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.IdDonVi))
            {
                var listDonVi = GlobalData.ListAllDonVi.Count > 0 ? GlobalData.ListAllDonVi : _sqlContext.DonVi.ToList();
                var donViSearch = listDonVi.Where(s => s.IdDonVi.Equals(searchModel.IdDonVi)).FirstOrDefault();
                //Nếu là bộ chỉ huy tỉnh
                if (donViSearch.Level == 1 || (donViSearch.Level == 2 && donViSearch.IdLoaiDonVi == "DV03"))
                {
                    string idDonViCapDuoiList = String.Join(";", _comboboxService.GetDonViByIdDonVi(searchModel.IdDonVi).Result.Select(r => r.Id).ToList());
                    if (!string.IsNullOrEmpty(idDonViCapDuoiList))
                    {
                        dataQuery = dataQuery.Where(i => idDonViCapDuoiList.Contains(i.IdDonVi));
                    }
                }
                else
                {
                    dataQuery = dataQuery.Where(a => searchModel.IdDonVi.Equals(a.IdDonVi));
                }
            }

            if (!string.IsNullOrEmpty(searchModel.HoVaTen))
            {
                dataQuery = dataQuery.Where(a => a.HoVaTen.ToUpper().Contains(searchModel.HoVaTen.ToUpper()));
            }

            if (searchModel.NgaySinhFrom.HasValue)
            {
                searchModel.NgaySinhFrom = searchModel.NgaySinhFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.NgaySinh >= searchModel.NgaySinhFrom);
            }

            if (searchModel.NgaySinhTo.HasValue)
            {
                searchModel.NgaySinhTo = searchModel.NgaySinhTo.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.NgaySinh <= searchModel.NgaySinhTo);
            }

            if (searchModel.GioiTinh.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.GioiTinh == searchModel.GioiTinh);
            }

            if (!string.IsNullOrEmpty(searchModel.QueQuan))
            {
                dataQuery = dataQuery.Where(a => (a.QueQuan ?? "").ToUpper().Contains(searchModel.QueQuan.ToUpper()));
            }

            if (searchModel.NgayViPhamFrom.HasValue)
            {
                searchModel.NgayViPhamFrom = searchModel.NgayViPhamFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.NgayViPham >= searchModel.NgayViPhamFrom);
            }

            if (searchModel.NgayViPhamTo.HasValue)
            {
                searchModel.NgayViPhamTo = searchModel.NgayViPhamTo.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.NgayViPham >= searchModel.NgayViPhamTo);
            }

            if (!string.IsNullOrEmpty(searchModel.LinhVuc))
            {
                dataQuery = dataQuery.Where(a => (a.LinhVuc ?? "").Equals(searchModel.LinhVuc.ToUpper()));
            }

            if (searchModel.TienTrinh.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.TienTrinhHoSo == searchModel.TienTrinh);
            }
            return dataQuery;
        }

        /// <summary>
        /// Xuất danh sách vụ việc
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExportFileAsync(TraCuuSearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport)
        {
            searchModel.NoPagination = true;
            SearchBaseResultModel<TraCuuSearchResultModel> searchResult = SearchAsync(searchModel).Result;

            TraCuuExportModel traCuuExportModel = new TraCuuExportModel();
            traCuuExportModel.NgayXuat = DateTime.Now.ToStringDDMMYY();
            traCuuExportModel.Data = searchResult.DataResults.Select(a => new ItemExportModel()
            {
                Index = a.Index,
                HoVaTen = a.HoVaTen,
                NgaySinh = a.NgaySinh?.ToStringDDMMYY() ?? "",
                GioiTinh = a.GioiTinh,
                QueQuan = a.QueQuan,
                DonVi = a.DonVi,
                NgayViPham = a.NgayViPham,
                LinhVuc = a.LinhVuc,
                TienTrinhHoSo = a.TienTrinhHoSo,
                SoLanViPham = a.SoLanViPham,
                XuLy = a.XuLy,
                TongTienPhat = a.TongTienPhat.NTSToStringCurrency()
            }).ToList();

            MemoryStream streamFile = null;
            if (optionExport == NTSConstants.OptionExport.Excel)
            {
                streamFile = _excelService.ExportExcel<TraCuuExportModel>(pathTemplate, traCuuExportModel);
            }
            else if (optionExport == NTSConstants.OptionExport.Pdf)
            {
                streamFile = _excelService.ExportExcelConvertToPdf(pathTemplate, traCuuExportModel);
            }

            return streamFile;
        }

        /// <summary>
        /// Thống kê nhanh web
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<List<TreeItem>> QuickDashboard(TraCuuSearchModel searchModel)
        {
            List<TreeItem> quickTreeView = new List<TreeItem>();
            TreeItem treeItem;
            TreeSubItem treeSubItem;
            //try
            //{
            //    var dataQuery = MakeWhereCondition(searchModel).ToList();

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số người vi phạm";
            //    treeItem.Expanded = true;
            //    treeItem.Total = dataQuery.Sum(s => s.TongNguoiVP);

            //    treeItem.Child = new List<TreeSubItem>();
            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Chưa kết luận";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Có hành vi vi phạm";
            //    treeSubItem.Total = dataQuery.Sum(s => s.TongNguoiVP);
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Không có hành vi vi phạm";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Theo độ tuổi";
            //    treeItem.Expanded = true;

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nam dưới 16 tuổi";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nam từ đủ 16 đến dưới 18 tuổi";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nam từ đủ 18 tuổi trở lên";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nữ dưới 16 tuổi";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nữ từ đủ 16 đến dưới 18 tuổi";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tổng số người vi phạm nữ từ đủ 18 tuổi trở lên";
            //    treeSubItem.Total = 0;
            //    treeItem.Child.Add(treeSubItem);
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số tổ chức vi phạm";
            //    treeItem.Total = dataQuery.Sum(s => s.TongToChucVP);
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng trị giá tang vật tiêu huỷ";
            //    treeItem.Total = 0;
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng trị giá tang vật bán phát mại";
            //    treeItem.Total = 0;
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng tiền xử phạt VPHC";
            //    treeItem.Total = (decimal)dataQuery.Sum(a => a.TongTienPhat);
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số quyết định";
            //    treeItem.Total = dataQuery.Sum(s => s.TongToChucVP) + dataQuery.Sum(s => s.TongNguoiVP);
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số quyết định đã thi hành";
            //    treeItem.Total = dataQuery.Sum(s => s.TongNguoiVP);
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số quyết định hoãn, miễn, giảm";
            //    treeItem.Total = 0;
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số quyết định cưỡng chế thi hành";
            //    treeItem.Total = 0;
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số quyết định bị khiếu nại, khiếu kiện";
            //    treeItem.Total = 0;
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số vụ chuyển truy cứu trách nhiệm hình sự";
            //    treeItem.Total = dataQuery.Where(a => a.IdXuLy.Equals(NTSConstants.VPHC_XuLy_ChuyenDTHS)).Count();
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = $"Tổng số vụ áp dụng BP nhắc nhở người chưa thành niên";
            //    treeItem.Total = dataQuery.Where(a => a.IdXuLy.Equals(NTSConstants.VPHC_XuLy_ApDungNhacNho)).Count();
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = "Nguồn phát hiện";
            //    treeItem.Expanded = true;
            //    var nguonTin = _sqlContext.NguonTinVPHC.AsNoTracking().ToList();
            //    foreach (var itemNguonTin in nguonTin)
            //    {
            //        treeSubItem = new TreeSubItem();
            //        treeSubItem.Text = $"{itemNguonTin.Name}";
            //        treeSubItem.Total = dataQuery.Where(r => r.IdNguonPhatHien.Equals(itemNguonTin.Id)).Count();
            //        treeItem.Child.Add(treeSubItem);
            //    }
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = "Phân loại";
            //    treeItem.Expanded = true;
            //    quickTreeView.Add(treeItem);
            //    var listPhanLoaiTin = StatusHelper.GetListStatus(GroupsHelper.ThamQuyenXL);
            //    foreach (var item in listPhanLoaiTin)
            //    {
            //        treeSubItem = new TreeSubItem();
            //        treeSubItem.Text = $"{item.Name}";
            //        treeSubItem.Total = dataQuery.Where(r => (int)item.Id == r.PhanLoai).Count();
            //        treeItem.Child.Add(treeSubItem);
            //    }

            //    treeItem = new TreeItem();
            //    treeItem.Text = "Xử lý";
            //    treeItem.Expanded = true;
            //    var xuLyVPHC = _sqlContext.XuLyVPHC.AsNoTracking().ToList();
            //    foreach (var itemXuLy in xuLyVPHC)
            //    {
            //        treeSubItem = new TreeSubItem();
            //        treeSubItem.Text = $"{itemXuLy.Name}";
            //        treeSubItem.Total = dataQuery.Where(r => r.IdXuLy.Equals(itemXuLy.Id)).Count();
            //        treeItem.Child.Add(treeSubItem);
            //    }
            //    quickTreeView.Add(treeItem);

            //    treeItem = new TreeItem();
            //    treeItem.Text = "Tổng theo tiến trình vụ việc";
            //    treeItem.Expanded = true;

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Tiếp nhận, phát hiện";
            //    treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 0).Count();
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Kiểm tra, xác minh";
            //    treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 1).Count();
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Xử lý";
            //    treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 2).Count();
            //    treeItem.Child.Add(treeSubItem);

            //    treeSubItem = new TreeSubItem();
            //    treeSubItem.Text = $"Kết thúc";
            //    treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 3).Count();
            //    treeItem.Child.Add(treeSubItem);
            //    quickTreeView.Add(treeItem);
            //}
            //catch { }
            return quickTreeView;
        }
    }
}
