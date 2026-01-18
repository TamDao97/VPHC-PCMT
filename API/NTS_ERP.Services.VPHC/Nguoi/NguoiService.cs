using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.Nguoi;
using NTS_ERP.Models.VPHC.NguoiVP;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.Nguoi
{
    public class NguoiService : INguoiService
    {

        private readonly NTS_ERPContext _sqlContext;

        public NguoiService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm tổ chức vi phạm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<NguoiResultModel>> Search(NguoiSearchModel searchModel)
        {
            SearchBaseResultModel<NguoiResultModel> searchResult = new SearchBaseResultModel<NguoiResultModel>();
            var dataQuery = (from a in _sqlContext.Nguoi.AsNoTracking()
                             orderby a.HoVaTen
                             select new
                             {
                                 a.IdNguoi,
                                 a.HoVaTen,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new NguoiResultModel()
            {
                HoVaTen = s.HoVaTen
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm nhóm tổ chức vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(NguoiModifyModel model, string userId)
        {
            model.IdNguoi = Guid.NewGuid().ToString();
            var nguoiEntity = JsonConvert.DeserializeObject<Models.Entities.Nguoi>(JsonConvert.SerializeObject(model));
            nguoiEntity.CreateBy = userId;
            nguoiEntity.CreateDate = DateTime.Now;
            nguoiEntity.UpdateBy = userId;
            nguoiEntity.UpdateDate = DateTime.Now;

            _sqlContext.Nguoi.Add(nguoiEntity);

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
        public async Task<NguoiModifyModel> GetById(string id)
        {
            var nguoiModel = _sqlContext.Nguoi.FirstOrDefault(u => u.IdNguoi.Equals(id));

            if (nguoiModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            NguoiModifyModel outputModel = JsonConvert.DeserializeObject<NguoiModifyModel>(JsonConvert.SerializeObject(nguoiModel, Formatting.None,
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
        public async Task Update(string id, NguoiModifyModel model, string userId)
        {
            var nguoiUpdate = _sqlContext.Nguoi.FirstOrDefault(i => i.IdNguoi.Equals(id));

            if (nguoiUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var nguoiEntity = JsonConvert.DeserializeObject<Models.Entities.Nguoi>(JsonConvert.SerializeObject(model));
            nguoiEntity.UpdateBy = userId;
            nguoiEntity.UpdateDate = DateTime.Now;
            _sqlContext.Entry(nguoiUpdate).CurrentValues.SetValues(nguoiEntity);

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
            var nguoiEntity = _sqlContext.Nguoi.FirstOrDefault(u => u.IdNguoi.Equals(id));
            if (nguoiEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.Nguoi.Remove(nguoiEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm tổ chức vi phạm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> Add(NTS_ERPContext sqlContext, NguoiModifyModel model, CurrentUserModel currentUser)
        {
            var nguoiUpdate = sqlContext.Nguoi.FirstOrDefault(i => i.IdNguoi.Equals(model.IdNguoi));
            Models.Entities.Nguoi nguoiEntity;
            if (nguoiUpdate == null)
            {
                model.IdNguoi = Guid.NewGuid().ToString();
                nguoiEntity = JsonConvert.DeserializeObject<Models.Entities.Nguoi>(JsonConvert.SerializeObject(model));
                nguoiEntity.DiaChiDayDu = this.GhepDiaChi(nguoiEntity.IdTinh, nguoiEntity.IdHuyen, nguoiEntity.IdXa);
                nguoiEntity.DiaChiHienNayDayDu = this.GhepDiaChi(nguoiEntity.IdTinhHienNay, nguoiEntity.IdHuyenHienNay, nguoiEntity.IdXaHienNay, nguoiEntity.DiaChi);
                nguoiEntity.IdDonVi = currentUser.DonViId;
                nguoiEntity.CreateBy = currentUser.UserId;
                nguoiEntity.CreateDate = DateTime.Now;
                nguoiEntity.UpdateBy = currentUser.UserId;
                nguoiEntity.UpdateDate = DateTime.Now;
                sqlContext.Nguoi.Add(nguoiEntity);
            }
            else
            {
                nguoiEntity = JsonConvert.DeserializeObject<Models.Entities.Nguoi>(JsonConvert.SerializeObject(model));
                nguoiEntity.DiaChiDayDu = this.GhepDiaChi(nguoiEntity.IdTinh, nguoiEntity.IdHuyen, nguoiEntity.IdXa);
                nguoiEntity.DiaChiHienNayDayDu = this.GhepDiaChi(nguoiEntity.IdTinhHienNay, nguoiEntity.IdHuyenHienNay, nguoiEntity.IdXaHienNay, nguoiEntity.DiaChi);
                nguoiEntity.UpdateBy = currentUser.UserId;
                nguoiEntity.UpdateDate = DateTime.Now;
                sqlContext.Entry(nguoiUpdate).CurrentValues.SetValues(nguoiEntity);
            }
            return model?.IdNguoi ?? "";
        }

        private string GhepDiaChi(string idTinh, string idHuyen, string idXa, string diaChi = "")
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
    }

}
