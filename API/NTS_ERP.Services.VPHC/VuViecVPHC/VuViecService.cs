using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS.Document.Excel;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Combobox;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.TreeView;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.CanBo;
using NTS_ERP.Models.VPHC.VuViec;
using NTS_ERP.Services.Cores;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.VPHC.ChungChiGiayPhep;
using NTS_ERP.Services.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhuongTien;
using NTS_ERP.Services.VPHC.Signalr;
using NTS_ERP.Services.VPHC.TangVat;
using NTS_ERP.Services.VPHC.ToChucVP;
using TrafficControl.Core;
using Wangkanai.Extensions;

namespace NTS_ERP.Services.VPHC.VuViecVPHC
{
    public class VuViecService : IVuViecService
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
        private readonly IHubContext<SignalrHubVPHC, IHubVPHCClient> _signalrHub;
        public VuViecService(NTS_ERPContext sqlContext, IExcelService excelService, IComboboxService comboboxService, INguoiVPService nguoiVPService, IToChucVPService toChucVPService,
            ITangVatService tangVatService, IPhienDichService phienDichService, IPhuongTienService phuongTienService, INguoiChungKienService chungKienService,
            IChungChiGiayPhepService chungChiGiayPhep, IHubContext<SignalrHubVPHC, IHubVPHCClient> signalrHub)
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
            this._signalrHub = signalrHub;
        }

        /// <summary>
        /// Tìm kiếm vụ việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<VuViecBaseResultModel> SearchAsync(VuViecSearchModel searchModel)
        {
            VuViecBaseResultModel searchResult = new VuViecBaseResultModel();

            var dataQuery = MakeWhereCondition(searchModel);

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            else
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, "MaHoSo");
            }

            searchResult.TotalItems = dataQuery.Count();
            searchResult.TongNguoiVP = dataQuery.Sum(s => s.TongNguoiVP);
            searchResult.TongToChucVP = dataQuery.Sum(s => s.TongToChucVP);
            searchResult.TongTienPhat = dataQuery.Sum(s => s.TongTienPhat);
            searchResult.VuTiepNhan = dataQuery.Where(w => 0 == w.TienTrinhHoSo).Count();
            searchResult.NguoiTiepNhan = dataQuery.Where(w => 0 == w.TienTrinhHoSo).Sum(s => s.TongToChucVP + s.TongNguoiVP);
            searchResult.VuLBB = dataQuery.Where(w => 1 == w.TienTrinhHoSo).Count();
            searchResult.NguoiLBB = dataQuery.Where(w => 1 == w.TienTrinhHoSo).Sum(s => s.TongToChucVP + s.TongNguoiVP);
            searchResult.VuXM = dataQuery.Where(w => 2 == w.TienTrinhHoSo).Count();
            searchResult.NguoiXM = dataQuery.Where(w => 2 == w.TienTrinhHoSo).Sum(s => s.TongToChucVP + s.TongNguoiVP);
            searchResult.VuXuLy = dataQuery.Where(w => 3 == w.TienTrinhHoSo).Count();
            searchResult.NguoiXuLy = dataQuery.Where(w => 3 == w.TienTrinhHoSo).Sum(s => s.TongToChucVP + s.TongNguoiVP);
            searchResult.VuKetThuc = dataQuery.Where(w => 4 == w.TienTrinhHoSo).Count();
            searchResult.NguoiKetThuc = dataQuery.Where(w => 4 == w.TienTrinhHoSo).Sum(s => s.TongToChucVP + s.TongNguoiVP);

            var donVi = _sqlContext.DonVi.AsNoTracking().Select(s => new ComboboxModel { Id = s.IdDonVi, Name = s.Ten, ObjectId = s.IdLoaiDonVi }).ToList();
            var nguonTin = _sqlContext.NguonTinVPHC.AsNoTracking().Select(s => new ComboboxModel { Id = s.Id, Name = s.Name }).ToList();
            var xuLyVPHC = _sqlContext.XuLyVPHC.AsNoTracking().Select(s => new ComboboxModel { Id = s.Id, Name = s.Name }).ToList();
            xuLyVPHC.Add(new ComboboxModel() { Id = "0", Name = "Đang xử lý" });
            searchResult.DataResults = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) => new VuViecSearchResultModel()
            {
                Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1,
                Id = s.Id,
                DonVi = donVi.FirstOrDefault(a => a.Id.Equals(s.IdDonVi))?.Name ?? "",
                MaHoSo = s.MaHoSo,
                NguonPhatHien = s.IdNguonPhatHien,
                ThoiGianTiepNhan = s.ThoiGianTiepNhan,
                DienBien = s.DienBien,
                GhiChu = s.GhiChu,
                DiaChiDayDu = s.DiaChiDayDu,
                PhanLoai = StatusHelper.GetStatusName(GroupsHelper.ThamQuyenXL, s.PhanLoai),
                TongNguoiVP = s.TongNguoiVP,
                TongToChucVP = s.TongToChucVP,
                TongTienPhat = s.TongTienPhat,
                XuLy = xuLyVPHC.FirstOrDefault(a => a.Id.Equals(s.IdXuLy))?.Name ?? "",
                TienTrinhHoSo = StatusHelper.GetStatusName(GroupsHelper.TienTrinhVuViec, s.TienTrinhHoSo),
            }).ToList();
            return searchResult;
        }

        private IQueryable<VuViecQueryModel> MakeWhereCondition(VuViecSearchModel searchModel)
        {
            var dataQuery = (from a in _sqlContext.VuViecVPHC.AsNoTracking()
                             where !a.IsDelete
                             select new VuViecQueryModel()
                             {
                                 Id = a.Id,
                                 MaHoSo = a.MaHoSo,
                                 IdDonVi = a.IdDonVi,
                                 IdNguonPhatHien = a.IdNguonPhatHien,
                                 ThoiGianTiepNhan = a.ThoiGianTiepNhan,
                                 DiaChiDayDu = a.DiaChiDayDu,
                                 DienBien = a.DienBien,
                                 IdTinhPhatHien = a.IdTinhPhatHien,
                                 IdHuyenPhatHien = a.IdHuyenPhatHien,
                                 IdXaPhatHien = a.IdXaPhatHien,
                                 PhanLoai = a.PhanLoai,
                                 IdXuLy = a.IdXuLy,
                                 TienTrinhHoSo = a.TienTrinhHoSo,
                                 TongNguoiVP = a.TongNguoiVP,
                                 TongToChucVP = a.TongToChucVP,
                                 TongTienPhat = a.TongTienPhat,
                                 IdLinhVucTKBC = a.IdLinhVucBCTH
                             }).AsQueryable();

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

            if (!string.IsNullOrEmpty(searchModel.MaHoSo))
            {
                dataQuery = dataQuery.Where(a => a.MaHoSo.ToUpper().Contains(searchModel.MaHoSo.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.DienBien))
            {
                dataQuery = dataQuery.Where(a => a.DienBien.ToUpper().Contains(searchModel.DienBien.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.IdNguonPhatHien))
            {
                dataQuery = dataQuery.Where(a => searchModel.IdNguonPhatHien.Equals(a.IdNguonPhatHien));
            }

            if (searchModel.ThoiGianTiepNhanFrom.HasValue)
            {
                searchModel.ThoiGianTiepNhanFrom = searchModel.ThoiGianTiepNhanFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.ThoiGianTiepNhan >= searchModel.ThoiGianTiepNhanFrom);

            }
            if (searchModel.ThoiGianTiepNhanTo.HasValue)
            {
                searchModel.ThoiGianTiepNhanTo = searchModel.ThoiGianTiepNhanTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ThoiGianTiepNhan <= searchModel.ThoiGianTiepNhanTo);
            }

            if (!string.IsNullOrEmpty(searchModel.DiaDiemPhatHien))
            {
                dataQuery = dataQuery.Where(a => searchModel.DiaDiemPhatHien.ToUpper().Contains(a.DiaChiDayDu.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.LinhVuc))
            {
                dataQuery = dataQuery.Where(a => a.IdLinhVucTKBC.Equals(searchModel.LinhVuc));
            }

            if (searchModel.PhanLoai.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.PhanLoai == searchModel.PhanLoai);
            }

            if (!string.IsNullOrEmpty(searchModel.IdXuLy))
            {
                dataQuery = dataQuery.Where(a => searchModel.IdXuLy.Equals(a.IdXuLy));
            }

            //public int? TienTrinhHoSo { get; set; }

            return dataQuery;
        }

        /// <summary>
        /// Thêm mới vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> CreateAsync(VuViecModifyModel model, CurrentUserModel currentUser)
        {
            model.Id = Guid.NewGuid().ToString();
            Models.Entities.VuViecVPHC viPhamHC = new Models.Entities.VuViecVPHC();
            viPhamHC.Id = model.Id;
            viPhamHC.IdDonVi = currentUser.DonViId;
            viPhamHC.MaHoSo = $"{model.IdDonVi}.{DateTime.Now.ToString(NTSConstants.DateFormatKey)}";
            viPhamHC.IdNguonPhatHien = model.IdNguonPhatHien ?? "";
            viPhamHC.ThoiGianTiepNhan = model.ThoiGianTiepNhan ?? DateTime.Now;
            viPhamHC.DienBien = model.DienBien;
            viPhamHC.GhiChu = model.GhiChu;
            viPhamHC.LoaiDiaDiem = model.LoaiDiaDiem;
            viPhamHC.IdTinhPhatHien = model.IdTinhPhatHien ?? "";
            viPhamHC.IdHuyenPhatHien = model.IdHuyenPhatHien;
            viPhamHC.IdXaPhatHien = model.IdXaPhatHien;
            viPhamHC.DiaChiChiTiet = model.DiaChiChiTiet;
            viPhamHC.DiaChiDayDu = this.GhepDiaChi(viPhamHC.IdTinhPhatHien, viPhamHC.IdHuyenPhatHien ?? "", viPhamHC.IdXaPhatHien ?? "", viPhamHC.DiaChiChiTiet ?? "");
            viPhamHC.PhanLoai = model.PhanLoai ?? 0;
            viPhamHC.CanBoDieuTra = model.CanBoDieuTra;
            viPhamHC.KetQuaDieuTra = model.KetQuaDieuTra;
            viPhamHC.TongNguoiVP = model.ListNguoiVP?.Count() ?? 0;
            viPhamHC.TongToChucVP = model.ListToChucVP?.Count() ?? 0;
            viPhamHC.IdXuLy = model.IdXuLy ?? "0";
            viPhamHC.TienTrinhHoSo = model.TienTrinhHoSo;
            viPhamHC.IdLinhVucBCTH = model.IdLinhVucBCTH;
            viPhamHC.CreateBy = currentUser.UserId;
            viPhamHC.CreateDate = DateTime.Now;
            viPhamHC.UpdateBy = currentUser.UserId;
            viPhamHC.UpdateDate = DateTime.Now;
            if (string.IsNullOrEmpty(viPhamHC.MaHoSo))
            {
                viPhamHC.MaHoSo = $"{viPhamHC.IdDonVi}.{viPhamHC.CreateDate.Value.ToString(NTSConstants.DateFormatKey)}";
            }

            _sqlContext.VuViecVPHC.Add(viPhamHC);

            await this._nguoiVPService.Add(_sqlContext, model.ListNguoiVP, viPhamHC.Id, currentUser);

            await this._toChucVPService.Add(_sqlContext, model.ListToChucVP, viPhamHC.Id, currentUser.UserId);

            await this._phienDichService.Add(_sqlContext, model.ListPhienDich, viPhamHC.Id, currentUser.UserId);

            await this._chungKienService.Add(_sqlContext, model.ListChungKien, viPhamHC.Id, currentUser.UserId);

            await this._tangVatService.Add(_sqlContext, model.ListTangVat, viPhamHC.Id, currentUser.UserId);

            await this._phuongTienService.Add(_sqlContext, model.ListPhuongTien, viPhamHC.Id, currentUser.UserId);

            await this._chungChiGiayPhep.Add(_sqlContext, model.ListGiayPhepChungChi, viPhamHC.Id, currentUser.UserId);

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();

                    int tongNguoi = viPhamHC.TongNguoiVP + viPhamHC.TongToChucVP;
                    string nguoi = tongNguoi > 0 ? $". Gồm: {tongNguoi} người vi phạm" : "";
                    string tangVat = model.ListTangVat.Count > 0 ? $". Tang vật: {string.Join("; ", model.ListTangVat.Select(s => $"{s.SoLuong} {s.TenDonViTinh} {s.TenLoaiTangVat}"))}" : "";
                    string donVi = (from a in _sqlContext.DonVi.Where(w => w.IdDonVi.Equals(model.IdDonVi))
                                    join b in _sqlContext.Province on a.IdTinh equals b.Id into ab
                                    from b in ab.DefaultIfEmpty()
                                    select $"{a.Ten} {b.Name}").FirstOrDefault() ?? "";
                    string linhVuc = _sqlContext.LinhVucThongKeBCTH.FirstOrDefault(s => s.Id.Equals(viPhamHC.IdLinhVucBCTH))?.Name ?? "";
                    await this._signalrHub.Clients.All.NotifyVuViec(viPhamHC.Id, donVi, $"Phát hiện vụ việc {linhVuc} lúc {viPhamHC.ThoiGianTiepNhan.ToStringHHMMDDMMYY()}.{nguoi}{tangVat}");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
            return viPhamHC.Id;
        }

        /// <summary>
        /// Cập nhật thông tin vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, VuViecModifyModel model, CurrentUserModel currentUser)
        {
            var vuViecUpdate = _sqlContext.VuViecVPHC.FirstOrDefault(i => i.Id.Equals(id));

            if (vuViecUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            vuViecUpdate.IdDonVi = currentUser.DonViId;
            vuViecUpdate.IdNguonPhatHien = model.IdNguonPhatHien ?? "";
            vuViecUpdate.ThoiGianTiepNhan = model.ThoiGianTiepNhan ?? DateTime.Now;
            vuViecUpdate.DienBien = model.DienBien;
            vuViecUpdate.GhiChu = model.GhiChu;
            vuViecUpdate.LoaiDiaDiem = model.LoaiDiaDiem;
            vuViecUpdate.IdTinhPhatHien = model.IdTinhPhatHien ?? "";
            vuViecUpdate.IdHuyenPhatHien = model.IdHuyenPhatHien;
            vuViecUpdate.IdXaPhatHien = model.IdXaPhatHien;
            vuViecUpdate.DiaChiChiTiet = model.DiaChiChiTiet;
            vuViecUpdate.DiaChiDayDu = this.GhepDiaChi(vuViecUpdate.IdTinhPhatHien, vuViecUpdate.IdHuyenPhatHien, vuViecUpdate.IdXaPhatHien, vuViecUpdate.DiaChiChiTiet);
            vuViecUpdate.PhanLoai = model.PhanLoai ?? 0;
            vuViecUpdate.CanBoDieuTra = model.CanBoDieuTra;
            vuViecUpdate.KetQuaDieuTra = model.KetQuaDieuTra;
            vuViecUpdate.TongNguoiVP = model.ListNguoiVP?.Count() ?? 0;
            vuViecUpdate.TongToChucVP = model.ListToChucVP?.Count() ?? 0;
            vuViecUpdate.IdXuLy = model.IdXuLy ?? "0";
            vuViecUpdate.TienTrinhHoSo = model.TienTrinhHoSo;
            vuViecUpdate.IdLinhVucBCTH = model.IdLinhVucBCTH;
            vuViecUpdate.UpdateBy = currentUser.UserId;
            vuViecUpdate.UpdateDate = DateTime.Now;

            await this._nguoiVPService.Add(_sqlContext, model.ListNguoiVP, vuViecUpdate.Id, currentUser);

            await this._toChucVPService.Add(_sqlContext, model.ListToChucVP, vuViecUpdate.Id, currentUser.UserId);

            await this._phienDichService.Add(_sqlContext, model.ListPhienDich, vuViecUpdate.Id, currentUser.UserId);

            await this._chungKienService.Add(_sqlContext, model.ListChungKien, vuViecUpdate.Id, currentUser.UserId);

            await this._tangVatService.Add(_sqlContext, model.ListTangVat, vuViecUpdate.Id, currentUser.UserId);

            await this._phuongTienService.Add(_sqlContext, model.ListPhuongTien, vuViecUpdate.Id, currentUser.UserId);

            await this._chungChiGiayPhep.Add(_sqlContext, model.ListGiayPhepChungChi, vuViecUpdate.Id, currentUser.UserId);

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Lấy thông tin vụ việc
        /// </summary>
        /// <param name="VuViecId"></param>
        /// <returns></returns>
        public async Task<VuViecModifyModel> GetByIdAsnyc(string id)
        {
            var vuViec = _sqlContext.VuViecVPHC.Where(r => r.Id.Equals(id)).FirstOrDefault();

            if (vuViec == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            VuViecModifyModel outputModel = JsonConvert.DeserializeObject<VuViecModifyModel>(JsonConvert.SerializeObject(vuViec, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            outputModel.ListNguoiVP = this._nguoiVPService.GetNguoiVP(_sqlContext, id);

            outputModel.ListToChucVP = this._toChucVPService.GetToChucVP(_sqlContext, id);

            outputModel.ListPhienDich = this._phienDichService.GetPhienDich(_sqlContext, id);

            outputModel.ListChungKien = this._chungKienService.GetNguoiChungKien(_sqlContext, id);

            outputModel.ListTangVat = this._tangVatService.GetTangVat(_sqlContext, id);

            outputModel.ListPhuongTien = this._phuongTienService.GetPhuongTien(_sqlContext, id);

            outputModel.ListGiayPhepChungChi = this._chungChiGiayPhep.GetChungChiGiayPhep(_sqlContext, id);

            return outputModel;
        }

        /// <summary>
        /// Xóa vụ việc
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteEntityAsync(string id, string userid, bool isSoftDelete = true)
        {
            var vuViecRemove = _sqlContext.VuViecVPHC.FirstOrDefault(i => i.Id.Equals(id));

            if (vuViecRemove == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                if (vuViecRemove != null)
                {
                    if (isSoftDelete)
                        vuViecRemove.IsDelete = true;
                    else
                        _sqlContext.VuViecVPHC.Remove(vuViecRemove);
                }
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xuất danh sách vụ việc
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExportFileAsync(VuViecSearchModel searchModel, string pathTemplate, NTSConstants.OptionExport optionExport)
        {
            searchModel.NoPagination = true;
            SearchBaseResultModel<VuViecSearchResultModel> searchResult = SearchAsync(searchModel).Result;

            VuViecExportModel vuViecExportModel = new VuViecExportModel();
            vuViecExportModel.NgayXuat = DateTime.Now.ToStringDDMMYY();
            vuViecExportModel.Data = searchResult.DataResults.Select(a => new ItemExportModel()
            {
                Index = a.Index,
                MaHoSo = a.MaHoSo,

                DonVi = a.DonVi,

                ThoiGianTiepNhan = a.ThoiGianTiepNhan.ToStringDDMMYY(),

                DiaChiDayDu = a.DiaChiDayDu,

                TongNguoiVP = a.TongNguoiVP,

                TongToChucVP = a.TongToChucVP,

                TongTienPhat = a.TongTienPhat,
                XuLy = a.XuLy,

                TienTrinhHoSo = a.TienTrinhHoSo
            }).ToList();

            MemoryStream streamFile = null;
            if (optionExport == NTSConstants.OptionExport.Excel)
            {
                streamFile = _excelService.ExportExcel<VuViecExportModel>(pathTemplate, vuViecExportModel);
            }
            else if (optionExport == NTSConstants.OptionExport.Pdf)
            {
                streamFile = _excelService.ExportExcelConvertToPdf<VuViecExportModel>(pathTemplate, vuViecExportModel);
            }

            return streamFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task FinishAsync(string id, string userId)
        {
            var vuViecUpdate = _sqlContext.VuViecVPHC.FirstOrDefault(i => i.Id.Equals(id));

            if (vuViecUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            vuViecUpdate.UpdateBy = userId;
            vuViecUpdate.UpdateDate = DateTime.Now;
            vuViecUpdate.TienTrinhHoSo = 4;

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Thống kê nhanh web
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<List<TreeItem>> QuickDashboard(VuViecSearchModel searchModel)
        {
            List<TreeItem> quickTreeView = new List<TreeItem>();
            TreeItem treeItem;
            TreeItem treeSubItem;
            try
            {
                var dataQuery = MakeWhereCondition(searchModel).ToList();

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số người vi phạm";
                treeItem.Expanded = true;
                treeItem.Total = dataQuery.Sum(s => s.TongNguoiVP);

                treeItem.Child = new List<TreeItem>();
                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Chưa kết luận";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Có hành vi vi phạm";
                treeSubItem.Total = dataQuery.Sum(s => s.TongNguoiVP);
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Không có hành vi vi phạm";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Theo độ tuổi";
                treeItem.HideTotal = true;
                treeItem.Expanded = true;

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nam dưới 16 tuổi";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nam từ đủ 16 đến dưới 18 tuổi";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nam từ đủ 18 tuổi trở lên";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nữ dưới 16 tuổi";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nữ từ đủ 16 đến dưới 18 tuổi";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tổng số người vi phạm nữ từ đủ 18 tuổi trở lên";
                treeSubItem.Total = 0;
                treeItem.Child.Add(treeSubItem);
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số tổ chức vi phạm";
                treeItem.Total = dataQuery.Sum(s => s.TongToChucVP);
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng trị giá tang vật tiêu huỷ";
                treeItem.Total = 0;
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng trị giá tang vật bán phát mại";
                treeItem.Total = 0;
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng tiền xử phạt VPHC";
                treeItem.Total = (decimal)dataQuery.Sum(a => a.TongTienPhat);
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số quyết định";
                treeItem.Total = dataQuery.Sum(s => s.TongToChucVP) + dataQuery.Sum(s => s.TongNguoiVP);
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số quyết định đã thi hành";
                treeItem.Total = dataQuery.Sum(s => s.TongNguoiVP);
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số quyết định hoãn, miễn, giảm";
                treeItem.Total = 0;
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số quyết định cưỡng chế thi hành";
                treeItem.Total = 0;
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số quyết định bị khiếu nại, khiếu kiện";
                treeItem.Total = 0;
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số vụ chuyển truy cứu trách nhiệm hình sự";
                treeItem.Total = dataQuery.Where(a => a.IdXuLy.Equals(NTSConstants.VPHC_XuLy_ChuyenDTHS)).Count();
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = $"Tổng số vụ áp dụng BP nhắc nhở người chưa thành niên";
                treeItem.Total = dataQuery.Where(a => a.IdXuLy.Equals(NTSConstants.VPHC_XuLy_ApDungNhacNho)).Count();
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = "Nguồn phát hiện";
                treeItem.HideTotal = true;
                treeItem.Expanded = true;
                var nguonTin = _sqlContext.NguonTinVPHC.AsNoTracking().ToList();
                foreach (var itemNguonTin in nguonTin)
                {
                    treeSubItem = new TreeItem();
                    treeSubItem.Text = $"{itemNguonTin.Name}";
                    treeSubItem.Total = dataQuery.Where(r => r.IdNguonPhatHien.Equals(itemNguonTin.Id)).Count();
                    treeItem.Child.Add(treeSubItem);
                }
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = "Phân loại";
                treeItem.HideTotal = true;
                treeItem.Expanded = true;
                quickTreeView.Add(treeItem);
                var listPhanLoaiTin = StatusHelper.GetListStatus(GroupsHelper.ThamQuyenXL);
                foreach (var item in listPhanLoaiTin)
                {
                    treeSubItem = new TreeItem();
                    treeSubItem.Text = $"{item.Name}";
                    treeSubItem.Total = dataQuery.Where(r => (int)item.Id == r.PhanLoai).Count();
                    treeItem.Child.Add(treeSubItem);
                }

                treeItem = new TreeItem();
                treeItem.Text = "Xử lý";
                treeItem.HideTotal = true;
                treeItem.Expanded = true;
                var xuLyVPHC = _sqlContext.XuLyVPHC.AsNoTracking().ToList();
                xuLyVPHC.Insert(0, new XuLyVPHC() { Id = "0", Name = "Chưa xử lý" });
                foreach (var itemXuLy in xuLyVPHC)
                {
                    treeSubItem = new TreeItem();
                    treeSubItem.Text = $"{itemXuLy.Name}";
                    treeSubItem.Total = dataQuery.Where(r => r.IdXuLy.Equals(itemXuLy.Id)).Count();
                    treeItem.Child.Add(treeSubItem);
                }
                quickTreeView.Add(treeItem);

                treeItem = new TreeItem();
                treeItem.Text = "Tổng theo tiến trình vụ việc";
                treeItem.HideTotal = true;
                treeItem.Expanded = true;

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Tiếp nhận, phát hiện";
                treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 0).Count();
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Lập biên bản";
                treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 1).Count();
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Kiểm tra, xác minh";
                treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 2).Count();
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Xử lý";
                treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 3).Count();
                treeItem.Child.Add(treeSubItem);

                treeSubItem = new TreeItem();
                treeSubItem.Text = $"Kết thúc";
                treeSubItem.Total = dataQuery.Where(r => r.TienTrinhHoSo == 4).Count();
                treeItem.Child.Add(treeSubItem);
                quickTreeView.Add(treeItem);

                string idDonViCapDuoiList = "";
                var donVis = _sqlContext.DonVi.AsNoTracking().ToList();

                //Thống kê tất cả các tỉnh
                if (string.IsNullOrEmpty(searchModel.IdTinhPhatHien) && string.IsNullOrEmpty(searchModel.IdDonVi))
                {
                    var provinces = _comboboxService.GetAllTinh(true);
                    treeItem = new TreeItem();
                    treeItem.HideTotal = true;
                    treeItem.Text = $"Bộ Đội Biên Phòng Tỉnh/Thành Phố";


                    foreach (var province in provinces)
                    {
                        treeSubItem = new TreeItem();
                        treeSubItem.Text = $"{province.Name}";
                        idDonViCapDuoiList = String.Join(";", this.GetDonViByIdTinhInWeb(donVis, (string)province.Id).Select(r => r.Id).ToList());
                        treeSubItem.Total = dataQuery.Where(a => idDonViCapDuoiList.Contains(a.IdDonVi)).Count();
                        treeItem.Child.Add(treeSubItem);
                    }
                    quickTreeView.Add(treeItem);
                }
                else
                {
                    List<ComboboxModel> listDonVi = new List<ComboboxModel>();
                    if (!string.IsNullOrEmpty(searchModel.IdDonVi))
                        listDonVi = this.GetDonViByIdDonViInWeb(donVis, searchModel.IdDonVi);
                    else
                        listDonVi = this.GetDonViByIdTinhInWeb(donVis, searchModel.IdTinhPhatHien);

                    if (listDonVi.Count > 1)
                    {
                        treeItem = new TreeItem();
                        treeItem.HideTotal = true;
                        treeItem.Text = $"Đơn vị";
                        foreach (var itemDonVi in listDonVi)
                        {
                            treeSubItem = new TreeItem();
                            treeSubItem.Text = $"{itemDonVi.Name.Replace("--", "")}";
                            //Toàn cục
                            if ("100000".Equals(itemDonVi.Id))
                            {
                                treeSubItem.Total = dataQuery.Count();
                            }
                            //Cấp tỉnh/TP
                            else if ("200000".Equals(itemDonVi.Id))
                            {
                                var listDonViTinhTP = this.GetDonViByIdDonViInWeb(donVis, itemDonVi.Id);
                                TreeItem treeSubItemTinhTP;
                                foreach (var itemTinhTP in listDonViTinhTP)
                                {
                                    if (!itemTinhTP.Id.Equals("200000"))
                                    {
                                        treeSubItemTinhTP = new TreeItem();
                                        treeSubItemTinhTP.Text = $"{itemTinhTP.Name.Replace("-- Bộ Chỉ huy ", "").Replace(" -- ", "")}";
                                        idDonViCapDuoiList = String.Join(";", this.GetDonViByIdDonViInWeb(donVis, itemTinhTP.Id).Select(r => r.Id).ToList());
                                        treeSubItemTinhTP.Total = dataQuery.Where(a => idDonViCapDuoiList.Contains(a.IdDonVi)).Count();
                                        treeSubItem.Total += treeSubItemTinhTP.Total;
                                        treeSubItem.Child.Add(treeSubItemTinhTP);
                                    }
                                }
                                treeItem.Child.Add(treeSubItem);
                                continue;
                            }
                            //Bộ chỉ huy
                            else if ("200000".Equals(itemDonVi.IdParent))
                            {
                                treeSubItem.Text = itemDonVi.Name.Replace("Bộ Chỉ huy ", "");

                                idDonViCapDuoiList = String.Join(";", this.GetDonViByIdDonViInWeb(donVis, itemDonVi.Id).Select(r => r.Id).ToList());
                                treeSubItem.Total = dataQuery.Where(a => idDonViCapDuoiList.Contains(a.IdDonVi)).Count();
                            }
                            else
                            {
                                treeSubItem.Text = itemDonVi.Name.Replace("--", "");
                                treeSubItem.Total = dataQuery.Where(a => itemDonVi.Id.Equals(a.IdDonVi)).Count();
                            }
                            treeItem.Child.Add(treeSubItem);
                        }
                        quickTreeView.Add(treeItem);

                    }
                }
            }
            catch { }
            return quickTreeView;
        }

        public VuViecTrongBienBanModel GetVuViecTrongBienBan(string id)
        {
            var vuViec = _sqlContext.VuViecVPHC.Where(r => r.Id.Equals(id)).FirstOrDefault();

            if (vuViec == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            VuViecTrongBienBanModel model = new VuViecTrongBienBanModel();
            model.Id = id;
            model.DiaChiDayDu = vuViec.DiaChiDayDu;

            if (!string.IsNullOrEmpty(vuViec.CanBoDieuTra))
            {
                model.ListCanBo = JsonConvert.DeserializeObject<List<CanBoChooseModel>>(vuViec.CanBoDieuTra);
            }

            model.ListNguoiVP = this._nguoiVPService.GetNguoiVP(_sqlContext, id);

            model.ListToChucVP = this._toChucVPService.GetToChucVP(_sqlContext, id);

            model.ListPhienDich = this._phienDichService.GetPhienDich(_sqlContext, id);

            model.ListChungKien = this._chungKienService.GetNguoiChungKien(_sqlContext, id);

            return model;
        }

        public VuViecTrongQuyetDinhModel GetVuViecTrongQuyetDinh(string id)
        {
            var vuViec = _sqlContext.VuViecVPHC.Where(r => r.Id.Equals(id)).FirstOrDefault();

            if (vuViec == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            VuViecTrongQuyetDinhModel model = new VuViecTrongQuyetDinhModel();
            model.Id = id;
            model.ListNguoiVP = this._nguoiVPService.GetNguoiVP(_sqlContext, id);

            model.ListToChucVP = this._toChucVPService.GetToChucVP(_sqlContext, id);
            return model;
        }

        public bool CheckViewNotify(string idViPham, string idDonVi)
        {
            bool view = false;

            string idDonViCapDuoiList = String.Join(";", _comboboxService.GetDonViByIdDonVi(idDonVi).Result.Select(r => r.Id).ToList());
            var vuViec = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(idViPham)).FirstOrDefault();
            var donVi = _sqlContext.DonVi.Where(s => s.IdDonVi.Equals(idDonVi)).FirstOrDefault();
            if (vuViec != null && donVi != null)
            {
                //Trường hợp vi phạm của đơn vị cấp dưới
                if (vuViec.IdDonVi != idDonVi && idDonViCapDuoiList.Contains(vuViec.IdDonVi))
                {
                    view = true;
                }
                //Cấp cụ, cấp phòng của cục
                else if (vuViec.IdDonVi != idDonVi && ("100000".Equals(idDonVi) || (donVi.Level == 2 && "DV01".Equals(donVi.IdLoaiDonVi))))
                {

                }
            }

            return view;
        }

        #region -------------------------Các hàm nội bộ----------------------------------
        private List<ComboboxModel> GetDonViByIdTinhInWeb(List<DonVi> listDonVi, string idTinh)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {

                var data = listDonVi.Where(a => !string.IsNullOrEmpty(a.IdTinh) && a.IdTinh.Equals(idTinh) && a.IdLoaiDonVi.Equals("DV03") && a.Level == 2).OrderBy(r => r.Order).Select(s => new ComboboxModel()
                {
                    Id = s.IdDonVi,
                    Name = s.Ten,
                    IdParent = s.IdDonViCha,
                    Level = s.Level
                }).ToList();

                List<ComboboxModel> listChild;
                foreach (var item in data)
                {
                    listChild = listDonVi.Where(i => !string.IsNullOrEmpty(i.IdDonViCha) && i.IdDonViCha.Equals(item.Id))
                    .Select(s => new ComboboxModel()
                    {
                        Id = s.IdDonVi,
                        Name = s.Ten,
                        IdParent = s.IdDonViCha,
                        Level = s.Level
                    }).ToList();
                    listCombobox.Add(item);
                    listCombobox.AddRange(GetListChild(item.Id, listChild, string.Empty));
                }
            }
            catch { }
            return listCombobox;
        }

        private List<ComboboxModel> GetListChild(string parentId, List<ComboboxModel> listGroupDocument, string convert)
        {
            List<ComboboxModel> listChild = new List<ComboboxModel>();
            var listChilds = listGroupDocument.Where(r => r.IdParent.Equals(parentId)).ToList();
            string convertchild = $"{convert} --";
            foreach (var item in listChilds)
            {
                item.IdParent = parentId;
                item.Name = convertchild + " " + item.Name;
                listChild.Add(item);
                listChild.AddRange(GetListChild(item.Id, listGroupDocument, convertchild));
            }

            return listChild;
        }

        private List<ComboboxModel> GetDonViByIdDonViInWeb(List<DonVi> listDonVi, string idDonVi)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                var data = listDonVi.Where(a => a.IdDonVi.Equals(idDonVi)).Select(s => new ComboboxModel()
                {
                    Id = s.IdDonVi,
                    Name = s.Ten,
                    IdParent = s.IdDonViCha,
                    Level = s.Level
                }).ToList();

                List<ComboboxModel> listChild;
                foreach (var item in data)
                {
                    listChild = listDonVi.Where(i => !string.IsNullOrEmpty(i.IdDonViCha) && i.IdDonViCha.Equals(item.Id))
                    .Select(s => new ComboboxModel()
                    {
                        Id = s.IdDonVi,
                        Name = s.Ten,
                        IdParent = s.IdDonViCha,
                        Level = s.Level
                    }).ToList();
                    listCombobox.Add(item);
                    listCombobox.AddRange(GetListChild(item.Id, listChild, string.Empty));
                }
            }
            catch { }
            return listCombobox;
        }

        private string GhepDiaChi(string? idTinh, string? idHuyen, string? idXa, string? diaChi)
        {
            string diaChiDayDu = "";
            if (!string.IsNullOrEmpty(diaChi))
            {
                diaChiDayDu += diaChi;
            }
            if (!string.IsNullOrEmpty(idXa))
            {
                var xa = _sqlContext.Ward.FirstOrDefault(s => s.Id.Equals(idXa));
                diaChiDayDu += xa != null ? (string.IsNullOrEmpty(diaChiDayDu) ? xa.Name : $" - {xa.Name}") : "";
            }
            if (!string.IsNullOrEmpty(idHuyen))
            {
                var huyen = _sqlContext.District.FirstOrDefault(s => s.Id.Equals(idHuyen));
                diaChiDayDu += huyen != null ? (string.IsNullOrEmpty(diaChiDayDu) ? huyen.Name : $" - {huyen.Name}") : "";
            }
            if (!string.IsNullOrEmpty(idTinh))
            {
                var tinh = _sqlContext.Province.FirstOrDefault(s => s.Id.Equals(idTinh));
                diaChiDayDu += tinh != null ? (string.IsNullOrEmpty(diaChiDayDu) ? tinh.Name : $" - {tinh.Name}") : "";
            }
            return diaChiDayDu;
        }
        #endregion
    }
}
