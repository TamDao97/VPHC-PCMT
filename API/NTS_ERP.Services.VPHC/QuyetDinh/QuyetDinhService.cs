using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS.Document.Word;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.User;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.QuyetDinh;
using TrafficControl.Core;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace NTS_ERP.Services.VPHC.QuyetDinh
{
    public class QuyetDinhService : IQuyetDinhService
    {

        private readonly NTS_ERPContext _sqlContext;
        private readonly IWordService _wordService;

        public QuyetDinhService(NTS_ERPContext sqlContext, IWordService wordService)
        {
            this._sqlContext = sqlContext;
            this._wordService = wordService;
        }

        /// <summary>
        /// Tìm kiếm biên bản
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<QuyetDinhResultModel>> Search(QuyetDinhSearchModel searchModel)
        {
            SearchBaseResultModel<QuyetDinhResultModel> searchResult = new SearchBaseResultModel<QuyetDinhResultModel>();
            var dataQuery = (from a in _sqlContext.QuyetDinhXuPhat.AsNoTracking()
                             join b in _sqlContext.DanhMucQuyetDinh.AsNoTracking() on a.IdDanhMucQuyetDinh equals b.Id
                             orderby a.NgayRaQD
                             select new
                             {
                                 a.IdQuyetDinh,
                                 b.Name,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.HoVaTen))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.HoVaTen.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new QuyetDinhResultModel()
            {
                HoVaTen = s.Name
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm biên bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(QuyetDinhModifyModel model, string userId)
        {
            var quyetDinhEntity = new Models.Entities.QuyetDinhXuPhat();
            quyetDinhEntity.IdQuyetDinh = Guid.NewGuid().ToString();
            quyetDinhEntity.IdDanhMucQuyetDinh = model.IdDanhMucQuyetDinh;
            quyetDinhEntity.IdVuViec = model.IdVuViec;
            quyetDinhEntity.So = model.So;
            quyetDinhEntity.NgayRaQD = model.NgayRaQD;
            quyetDinhEntity.CanCu = model.CanCu;
            quyetDinhEntity.DoiTuongViPham = model.DoiTuongViPham;
            quyetDinhEntity.IdNguoiViPham = model.IdNguoiViPham;
            quyetDinhEntity.IdToChucViPham = model.IdToChucViPham;
            quyetDinhEntity.HanhViViPham = model.HanhViViPham;
            quyetDinhEntity.QuyDinhTai = model.QuyDinhTai;
            quyetDinhEntity.TinhTietTangNang = model.TinhTietTangNang;
            quyetDinhEntity.TinhTietGiamNhe = model.TinhTietGiamNhe;
            quyetDinhEntity.PhatChinh = model.PhatChinh;
            quyetDinhEntity.CuThePC = model.CuThePC;
            quyetDinhEntity.MucPhat = model.MucPhat;
            quyetDinhEntity.MucPhatText = model.MucPhatText;
            quyetDinhEntity.PhatBoSung = model.PhatBoSung;
            quyetDinhEntity.CuThePBS = model.CuThePBS;
            quyetDinhEntity.SoNgayThucHienPBS = model.SoNgayThucHienPBS;
            quyetDinhEntity.KhacPhucHauQua = model.KhacPhucHauQua;
            quyetDinhEntity.CuTheKPHQ = model.CuTheKPHQ;
            quyetDinhEntity.SoNgayThucHienKPHQ = model.SoNgayThucHienKPHQ;
            quyetDinhEntity.NoiDungLienQuanKPHQ = model.NoiDungLienQuanKPHQ;
            quyetDinhEntity.ChiPhiKPHQ = model.ChiPhiKPHQ;
            quyetDinhEntity.ChiPhiKPHQText = model.ChiPhiKPHQText;
            quyetDinhEntity.CoQuanThucHienKPHC = model.CoQuanThucHienKPHC;
            quyetDinhEntity.NgayQDCoHieuLuc = model.NgayQDCoHieuLuc;
            quyetDinhEntity.HanNopPhat = model.HanNopPhat;
            quyetDinhEntity.DiaDiemNopPhat = model.DiaDiemNopPhat;
            quyetDinhEntity.DonViThuTienPhat = model.DonViThuTienPhat;
            quyetDinhEntity.DonViThucHien = model.DonViThucHien;
            quyetDinhEntity.DonViPhoiHop = model.DonViPhoiHop;

            _sqlContext.QuyetDinhXuPhat.Add(quyetDinhEntity);

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(model.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                var quyetDinhXuPhat = _sqlContext.QuyetDinhXuPhat.Where(s => s.IdVuViec.Equals(model.IdVuViec)).Select(s => new { s.IdDanhMucQuyetDinh, MucPhat = s.MucPhat ?? 0 });
                vuViecUpdate.TongQuyetDinh = quyetDinhXuPhat.Count() + 1;
                vuViecUpdate.TongQDXuPhat = quyetDinhXuPhat.Where(s => s.IdDanhMucQuyetDinh.Equals("1") || s.IdDanhMucQuyetDinh.Equals("2")).Count() + (quyetDinhEntity.IdDanhMucQuyetDinh.Equals("1") || quyetDinhEntity.IdDanhMucQuyetDinh.Equals("2") ? 1 : 0);
                vuViecUpdate.TongTienPhat = quyetDinhXuPhat.Sum(s => s.MucPhat) + (model.MucPhat ?? 0);
                if (vuViecUpdate.TienTrinhHoSo <= 2)
                {
                    vuViecUpdate.TienTrinhHoSo = 3;
                }
            }

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
        /// Lấy biên bản theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QuyetDinhModifyModel> GetById(string id)
        {
            var quyetDinhModel = _sqlContext.QuyetDinhXuPhat.AsNoTracking().FirstOrDefault(u => u.IdQuyetDinh.Equals(id));

            if (quyetDinhModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            QuyetDinhModifyModel outputModel = JsonConvert.DeserializeObject<QuyetDinhModifyModel>(JsonConvert.SerializeObject(quyetDinhModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            return outputModel;
        }

        /// <summary>
        /// Update biên bản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, QuyetDinhModifyModel model, string userId)
        {
            var quyetDinhUpdate = _sqlContext.QuyetDinhXuPhat.FirstOrDefault(i => i.IdQuyetDinh.Equals(id));

            if (quyetDinhUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            quyetDinhUpdate.IdQuyetDinh = model.IdQuyetDinh;
            quyetDinhUpdate.IdDanhMucQuyetDinh = model.IdDanhMucQuyetDinh;
            quyetDinhUpdate.IdVuViec = model.IdVuViec;
            quyetDinhUpdate.So = model.So;
            quyetDinhUpdate.NgayRaQD = model.NgayRaQD;
            quyetDinhUpdate.CanCu = model.CanCu;
            quyetDinhUpdate.DoiTuongViPham = model.DoiTuongViPham;
            quyetDinhUpdate.IdNguoiViPham = model.IdNguoiViPham;
            quyetDinhUpdate.IdToChucViPham = model.IdToChucViPham;
            quyetDinhUpdate.HanhViViPham = model.HanhViViPham;
            quyetDinhUpdate.QuyDinhTai = model.QuyDinhTai;
            quyetDinhUpdate.TinhTietTangNang = model.TinhTietTangNang;
            quyetDinhUpdate.TinhTietGiamNhe = model.TinhTietGiamNhe;
            quyetDinhUpdate.PhatChinh = model.PhatChinh;
            quyetDinhUpdate.CuThePC = model.CuThePC;
            quyetDinhUpdate.MucPhat = model.MucPhat;
            quyetDinhUpdate.MucPhatText = model.MucPhatText;
            quyetDinhUpdate.PhatBoSung = model.PhatBoSung;
            quyetDinhUpdate.CuThePBS = model.CuThePBS;
            quyetDinhUpdate.SoNgayThucHienPBS = model.SoNgayThucHienPBS;
            quyetDinhUpdate.KhacPhucHauQua = model.KhacPhucHauQua;
            quyetDinhUpdate.CuTheKPHQ = model.CuTheKPHQ;
            quyetDinhUpdate.SoNgayThucHienKPHQ = model.SoNgayThucHienKPHQ;
            quyetDinhUpdate.NoiDungLienQuanKPHQ = model.NoiDungLienQuanKPHQ;
            quyetDinhUpdate.ChiPhiKPHQ = model.ChiPhiKPHQ;
            quyetDinhUpdate.ChiPhiKPHQText = model.ChiPhiKPHQText;
            quyetDinhUpdate.CoQuanThucHienKPHC = model.CoQuanThucHienKPHC;
            quyetDinhUpdate.NgayQDCoHieuLuc = model.NgayQDCoHieuLuc;
            quyetDinhUpdate.HanNopPhat = model.HanNopPhat;
            quyetDinhUpdate.DiaDiemNopPhat = model.DiaDiemNopPhat;
            quyetDinhUpdate.DonViThuTienPhat = model.DonViThuTienPhat;
            quyetDinhUpdate.DonViThucHien = model.DonViThucHien;
            quyetDinhUpdate.DonViPhoiHop = model.DonViPhoiHop;

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(model.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                var quyetDinhXuPhat = _sqlContext.QuyetDinhXuPhat.Where(s => s.IdVuViec.Equals(model.IdVuViec) && !s.IdQuyetDinh.Equals(id)).Select(s => s.MucPhat ?? 0);
                vuViecUpdate.TongTienPhat = quyetDinhXuPhat.Sum() + (model.MucPhat ?? 0);
                if (vuViecUpdate.TienTrinhHoSo < 3)
                {
                    vuViecUpdate.TienTrinhHoSo = 3;
                }
            }

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
        /// Xóa biên bản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var quyetDinhEntity = _sqlContext.QuyetDinhXuPhat.FirstOrDefault(u => u.IdQuyetDinh.Equals(id));
            if (quyetDinhEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.QuyetDinhXuPhat.Remove(quyetDinhEntity);

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(quyetDinhEntity.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                vuViecUpdate.TongQuyetDinh = _sqlContext.QuyetDinhXuPhat.Where(s => s.IdVuViec.Equals(quyetDinhEntity.IdVuViec)).Select(s => s.IdQuyetDinh).Count() - 1;
                vuViecUpdate.TongQDXuPhat = vuViecUpdate.TongQDXuPhat - (quyetDinhEntity.IdDanhMucQuyetDinh.Equals("1") || quyetDinhEntity.IdDanhMucQuyetDinh.Equals("2") ? 1 : 0);
                vuViecUpdate.TongTienPhat = vuViecUpdate.TongTienPhat - quyetDinhEntity.MucPhat ?? 0;
            }

            await _sqlContext.SaveChangesAsync();
        }

        public List<DanhMucQuyetDinhModle> GetDanhMuc()
        {
            List<DanhMucQuyetDinhModle> listDanhMuc = _sqlContext.DanhMucQuyetDinh.AsNoTracking().OrderBy(o => o.Order).Select(s => new DanhMucQuyetDinhModle()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            }).ToList();
            return listDanhMuc;
        }

        /// <summary>
        /// Lấy biên bản của vụ việc
        /// </summary>
        /// <param name="id">Id vụ việc</param>
        /// <returns></returns>
        public QuyetDinhVuViecResultModel GetQuyetDinhVuViec(string id)
        {
            QuyetDinhVuViecResultModel resultModel = new QuyetDinhVuViecResultModel();
            resultModel.DataResults = (from a in _sqlContext.QuyetDinhXuPhat.AsNoTracking().Where(u => u.IdVuViec.Equals(id))
                                       join b in _sqlContext.NguoiVPHC.AsNoTracking() on a.IdNguoiViPham equals b.IdNguoiVPHC into ab
                                       from b in ab.DefaultIfEmpty()
                                       join c in _sqlContext.ToChucVP.AsNoTracking() on a.IdToChucViPham equals c.IdToChucVP into ac
                                       from c in ac.DefaultIfEmpty()
                                       join d in _sqlContext.DanhMucQuyetDinh.AsNoTracking() on a.IdDanhMucQuyetDinh equals d.Id
                                       join e in _sqlContext.HinhThucPhat.AsNoTracking() on a.PhatChinh equals e.Id into ae
                                       from e in ae.DefaultIfEmpty()
                                       select new QuyetDinhVuViecModel()
                                       {
                                           IdQuyetDinh = a.IdQuyetDinh,
                                           So = a.So,
                                           TenQuyetDinh = d.Name,
                                           NgayRaQD = a.NgayRaQD,
                                           TenDoiTuongVP = b != null ? (_sqlContext.Nguoi.AsNoTracking().FirstOrDefault(s => s.IdNguoi.Equals(b.IdNguoi)).HoVaTen ?? "") : c.Ten ?? "",
                                           HanhViViPham = a.HanhViViPham,
                                           CanCu = a.CanCu,
                                           PhatChinh = e.Name ?? "",
                                           MucPhat = a.MucPhat ?? 0
                                       }).ToList().UpdateIndex();

            if (resultModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            resultModel.TotalItems = resultModel.DataResults.Count();
            resultModel.TongTienPhat = resultModel.DataResults.Sum(s => s.MucPhat ?? 0);
            return resultModel;
        }

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        public MemoryStream ExportFileAsync(string id, string pathTemplate, NTSConstants.OptionExport optionExport)
        {
            var quyetDinhModel = _sqlContext.QuyetDinhXuPhat.AsNoTracking().FirstOrDefault(u => u.IdQuyetDinh.Equals(id));
            if (quyetDinhModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            QuyetDinhXuatModel quyetDinhModifyModel = new QuyetDinhXuatModel();

            quyetDinhModifyModel.DonViCapTren = "BỘ ĐỘI BIÊN PHÒNG";
            quyetDinhModifyModel.DonVi = "BỘ CHỈ HUY BỘ ĐỘI BIÊN PHÒNG\r\nTỈNH THANH HÓA";
            quyetDinhModifyModel.TinhQD = "Thanh Hóa";

            quyetDinhModifyModel.So = quyetDinhModel.So;
            quyetDinhModifyModel.NgayQD = quyetDinhModel.NgayRaQD.ToStringDD();
            quyetDinhModifyModel.ThangQD = quyetDinhModel.NgayRaQD.ToStringMM();
            quyetDinhModifyModel.NamQD = quyetDinhModel.NgayRaQD.ToStringYYYY();
            quyetDinhModifyModel.ThamQuyenXuPhat = "CHỈ HUY TRƯỞNG BỘ ĐỘI BIÊN PHÒNG TỈNH THANH HÓA";
            quyetDinhModifyModel.CanCu = quyetDinhModel.CanCu;

            // Cá nhân hoặc Tổ chức
            quyetDinhModifyModel.DoiTuongVP = quyetDinhModel.DoiTuongViPham == 0 ? "Cá nhân" : "Tổ chức";
            if (quyetDinhModel.DoiTuongViPham == 0)
            {
                pathTemplate = NTSConstants.TemplateQuyetDinh_CaNhan_VPHC;
                var nguoiVP = (from a in _sqlContext.NguoiVPHC.AsNoTracking().Where(w => w.IdNguoiVPHC.Equals(quyetDinhModel.IdNguoiViPham))
                               join b in _sqlContext.Nguoi.AsNoTracking() on a.IdNguoi equals b.IdNguoi
                               join c in _sqlContext.QuocGia.AsNoTracking() on b.IdQuocTich equals c.Id into bc
                               from c in bc.DefaultIfEmpty()
                               join d in _sqlContext.NgheNghiep.AsNoTracking() on b.IdNgheNghiep equals d.Id into bd
                               from d in bd.DefaultIfEmpty()
                               select new
                               {
                                   b.HoVaTen,
                                   b.GioiTinh,
                                   b.NgaySinh,
                                   QuocTich = c.Name ?? "",
                                   b.DiaChiHienNayDayDu,
                                   b.CMND,
                                   b.NoiCap,
                                   b.NgayCap,
                                   NgheNghiep = d.Name ?? "",
                                   a.HoVaTenGH
                               }).FirstOrDefault();

                if (nguoiVP != null)
                {
                    quyetDinhModifyModel.TenNguoiVP = nguoiVP.HoVaTen ?? "";
                    // Giới tính vi phạm
                    quyetDinhModifyModel.GioiVP = nguoiVP.GioiTinh.ToStringGender() ?? "";
                    quyetDinhModifyModel.NgaySinhNguoiVP = nguoiVP.NgaySinh?.ToStringDDMMYY() ?? "";
                    // Quốc tịch người vi phạm
                    quyetDinhModifyModel.QTichVP = nguoiVP.QuocTich ?? "";
                    quyetDinhModifyModel.NgheNghiepNguoiVP = nguoiVP.NgheNghiep ?? "";
                    quyetDinhModifyModel.NoiONguoiVP = nguoiVP.DiaChiHienNayDayDu ?? "";
                    quyetDinhModifyModel.SoCCCDNguoiVP = nguoiVP.CMND ?? "";
                    quyetDinhModifyModel.NgayCapCCCDNguoiVP = nguoiVP.NgayCap?.ToStringDDMMYY() ?? "";
                    quyetDinhModifyModel.NoiCapCCCD = nguoiVP.NoiCap ?? "";

                    /// Ông hoặc bà
                    quyetDinhModifyModel.OngBa = nguoiVP.GioiTinh.ToStringGrandparents() ?? "";
                    /// Cá nhân hoặc Người đại diện của tổ chức
                    quyetDinhModifyModel.CaNhanDaiDien = "Cá nhân";

                    var bienBanModel = _sqlContext.BienBanVPHC.AsNoTracking().FirstOrDefault(u => u.IdVuViec.Equals(quyetDinhModel.IdVuViec) && u.IdNguoiViPham.Equals(quyetDinhModel.IdNguoiViPham));
                    if ((bienBanModel != null))
                    {
                        quyetDinhModifyModel.SoBB = bienBanModel.So;
                        quyetDinhModifyModel.NgayBB = bienBanModel.ThoiGianLap.ToStringDDMMYY();
                    }
                }
            }
            else
            {
                pathTemplate = NTSConstants.TemplateQuyetDinh_ToChuc_VPHC;
                var toChucVP = _sqlContext.ToChucVP.AsNoTracking().Where(w => w.IdViPhamHC.Equals(quyetDinhModel.IdToChucViPham)).Select(s => new
                {
                    s.Ten,
                    s.DiaChiTruSo,
                    s.MaSoDoanhNghiep,
                    s.SoDKKD,
                    s.NgayCapDKKD,
                    s.NoiCapDKKD,
                    s.HoTenPhapNhan,
                    s.GioiTinh,
                    s.ChucVu
                }).FirstOrDefault();
                if (toChucVP != null)
                {
                    quyetDinhModifyModel.TenToChucVP = toChucVP.Ten;
                    quyetDinhModifyModel.DiaChiTruSoToChucVP = toChucVP.DiaChiTruSo;
                    quyetDinhModifyModel.MaSoToChucVP = toChucVP.MaSoDoanhNghiep;
                    quyetDinhModifyModel.GiayPhepTCVP = toChucVP.SoDKKD;
                    quyetDinhModifyModel.NgayCapGiayPTCVP = toChucVP.NgayCapDKKD?.ToStringDDMMYY() ?? "";
                    quyetDinhModifyModel.NoiCapGiayPTCVP = toChucVP.NoiCapDKKD;
                    quyetDinhModifyModel.TenNguoiTCVP = toChucVP.HoTenPhapNhan;
                    quyetDinhModifyModel.GioiDD = toChucVP.GioiTinh?.ToStringGender() ?? "";
                    quyetDinhModifyModel.ChucDanhNguoiDaiDienTCVP = toChucVP.ChucVu;
                    /// Ông hoặc bà
                    quyetDinhModifyModel.OngBa = toChucVP.GioiTinh?.ToStringGrandparents() ?? "";
                    /// Cá nhân hoặc Người đại diện của tổ chức
                    quyetDinhModifyModel.CaNhanDaiDien = "Người đại diện của tổ chức";

                    var bienBanModel = _sqlContext.BienBanVPHC.AsNoTracking().FirstOrDefault(u => u.IdVuViec.Equals(quyetDinhModel.IdVuViec) && u.IdToChucViPham.Equals(quyetDinhModel.IdToChucViPham));
                    if ((bienBanModel != null))
                    {
                        quyetDinhModifyModel.SoBB = bienBanModel.So;
                        quyetDinhModifyModel.NgayBB = bienBanModel.ThoiGianLap.ToStringDDMMYY();
                    }
                }
            }

            quyetDinhModifyModel.HanhViViPham = quyetDinhModel.HanhViViPham;
            quyetDinhModifyModel.QuyDinhTai = quyetDinhModel.QuyDinhTai;


            quyetDinhModifyModel.TinhTietTangNang = quyetDinhModel.TinhTietTangNang;
            quyetDinhModifyModel.TinhTietGiamNhe = quyetDinhModel.TinhTietGiamNhe;
            quyetDinhModifyModel.PhatChinh = _sqlContext.HinhThucPhat.FirstOrDefault(s => s.Id.Equals(quyetDinhModel.PhatChinh))?.Name ?? "";
            if ("HTP001".Equals(quyetDinhModel.PhatChinh) || "HTP002".Equals(quyetDinhModel.PhatChinh))
            {
                quyetDinhModifyModel.CuThePC = quyetDinhModel.MucPhat?.NumberToCurrencyTextVN(true) + $" ({quyetDinhModel.MucPhatText})";
            }
            else
            {
                quyetDinhModifyModel.CuThePC = quyetDinhModel.CuThePC ?? "";
            }
            quyetDinhModifyModel.PhatBoSung = quyetDinhModel.PhatBoSung;
            quyetDinhModifyModel.CuThePBS = quyetDinhModel.CuThePBS ?? "";
            quyetDinhModifyModel.SoNgayThucHienPBS = quyetDinhModel.SoNgayThucHienPBS?.ToString() ?? "";
            quyetDinhModifyModel.KhacPhucHauQua = quyetDinhModel.KhacPhucHauQua ?? "";
            quyetDinhModifyModel.CuTheKPHQ = quyetDinhModel.CuTheKPHQ;
            quyetDinhModifyModel.SoNgayThucHienKPHQ = quyetDinhModel.SoNgayThucHienKPHQ?.ToString() ?? ".....";
            quyetDinhModifyModel.NoiDungLienQuanKPHQ = quyetDinhModel.NoiDungLienQuanKPHQ;
            quyetDinhModifyModel.ChiPhiKPHQ = quyetDinhModel.ChiPhiKPHQ?.NTSToStringCurrency() ?? "" + " đồng";
            quyetDinhModifyModel.ChiPhiKPHQText = quyetDinhModel.ChiPhiKPHQText;
            quyetDinhModifyModel.CoQuanThucHienKPHQ = quyetDinhModel.CoQuanThucHienKPHC;
            quyetDinhModifyModel.DiaDiemNopPhat = quyetDinhModel.DiaDiemNopPhat;
            quyetDinhModifyModel.HanNopPhat = quyetDinhModel.HanNopPhat?.ToString() ?? "...........";
            quyetDinhModifyModel.DonViThuTienPhat = quyetDinhModel.DonViThuTienPhat;
            quyetDinhModifyModel.DonViThucHien = quyetDinhModel.DonViThucHien;
            quyetDinhModifyModel.DonViPhoiHop = quyetDinhModel.DonViPhoiHop;
            quyetDinhModifyModel.ChucVuKy = "CHỈ HUY TRƯỞNG";

            MemoryStream streamFile = new MemoryStream();
            if (optionExport == NTSConstants.OptionExport.Word)
            {
                streamFile = this._wordService.ExportWord<QuyetDinhXuatModel>(pathTemplate, quyetDinhModifyModel);
            }
            else if (optionExport == NTSConstants.OptionExport.Pdf)
            {
                streamFile = this._wordService.ExportWordConvertToPdf<QuyetDinhXuatModel>(pathTemplate, quyetDinhModifyModel);
            }

            return streamFile;
        }
    }
}
