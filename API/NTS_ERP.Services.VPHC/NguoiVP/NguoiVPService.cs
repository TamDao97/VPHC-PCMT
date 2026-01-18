using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.Nguoi;
using System.Collections.Generic;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.NguoiVP
{
    public class NguoiVPService : INguoiVPService
    {

        private readonly NTS_ERPContext _sqlContext;
        private readonly INguoiService _nguoiService;

        public NguoiVPService(NTS_ERPContext sqlContext, INguoiService nguoiService)
        {
            this._sqlContext = sqlContext;
            this._nguoiService = nguoiService;
        }

        /// <summary>
        /// Tìm kiếm người vi phạm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<NguoiVPResultModel>> Search(NguoiVPSearchModel searchModel)
        {
            SearchBaseResultModel<NguoiVPResultModel> searchResult = new SearchBaseResultModel<NguoiVPResultModel>();
            var dataQuery = (from a in _sqlContext.NguoiVPHC.AsNoTracking()
                             join b in _sqlContext.Nguoi.AsNoTracking() on a.IdNguoi equals b.IdNguoi
                             orderby b.HoVaTen
                             select new
                             {
                                 a.IdNguoiVPHC,
                                 b.HoVaTen,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.HoVaTen))
            {
                dataQuery = dataQuery.Where(u => u.HoVaTen.ToUpper().Contains(searchModel.HoVaTen.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new NguoiVPResultModel()
            {
                HoVaTen = s.HoVaTen
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm người vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(NguoiVPModifyModel model, string userId)
        {
            model.IdNguoiVPHC = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiVPHC>(JsonConvert.SerializeObject(model));

            _sqlContext.NguoiVPHC.Add(toChucVPEntity);

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
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<NguoiVPModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.NguoiVPHC.AsNoTracking().FirstOrDefault(u => u.IdNguoiVPHC.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            NguoiVPModifyModel outputModel = JsonConvert.DeserializeObject<NguoiVPModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            return outputModel;
        }

        /// <summary>
        /// Update người vi phạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, NguoiVPModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.NguoiVPHC.FirstOrDefault(i => i.IdNguoiVPHC.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiVPHC>(JsonConvert.SerializeObject(model));
            _sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);

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
        /// Xóa người vi phạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.NguoiVPHC.FirstOrDefault(u => u.IdNguoiVPHC.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.NguoiVPHC.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }


        /// <summary>
        /// Thêm người vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<NguoiVPModifyModel>? models, string idVuViec, CurrentUserModel currentUser)
        {
            if (models == null || models.Count == 0)
                return;

            foreach (var model in models)
            {
                var nguoiVPUpdate = sqlContext.NguoiVPHC.FirstOrDefault(i => i.IdNguoiVPHC.Equals(model.IdNguoiVPHC));
                NguoiVPHC nguoiVPEntity;
                if (nguoiVPUpdate == null)
                {
                    model.IdNguoiVPHC = Guid.NewGuid().ToString();
                    nguoiVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiVPHC>(JsonConvert.SerializeObject(model));
                    nguoiVPEntity.IdViPhamHC = idVuViec;

                    string idNguoi = await _nguoiService.Add(sqlContext, model, currentUser);

                    nguoiVPEntity.IdNguoi = idNguoi;

                    sqlContext.NguoiVPHC.Add(nguoiVPEntity);
                }
                else
                {
                    nguoiVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiVPHC>(JsonConvert.SerializeObject(model));
                    await _nguoiService.Add(sqlContext, model, currentUser);
                    sqlContext.Entry(nguoiVPUpdate).CurrentValues.SetValues(nguoiVPEntity);
                }
            }

            //Thực hiện xóa các người vp
            var nguoiVPs = sqlContext.NguoiVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && nguoiVPs.Count > 0)
            {
                sqlContext.NguoiVPHC.RemoveRange(nguoiVPs);
            }
            else if (nguoiVPs.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = nguoiVPs.Where(s => !models.Select(s => s.IdNguoiVPHC).Contains(s.IdNguoiVPHC));
                sqlContext.NguoiVPHC.RemoveRange(toChucVPRemove);
            }
        }

        /// <summary>
        /// Thêm xác minh cho người vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddXacMinh(NTS_ERPContext sqlContext, List<NguoiVPModifyModel> models, string idVuViec, CurrentUserModel currentUser)
        {
            List<NguoiVPModifyModel> listAddNew = new List<NguoiVPModifyModel>();
            foreach (var model in models)
            {
                var nguoiVPUpdate = sqlContext.NguoiVPHC.FirstOrDefault(i => i.IdNguoiVPHC.Equals(model.IdNguoiVPHC));
                if (nguoiVPUpdate == null)
                {
                    listAddNew.Add(model);
                }
                else
                {
                    nguoiVPUpdate.KetLuanKiemTra = model.KetLuanKiemTra;
                    nguoiVPUpdate.HanhViViPham = model.HanhViViPham;
                }
            }

            //Thực hiện xóa các người vp
            var nguoiVPs = sqlContext.NguoiVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && nguoiVPs.Count > 0)
            {
                sqlContext.NguoiVPHC.RemoveRange(nguoiVPs);
            }
            else if (nguoiVPs.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = nguoiVPs.Where(s => !models.Select(s => s.IdNguoiVPHC).Contains(s.IdNguoiVPHC));
                sqlContext.NguoiVPHC.RemoveRange(toChucVPRemove);
            }

            if (listAddNew.Count > 0)
            {
                await this.Add(sqlContext, listAddNew, idVuViec, currentUser);
            }
        }

        public List<NguoiVPModifyModel> GetNguoiVP(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<NguoiVPModifyModel> listNguoiVP = new List<NguoiVPModifyModel>();

            listNguoiVP = (from a in sqlContext.NguoiVPHC.AsNoTracking()
                           where a.IdViPhamHC.Equals(idVuViec)
                           join b in sqlContext.Nguoi.AsNoTracking() on a.IdNguoi equals b.IdNguoi
                           join c in sqlContext.DanToc.AsNoTracking() on b.IdDanToc equals c.Id into bc
                           from c in bc.DefaultIfEmpty()
                           join d in sqlContext.QuocGia.AsNoTracking() on b.IdQuocTich equals d.Id into bd
                           from d in bd.DefaultIfEmpty()
                           join e in sqlContext.NgheNghiep.AsNoTracking() on b.IdNgheNghiep equals e.Id into be
                           from e in be.DefaultIfEmpty()
                           join f in sqlContext.TonGiao.AsNoTracking() on b.IdTonGiao equals f.Id into bf
                           from f in bf.DefaultIfEmpty()
                           select new NguoiVPModifyModel()
                           {
                               IdNguoiVPHC = a.IdNguoiVPHC,
                               IdViPhamHC = a.IdViPhamHC,
                               IdNguoi = b.IdNguoi,
                               IdDonVi = b.IdDonVi,
                               HoVaTen = b.HoVaTen,
                               NgaySinh = b.NgaySinh,
                               GioiTinh = b.GioiTinh,
                               TenGioiTinh = b.GioiTinh == 1 ? "Nam" : b.GioiTinh == 2 ? "Nữ" : "Khác",
                               IdQuocTich = b.IdQuocTich,
                               TenQuocTich = d.Name,
                               IdDanToc = b.IdDanToc,
                               TenDanToc = c.Name,
                               IdTonGiao = b.IdTonGiao,
                               TenTonGiao = f.Name,
                               IdNgheNghiep = b.IdNgheNghiep,
                               TenNgheNghiep = e.Name,
                               TrinhDoVanHoa = b.TrinhDoVanHoa,
                               SoDienThoai = b.SoDienThoai,
                               Cmnd = b.CMND,
                               NgayCap = b.NgayCap,
                               NoiCap = b.NoiCap,
                               IdTinh = b.IdTinh,
                               IdHuyen = b.IdHuyen,
                               IdXa = b.IdXa,
                               DiaChiDayDu = b.DiaChiDayDu,
                               IdTinhHienNay = b.IdTinhHienNay,
                               IdHuyenHienNay = b.IdHuyenHienNay,
                               IdXaHienNay = b.IdXaHienNay,
                               DiaChi = b.DiaChi,
                               DiaChiHienNayDayDu = b.DiaChiHienNayDayDu,
                               GhiChu = a.GhiChu,
                               Tuoi = a.Tuoi,
                               HoanCanhKhoKhan = a.HoanCanhKhoKhan,
                               ChiTietHoanCanh = a.ChiTietHoanCanh,
                               HanhViViPham = a.HanhViViPham,
                               KetLuanKiemTra = a.KetLuanKiemTra,
                               QuanHeGH = a.QuanHeGH,
                               HoVaTenGH = a.HoVaTenGH,
                               DiaChiGH = a.DiaChiGH
                           }).OrderBy(o => o.HoVaTen).ToList().UpdateIndex();
            if (listNguoiVP.Count > 0)
            {
                List<string> idNguoiVPHC = listNguoiVP.Select(s => s.IdNguoiVPHC).ToList();
                var bienBan = _sqlContext.BienBanVPHC.Where(s => idNguoiVPHC.Contains(s.IdNguoiViPham)).ToList();
                BienBanVPHC? bienBanVPHC;
                foreach (var item in listNguoiVP)
                {
                    bienBanVPHC = bienBan.FirstOrDefault(f => item.IdNguoiVPHC.Equals(f.IdNguoiViPham));
                    if (bienBanVPHC != null)
                    {
                        item.HanhViViPham = string.IsNullOrEmpty(item.HanhViViPham) ? bienBanVPHC.HanhViViPham : item.HanhViViPham;
                        item.QuyDinhTai = bienBanVPHC.QuyDinhTai;
                    }
                }
            }
            return listNguoiVP;
        }
    }
}
