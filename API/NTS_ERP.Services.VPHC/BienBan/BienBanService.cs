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
using NTS_ERP.Models.VPHC.BienBan;
using TrafficControl.Core;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace NTS_ERP.Services.VPHC.BienBan
{
    public class BienBanService : IBienBanService
    {

        private readonly NTS_ERPContext _sqlContext;
        private readonly IWordService _wordService;

        public BienBanService(NTS_ERPContext sqlContext, IWordService wordService)
        {
            this._sqlContext = sqlContext;
            this._wordService = wordService;
        }

        /// <summary>
        /// Tìm kiếm biên bản
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<BienBanResultModel>> Search(BienBanSearchModel searchModel)
        {
            SearchBaseResultModel<BienBanResultModel> searchResult = new SearchBaseResultModel<BienBanResultModel>();
            var dataQuery = (from a in _sqlContext.BienBanVPHC.AsNoTracking()
                             join b in _sqlContext.DanhMucBienBan.AsNoTracking() on a.IdDanhMucBienBan equals b.Id
                             orderby a.ThoiGianLap
                             select new
                             {
                                 a.IdBienBan,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new BienBanResultModel()
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
        public async Task Create(BienBanModifyModel model, string userId)
        {
            var bienBanEntity = new Models.Entities.BienBanVPHC();
            bienBanEntity.IdBienBan = Guid.NewGuid().ToString();
            bienBanEntity.IdDanhMucBienBan = model.IdDanhMucBienBan;
            bienBanEntity.IdVuViec = model.IdVuViec;
            bienBanEntity.So = model.So;
            bienBanEntity.ThoiGianLap = model.ThoiGianLap;
            bienBanEntity.LinhVuc = model.LinhVuc;
            bienBanEntity.LapTai = model.LapTai;
            bienBanEntity.DiaDiemLap = model.DiaDiemLap;
            bienBanEntity.LyDoLapChoKhac = model.LyDoLapChoKhac;
            bienBanEntity.CanCu = model.CanCu;
            bienBanEntity.IdCanBoLap = model.IdCanBoLap;
            bienBanEntity.ChuTheChungKien = model.ChuTheChungKien;
            bienBanEntity.IdNguoiChungKien = model.IdNguoiChungKien;
            bienBanEntity.IdPhienDichVien = model.IdPhienDichVien;
            bienBanEntity.DoiTuongViPham = model.DoiTuongViPham;
            bienBanEntity.IdNguoiViPham = model.IdNguoiViPham;
            bienBanEntity.IdToChucViPham = model.IdToChucViPham;
            bienBanEntity.HanhViViPham = model.HanhViViPham;
            bienBanEntity.QuyDinhTai = model.QuyDinhTai;
            bienBanEntity.ThietHai = model.ThietHai;
            bienBanEntity.YKienViPham = model.YKienViPham;
            bienBanEntity.YKienNguoiChungKien = model.YKienNguoiChungKien;
            bienBanEntity.YKienBenThietHai = model.YKienBenThietHai;
            bienBanEntity.BienPhapNganChan = model.BienPhapNganChan;
            bienBanEntity.CoQuanGiaiTrinh = model.CoQuanGiaiTrinh;
            bienBanEntity.ThoiGianGiaiQuyet = model.ThoiGianGiaiQuyet;
            bienBanEntity.CoQuanGiaiQuyet = model.CoQuanGiaiQuyet;
            bienBanEntity.ThoiGianLapXong = model.ThoiGianLapXong;
            bienBanEntity.SoTo = model.SoTo;
            bienBanEntity.SoBanIn = model.SoBanIn;
            bienBanEntity.LyDoViPhamKhongKy = model.LyDoViPhamKhongKy;
            bienBanEntity.LyDoChungKienKhongKy = model.LyDoChungKienKhongKy;

            _sqlContext.BienBanVPHC.Add(bienBanEntity);

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(model.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                vuViecUpdate.TongBienBan = _sqlContext.BienBanVPHC.Where(s => s.IdVuViec.Equals(model.IdVuViec)).Select(s => s.IdBienBan).Count() + 1;
                if (vuViecUpdate.TienTrinhHoSo <= 0)
                {
                    vuViecUpdate.TienTrinhHoSo = 1;
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
        public async Task<BienBanModifyModel> GetById(string id)
        {
            var bienBanModel = _sqlContext.BienBanVPHC.AsNoTracking().FirstOrDefault(u => u.IdBienBan.Equals(id));

            if (bienBanModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            BienBanModifyModel outputModel = JsonConvert.DeserializeObject<BienBanModifyModel>(JsonConvert.SerializeObject(bienBanModel, Formatting.None,
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
        public async Task Update(string id, BienBanModifyModel model, string userId)
        {
            var bienBanUpdate = _sqlContext.BienBanVPHC.FirstOrDefault(i => i.IdBienBan.Equals(id));

            if (bienBanUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            bienBanUpdate.So = model.So;
            bienBanUpdate.ThoiGianLap = model.ThoiGianLap;
            bienBanUpdate.LinhVuc = model.LinhVuc;
            bienBanUpdate.LapTai = model.LapTai;
            bienBanUpdate.DiaDiemLap = model.DiaDiemLap;
            bienBanUpdate.LyDoLapChoKhac = model.LyDoLapChoKhac;
            bienBanUpdate.CanCu = model.CanCu;
            bienBanUpdate.IdCanBoLap = model.IdCanBoLap;
            bienBanUpdate.ChuTheChungKien = model.ChuTheChungKien;
            bienBanUpdate.IdNguoiChungKien = model.IdNguoiChungKien;
            bienBanUpdate.IdPhienDichVien = model.IdPhienDichVien;
            bienBanUpdate.DoiTuongViPham = model.DoiTuongViPham;
            bienBanUpdate.IdNguoiViPham = model.IdNguoiViPham;
            bienBanUpdate.IdToChucViPham = model.IdToChucViPham;
            bienBanUpdate.HanhViViPham = model.HanhViViPham;
            bienBanUpdate.QuyDinhTai = model.QuyDinhTai;
            bienBanUpdate.ThietHai = model.ThietHai;
            bienBanUpdate.YKienViPham = model.YKienViPham;
            bienBanUpdate.YKienNguoiChungKien = model.YKienNguoiChungKien;
            bienBanUpdate.YKienBenThietHai = model.YKienBenThietHai;
            bienBanUpdate.BienPhapNganChan = model.BienPhapNganChan;
            bienBanUpdate.CoQuanGiaiTrinh = model.CoQuanGiaiTrinh;
            bienBanUpdate.ThoiGianGiaiQuyet = model.ThoiGianGiaiQuyet;
            bienBanUpdate.CoQuanGiaiQuyet = model.CoQuanGiaiQuyet;
            bienBanUpdate.ThoiGianLapXong = model.ThoiGianLapXong;
            bienBanUpdate.SoTo = model.SoTo;
            bienBanUpdate.SoBanIn = model.SoBanIn;
            bienBanUpdate.LyDoViPhamKhongKy = model.LyDoViPhamKhongKy;
            bienBanUpdate.LyDoChungKienKhongKy = model.LyDoChungKienKhongKy;

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
            var bienBanEntity = _sqlContext.BienBanVPHC.FirstOrDefault(u => u.IdBienBan.Equals(id));
            if (bienBanEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.BienBanVPHC.Remove(bienBanEntity);

            var vuViecUpdateTotal = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(bienBanEntity.IdVuViec)).FirstOrDefault();
            if (vuViecUpdateTotal != null)
                vuViecUpdateTotal.TongBienBan = _sqlContext.BienBanVPHC.Where(s => s.IdVuViec.Equals(bienBanEntity.IdVuViec)).Select(s => s.IdBienBan).Count() - 1;

            await _sqlContext.SaveChangesAsync();
        }

        public List<DanhMucBienBanModle> GetDanhMuc()
        {
            List<DanhMucBienBanModle> listDanhMuc = _sqlContext.DanhMucBienBan.AsNoTracking().OrderBy(o => o.Order).Select(s => new DanhMucBienBanModle()
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
        public List<BienBanVuViecModel> GetBienBanVuViec(string id)
        {
            var bienBanModel = (from a in _sqlContext.BienBanVPHC.AsNoTracking().Where(u => u.IdVuViec.Equals(id))
                                join b in _sqlContext.NguoiVPHC.AsNoTracking() on a.IdNguoiViPham equals b.IdNguoiVPHC into ab
                                from b in ab.DefaultIfEmpty()
                                join c in _sqlContext.ToChucVP.AsNoTracking() on a.IdToChucViPham equals c.IdToChucVP into ac
                                from c in ac.DefaultIfEmpty()
                                join d in _sqlContext.DanhMucBienBan.AsNoTracking() on a.IdDanhMucBienBan equals d.Id
                                join e in _sqlContext.CanBo.AsNoTracking() on a.IdCanBoLap equals e.IdCanBo into ae
                                from e in ae.DefaultIfEmpty()
                                select new BienBanVuViecModel()
                                {
                                    IdBienBan = a.IdBienBan,
                                    So = a.So,
                                    TenBienBan = d.Name,
                                    ThoiGianLap = a.ThoiGianLap,
                                    TenDoiTuongVP = b != null ? (_sqlContext.Nguoi.AsNoTracking().FirstOrDefault(s => s.IdNguoi.Equals(b.IdNguoi)).HoVaTen ?? "") :c.Ten?? "",
                                    DonViLap = e != null ? _sqlContext.DonVi.AsNoTracking().FirstOrDefault(s => s.IdDonVi.Equals(e.IdDonVi)).Ten ?? "" : "",
                                    TenCanBoLap = e.HoVaTen ?? "",
                                    ThoiGianLapXong = a.ThoiGianLapXong
                                }).ToList().UpdateIndex(); ;

            if (bienBanModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            return bienBanModel;
        }

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        public MemoryStream ExportFileAsync(string id, string pathTemplate, NTSConstants.OptionExport optionExport)
        {
            var bienBanModel = _sqlContext.BienBanVPHC.AsNoTracking().FirstOrDefault(u => u.IdBienBan.Equals(id));
            if (bienBanModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            BienBanXuatModel bienBanModifyModel = new BienBanXuatModel();

            bienBanModifyModel.DonViCapTren = "BCH BỘ ĐỘI BIÊN PHÒNG\r\nTỈNH THANH HÓA";
            bienBanModifyModel.DonVi = "ĐỒN BIÊN PHÒNG ĐA LỘC";

            bienBanModifyModel.So = bienBanModel.So;
            bienBanModifyModel.GioLap = bienBanModel.ThoiGianLap.ToStringHH();
            bienBanModifyModel.PhutLap = bienBanModel.ThoiGianLap.ToStringmm();
            bienBanModifyModel.NgayLap = bienBanModel.ThoiGianLap.ToStringDDMMYY();
            bienBanModifyModel.LinhVuc = bienBanModel.LinhVuc;
            bienBanModifyModel.DiaDiemLap = bienBanModel.DiaDiemLap;
            bienBanModifyModel.LyDoLapChoKhac = bienBanModel.LyDoLapChoKhac;
            bienBanModifyModel.CanCu = bienBanModel.CanCu;

            var canBo = (from a in _sqlContext.CanBo.AsNoTracking().Where(w => w.IdCanBo.Equals(bienBanModel.IdCanBoLap))
                         join b in _sqlContext.DonVi.AsNoTracking() on a.IdDonVi equals b.IdDonVi into ab
                         from b in ab.DefaultIfEmpty()
                         join c in _sqlContext.ChucVu.AsNoTracking() on a.IdChucVu equals c.Id into ac
                         from c in ac.DefaultIfEmpty()
                         select new { a.HoVaTen, ChucVu = c.Name ?? "", DonVi = b.Ten ?? "" }
                        ).FirstOrDefault();
            if (canBo != null)
            {
                bienBanModifyModel.TenCanBoLap = canBo.HoVaTen;
                bienBanModifyModel.ChucVuCBL = canBo.ChucVu;
                bienBanModifyModel.DonViCBL = canBo.DonVi;
            }

            var chungKien = _sqlContext.NguoiChungKien.Where(w => w.IdNguoiChungKien.Equals(bienBanModel.IdNguoiChungKien)).FirstOrDefault();
            if (chungKien != null)
            {
                bienBanModifyModel.TenNguoiCK = chungKien.HoVaTen ?? "";
                bienBanModifyModel.NgheNguoiCK = chungKien.NgheNghiep ?? "";
                bienBanModifyModel.DiaChiNguoiCK = chungKien.DiaChi ?? "";
                // Ông/ba chứng kiến
                bienBanModifyModel.OngBaCK = chungKien.GioiTinh.ToStringGender();
                
                if(chungKien.TruongHop == 0)
                {
                    // Người chứng kiến/ đại diện chính quyền cấp xã
                    bienBanModifyModel.NguoiDaiDienCK =  "Người chứng kiến";
                    bienBanModifyModel.NgheChuc = "Nghề nghiệp";
                    bienBanModifyModel.DiaChiCoQuan = "Địa chỉ";
                }
                else
                {
                    // Người chứng kiến/ đại diện chính quyền cấp xã
                    bienBanModifyModel.NguoiDaiDienCK ="Đại diện chính quyền cấp xã";
                    bienBanModifyModel.NgheChuc = "Chức vụ";
                    bienBanModifyModel.DiaChiCoQuan = "Cơ quan";
                } 
            }

            var phienDich = _sqlContext.PhienDichVienVPHC.Where(w => w.IdPhienDichVienVPHC.Equals(bienBanModel.IdPhienDichVien)).FirstOrDefault();
            if (phienDich != null)
            {
                bienBanModifyModel.TenPhienDich = phienDich?.HoVaTen ?? "";
                bienBanModifyModel.NgheNghiepPD = phienDich?.NgheNghiep ?? "";
                bienBanModifyModel.DiaChiPD = phienDich?.DiaChi ?? "";
            }

            // Cá nhân hoặc Tổ chức
            bienBanModifyModel.DoiTuongVP = bienBanModel.DoiTuongViPham == 0 ? "Cá nhân" : "Tổ chức";
            if (bienBanModel.DoiTuongViPham == 0)
            {
                pathTemplate = NTSConstants.TemplateBienBan_CaNhan_VPHC;
                var nguoiVP = (from a in _sqlContext.NguoiVPHC.AsNoTracking().Where(w => w.IdNguoiVPHC.Equals(bienBanModel.IdNguoiViPham))
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
                    bienBanModifyModel.TenNguoiVP = nguoiVP.HoVaTen ?? "";
                    // Giới tính vi phạm
                    bienBanModifyModel.GioiVP = nguoiVP.GioiTinh.ToStringGender() ?? "";
                    bienBanModifyModel.NgaySinhNguoiVP = nguoiVP.NgaySinh?.ToStringDDMMYY() ?? "";
                    // Quốc tịch người vi phạm
                    bienBanModifyModel.QTichVP = nguoiVP.QuocTich ?? "";
                    bienBanModifyModel.NgheNghiepNguoiVP = nguoiVP.NgheNghiep ?? "";
                    bienBanModifyModel.NoiONguoiVP = nguoiVP.DiaChiHienNayDayDu ?? "";
                    bienBanModifyModel.SoCCCDNguoiVP = nguoiVP.CMND ?? "";
                    bienBanModifyModel.NgayCapCCCDNguoiVP = nguoiVP.NgayCap?.ToStringDDMMYY() ?? "";
                    bienBanModifyModel.NoiCapCCCD = nguoiVP.NoiCap ?? "";
                    bienBanModifyModel.TenNguoiGiamHo = nguoiVP.HoVaTenGH;

                    /// Ông hoặc bà
                    bienBanModifyModel.OngBa = nguoiVP.GioiTinh.ToStringGrandparents() ?? "";
                    /// Cá nhân hoặc Người đại diện của tổ chức
                    bienBanModifyModel.CaNhanDaiDien = "Cá nhân";
                }
            }
            else
            {
                pathTemplate = NTSConstants.TemplateBienBan_ToChuc_VPHC;
                var toChucVP = _sqlContext.ToChucVP.AsNoTracking().Where(w => w.IdViPhamHC.Equals(bienBanModel.IdToChucViPham)).Select(s => new
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
                    bienBanModifyModel.TenToChucVP = toChucVP.Ten;
                    bienBanModifyModel.DiaChiTruSoToChucVP = toChucVP.DiaChiTruSo;
                    bienBanModifyModel.MaSoToChucVP = toChucVP.MaSoDoanhNghiep;
                    bienBanModifyModel.GiayPhepTCVP = toChucVP.SoDKKD;
                    bienBanModifyModel.NgayCapGiayPTCVP = toChucVP.NgayCapDKKD?.ToStringDDMMYY() ?? "";
                    bienBanModifyModel.NoiCapGiayPTCVP = toChucVP.NoiCapDKKD;
                    bienBanModifyModel.TenNguoiTCVP = toChucVP.HoTenPhapNhan;
                    bienBanModifyModel.GioiDD = toChucVP.GioiTinh?.ToStringGender() ?? "";
                    bienBanModifyModel.ChucDanhNguoiDaiDienTCVP = toChucVP.ChucVu;
                    /// Ông hoặc bà
                    bienBanModifyModel.OngBa = toChucVP.GioiTinh?.ToStringGrandparents() ?? "";
                    /// Cá nhân hoặc Người đại diện của tổ chức
                    bienBanModifyModel.CaNhanDaiDien = "Người đại diện của tổ chức";
                }
            }

            bienBanModifyModel.HanhViViPham = bienBanModel.HanhViViPham;
            bienBanModifyModel.QuyDinhTai = bienBanModel.QuyDinhTai;
            bienBanModifyModel.YKienNguoiChungKien = bienBanModel.YKienNguoiChungKien;

            bienBanModifyModel.YKienBenThietHai = bienBanModel.YKienBenThietHai;
            bienBanModifyModel.ThietHai = bienBanModel.ThietHai;
            bienBanModifyModel.YKienViPham = bienBanModel.YKienViPham;
            bienBanModifyModel.BienPhapNganChan = bienBanModel.BienPhapNganChan;

            bienBanModifyModel.CoQuanGiaiTrinh = bienBanModel.CoQuanGiaiTrinh;
            bienBanModifyModel.GioGiaiQuyet = bienBanModel.ThoiGianGiaiQuyet?.ToStringHH() ?? "";
            bienBanModifyModel.PhutGiaiQuyet = bienBanModel.ThoiGianGiaiQuyet?.ToStringmm() ?? "";
            bienBanModifyModel.NgayGiaiQuyet = bienBanModel.ThoiGianGiaiQuyet?.ToStringDDMMYY() ?? "";
            bienBanModifyModel.CoQuanGiaiQuyet = bienBanModel.CoQuanGiaiQuyet;
            bienBanModifyModel.GioLapXong = bienBanModel.ThoiGianLapXong?.ToStringHH() ?? "";
            bienBanModifyModel.PhutLapXong = bienBanModel.ThoiGianLapXong?.ToStringmm() ?? "";
            bienBanModifyModel.NgayLapXong = bienBanModel.ThoiGianLapXong?.ToStringDDMMYY() ?? "";
            bienBanModifyModel.SoTo = bienBanModel.SoTo;
            bienBanModifyModel.SoBanIn = bienBanModel.SoBanIn;
            bienBanModifyModel.LyDoViPhamKhongKy = bienBanModel.LyDoViPhamKhongKy;
            bienBanModifyModel.LyDoChungKienKhongKy = bienBanModel.LyDoChungKienKhongKy;


            MemoryStream streamFile = new MemoryStream();
            if (optionExport == NTSConstants.OptionExport.Word)
            {
                streamFile = this._wordService.ExportWord<BienBanXuatModel>(pathTemplate, bienBanModifyModel);
            }
            else if (optionExport == NTSConstants.OptionExport.Pdf)
            {
                streamFile = this._wordService.ExportWordConvertToPdf<BienBanXuatModel>(pathTemplate, bienBanModifyModel);
            }

            return streamFile;
        }
    }
}
