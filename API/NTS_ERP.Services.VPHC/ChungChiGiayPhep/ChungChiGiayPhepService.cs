using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.ChungChiGiayPhep;
using NTS_ERP.Models.VPHC.ChungChiGiayPhep;
using System.Drawing;
using TrafficControl.Core;
using NTS.Common.Utils;

namespace NTS_ERP.Services.VPHC.ChungChiGiayPhep
{
    public class ChungChiGiayPhepService : IChungChiGiayPhepService
    {

        private readonly NTS_ERPContext _sqlContext;

        public ChungChiGiayPhepService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm chứng chỉ, giấy phép
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<ChungChiGiayPhepResultModel>> Search(ChungChiGiayPhepSearchModel searchModel)
        {
            SearchBaseResultModel<ChungChiGiayPhepResultModel> searchResult = new SearchBaseResultModel<ChungChiGiayPhepResultModel>();
            var dataQuery = (from a in _sqlContext.ChungChiGiayPhep.AsNoTracking()
                             orderby a.Ten
                             select new
                             {
                                 a.IdChungChiGiayPhep,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new ChungChiGiayPhepResultModel()
            {
                Ten = s.Ten
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm chứng chỉ, giấy phép
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(ChungChiGiayPhepModifyModel model, string userId)
        {
            model.IdChungChiGiayPhep = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ChungChiGiayPhep>(JsonConvert.SerializeObject(model));

            await _sqlContext.ChungChiGiayPhep.AddAsync(toChucVPEntity);

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
        public async Task<ChungChiGiayPhepModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.ChungChiGiayPhep.FirstOrDefault(u => u.IdChungChiGiayPhep.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            ChungChiGiayPhepModifyModel outputModel = JsonConvert.DeserializeObject<ChungChiGiayPhepModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update chứng chỉ, giấy phép
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, ChungChiGiayPhepModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.ChungChiGiayPhep.FirstOrDefault(i => i.IdChungChiGiayPhep.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ChungChiGiayPhep>(JsonConvert.SerializeObject(model));
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
        /// Xóa chứng chỉ, giấy phép
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.ChungChiGiayPhep.FirstOrDefault(u => u.IdChungChiGiayPhep.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.ChungChiGiayPhep.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm chứng chỉ, giấy phép
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<ChungChiGiayPhepModifyModel> models, string idVuViec, string userId)
        {
            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.ChungChiGiayPhep.FirstOrDefault(i => i.IdChungChiGiayPhep.Equals(model.IdChungChiGiayPhep));
                Models.Entities.ChungChiGiayPhep toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdChungChiGiayPhep = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ChungChiGiayPhep>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;

                    sqlContext.ChungChiGiayPhep.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.ChungChiGiayPhep>(JsonConvert.SerializeObject(model));
                    sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);
                }
            }

            //Thực hiện xóa chứng chỉ
            var chungChis = sqlContext.ChungChiGiayPhep.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && chungChis.Count > 0)
            {
                sqlContext.ChungChiGiayPhep.RemoveRange(chungChis);
            }
            else if (chungChis.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = chungChis.Where(s => !models.Select(s => s.IdChungChiGiayPhep).Contains(s.IdChungChiGiayPhep));
                sqlContext.ChungChiGiayPhep.RemoveRange(toChucVPRemove);
            }
        }

        /// <summary>
        /// Xử lý chứng chỉ, giấy phép
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddXuLy(NTS_ERPContext sqlContext, List<ChungChiGiayPhepModifyModel> models, string idVuViec, string userId)
        {
            var chungChiGiayPhepList = sqlContext.ChungChiGiayPhep.Where(i => i.IdViPhamHC.Equals(idVuViec));
            foreach (var model in models)
            {
                var chungChiUpdate = chungChiGiayPhepList.FirstOrDefault(i => i.IdChungChiGiayPhep.Equals(model.IdChungChiGiayPhep));
                if (chungChiUpdate != null)
                {
                    chungChiUpdate.XuLy = model.XuLy;
                    chungChiUpdate.GhiChu = model.GhiChu;
                }
            }
        }

        public List<ChungChiGiayPhepModifyModel> GetChungChiGiayPhep(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<ChungChiGiayPhepModifyModel> listNguoiVP = new List<ChungChiGiayPhepModifyModel>();

            listNguoiVP = (from a in sqlContext.ChungChiGiayPhep.AsNoTracking()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new ChungChiGiayPhepModifyModel()
                           {
                               IdChungChiGiayPhep = a.IdChungChiGiayPhep,
                               IdViPhamHC = a.IdViPhamHC,
                               Ten = a.Ten,
                               SoLuong = a.SoLuong,
                               GhiChu = a.GhiChu,
                               XuLy = a.XuLy,
                               TinhTrangDacDiem = a.TinhTrangDacDiem
                           }).OrderBy(o => o.Ten).ToList().UpdateIndex();
            return listNguoiVP;
        }
    }
}
