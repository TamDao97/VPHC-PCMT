using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.ToChucVP;
using NTS_ERP.Models.VPHC.ToChucVP;
using System.Drawing;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.ToChucVP
{
    public class ToChucVPService : IToChucVPService
    {

        private readonly NTS_ERPContext _sqlContext;

        public ToChucVPService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm tổ chức vi phạm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<ToChucVPResultModel>> Search(ToChucVPSearchModel searchModel)
        {
            SearchBaseResultModel<ToChucVPResultModel> searchResult = new SearchBaseResultModel<ToChucVPResultModel>();
            var dataQuery = (from a in _sqlContext.ToChucVP.AsNoTracking()
                             orderby a.Ten
                             select new
                             {
                                 a.IdToChucVP,
                                 a.Ten,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Ten))
            {
                dataQuery = dataQuery.Where(u => u.Ten.ToUpper().Contains(searchModel.Ten.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new ToChucVPResultModel()
            {
                Ten = s.Ten
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm tổ chức vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(ToChucVPModifyModel model, string userId)
        {
            model.IdToChucVP = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ToChucVP>(JsonConvert.SerializeObject(model));
            toChucVPEntity.CreateBy = userId;
            toChucVPEntity.CreateDate = DateTime.Now;
            toChucVPEntity.UpdateBy = userId;
            toChucVPEntity.UpdateDate = DateTime.Now;

            _sqlContext.ToChucVP.Add(toChucVPEntity);

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
        public async Task<ToChucVPModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.ToChucVP.FirstOrDefault(u => u.IdToChucVP.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            ToChucVPModifyModel outputModel = JsonConvert.DeserializeObject<ToChucVPModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update tổ chức vi phạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, ToChucVPModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.ToChucVP.FirstOrDefault(i => i.IdToChucVP.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ToChucVP>(JsonConvert.SerializeObject(model));
            toChucVPEntity.UpdateBy = userId;
            toChucVPEntity.UpdateDate = DateTime.Now;
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
        /// Xóa tổ chức vi phạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.ToChucVP.FirstOrDefault(u => u.IdToChucVP.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.ToChucVP.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm tổ chức vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<ToChucVPModifyModel>? models, string idVuViec, string userId)
        {
            if (models == null || models.Count == 0)
                return;

            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.ToChucVP.FirstOrDefault(i => i.IdToChucVP.Equals(model.IdToChucVP));
                Models.Entities.ToChucVP toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdToChucVP = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ToChucVP>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;
                    toChucVPEntity.CreateBy = userId;
                    toChucVPEntity.CreateDate = DateTime.Now;
                    toChucVPEntity.UpdateBy = userId;
                    toChucVPEntity.UpdateDate = DateTime.Now;

                    sqlContext.ToChucVP.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ToChucVP>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.UpdateBy = userId;
                    toChucVPEntity.UpdateDate = DateTime.Now;
                    sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);
                }
            }

            //Thực hiện xóa các các tổ chức
            var toChucVPs = sqlContext.ToChucVP.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && toChucVPs.Count > 0)
            {
                sqlContext.ToChucVP.RemoveRange(toChucVPs);
            }
            else if (toChucVPs.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = toChucVPs.Where(s => !models.Select(s => s.IdToChucVP).Contains(s.IdToChucVP));
                sqlContext.ToChucVP.RemoveRange(toChucVPRemove);
            }
        }

        /// <summary>
        /// Thêm xác minh cho tổ chức vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddXacMinh(NTS_ERPContext sqlContext, List<ToChucVPModifyModel> models, string idVuViec, string userId)
        {
            List<ToChucVPModifyModel> listAddNew = new List<ToChucVPModifyModel>();
            foreach (var model in models)
            {
                var nguoiVPUpdate = sqlContext.ToChucVP.FirstOrDefault(i => i.IdToChucVP.Equals(model.IdToChucVP));
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

            //Thực hiện xóa các tổ chức
            var toChucVPs = sqlContext.ToChucVP.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && toChucVPs.Count > 0)
            {
                sqlContext.ToChucVP.RemoveRange(toChucVPs);
            }
            else if (toChucVPs.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = toChucVPs.Where(s => !models.Select(s => s.IdToChucVP).Contains(s.IdToChucVP));
                sqlContext.ToChucVP.RemoveRange(toChucVPRemove);
            }

            if (listAddNew.Count > 0)
            {
                await this.Add(sqlContext, listAddNew, idVuViec, userId);
            }
        }

        public List<ToChucVPModifyModel> GetToChucVP(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<ToChucVPModifyModel> listNguoiVP = new List<ToChucVPModifyModel>();

            listNguoiVP = (from a in sqlContext.ToChucVP.AsNoTracking()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new ToChucVPModifyModel()
                           {
                               IdToChucVP = a.IdToChucVP,
                               IdViPhamHC = a.IdViPhamHC,
                               Ten = a.Ten,
                               DiaChiTruSo = a.DiaChiTruSo,
                               MaSoDoanhNghiep = a.MaSoDoanhNghiep,
                               SoDKKD = a.SoDKKD,
                               NgayCapDKKD = a.NgayCapDKKD,
                               NoiCapDKKD = a.NoiCapDKKD,
                               HoTenPhapNhan = a.HoTenPhapNhan,
                               GioiTinh = a.GioiTinh,
                               TenGioiTinh = a.GioiTinh == 1 ? "Nam" : a.GioiTinh == 2 ? "Nữ" : "Khác",
                               ChucVu = a.ChucVu,
                               HanhViViPham = a.HanhViViPham,
                               KetLuanKiemTra = a.KetLuanKiemTra
                           }).OrderBy(o => o.Ten).ToList().UpdateIndex();

            if (listNguoiVP.Count > 0)
            {
                List<string> idNguoiVPHC = listNguoiVP.Select(s => s.IdToChucVP).ToList();
                var bienBan = _sqlContext.BienBanVPHC.Where(s => idNguoiVPHC.Contains(s.IdToChucViPham)).ToList();
                BienBanVPHC? bienBanVPHC;
                foreach (var item in listNguoiVP)
                {
                    bienBanVPHC = bienBan.FirstOrDefault(f => item.IdToChucVP.Equals(f.IdToChucViPham));
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
