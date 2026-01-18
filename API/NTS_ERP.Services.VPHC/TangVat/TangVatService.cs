using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.TangVat;
using NTS_ERP.Models.VPHC.TangVat;
using System.Drawing;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.TangVat
{
    public class TangVatService : ITangVatService
    {

        private readonly NTS_ERPContext _sqlContext;

        public TangVatService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm tang vật
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<TangVatResultModel>> Search(TangVatSearchModel searchModel)
        {
            SearchBaseResultModel<TangVatResultModel> searchResult = new SearchBaseResultModel<TangVatResultModel>();
            var dataQuery = (from a in _sqlContext.TangVatVPHC.AsNoTracking()
                             join b in _sqlContext.LoaiTangVat.AsNoTracking() on a.IdLoaiTangVat equals b.Id
                             orderby b.Name
                             select new
                             {
                                 a.IdTangVatVPHC,
                                 a.Name,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Ten))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Ten.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new TangVatResultModel()
            {
                Name = s.Name
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm tang vật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(TangVatModifyModel model, string userId)
        {
            model.IdTangVatVPHC = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.TangVatVPHC>(JsonConvert.SerializeObject(model));

            await _sqlContext.TangVatVPHC.AddAsync(toChucVPEntity);

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
        public async Task<TangVatModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.TangVatVPHC.FirstOrDefault(u => u.IdTangVatVPHC.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            TangVatModifyModel outputModel = JsonConvert.DeserializeObject<TangVatModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update tang vật
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, TangVatModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.TangVatVPHC.FirstOrDefault(i => i.IdTangVatVPHC.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.TangVatVPHC>(JsonConvert.SerializeObject(model));
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
        /// Xóa tang vật
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.TangVatVPHC.FirstOrDefault(u => u.IdTangVatVPHC.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.TangVatVPHC.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm tang vật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<TangVatModifyModel> models, string idVuViec, string userId)
        {
            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.TangVatVPHC.FirstOrDefault(i => i.IdTangVatVPHC.Equals(model.IdTangVatVPHC));
                Models.Entities.TangVatVPHC toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdTangVatVPHC = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.TangVatVPHC>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;

                    sqlContext.TangVatVPHC.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.TangVatVPHC>(JsonConvert.SerializeObject(model));
                    sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);
                }
            }

            //Thực hiện xóa tang vật
            var tangVats = sqlContext.TangVatVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && tangVats.Count > 0)
            {
                sqlContext.TangVatVPHC.RemoveRange(tangVats);
            }
            else if (tangVats.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = tangVats.Where(s => !models.Select(s => s.IdTangVatVPHC).Contains(s.IdTangVatVPHC));
                sqlContext.TangVatVPHC.RemoveRange(toChucVPRemove);
            }
        }

        /// <summary>
        /// Xử lý tang vật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddXuLy(NTS_ERPContext sqlContext, List<TangVatModifyModel> models, string idVuViec, string userId)
        {
            var tangVatVPHCList = sqlContext.TangVatVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec));
            foreach (var model in models)
            {
                var tangVatUpdate = tangVatVPHCList.FirstOrDefault(i => i.IdTangVatVPHC.Equals(model.IdTangVatVPHC));
                if (tangVatUpdate != null)
                {
                    tangVatUpdate.XuLy = model.XuLy;
                    tangVatUpdate.GhiChu = model.GhiChu;
                }
            }
        }

        public List<TangVatModifyModel> GetTangVat(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<TangVatModifyModel> listNguoiVP = new List<TangVatModifyModel>();

            listNguoiVP = (from a in sqlContext.TangVatVPHC.AsNoTracking()
                           join b in sqlContext.LoaiTangVat.AsNoTracking() on a.IdLoaiTangVat equals b.Id into ab
                           from b in ab.DefaultIfEmpty()
                           join c in sqlContext.DonViTinh.AsNoTracking() on a.IdDonViTinh equals c.IdDonViTinh into ac
                           from c in ac.DefaultIfEmpty()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new TangVatModifyModel()
                           {
                               IdTangVatVPHC = a.IdTangVatVPHC,
                               IdViPhamHC = a.IdViPhamHC,
                               TenLoaiTangVat = b.Name,
                               IdLoaiTangVat = a.IdLoaiTangVat,
                               IdDonViTinh = a.IdDonViTinh,
                               TenDonViTinh = c.Ten,
                               SoLuong = a.SoLuong,
                               TiepNhan = a.TiepNhan,
                               BoSung = a.BoSung,
                               GhiChu = a.GhiChu,
                               IdDonViTinhThuc = a.IdDonViTinhThuc,
                               SoLuongThuc = a.SoLuongThuc,
                               XuLy = a.XuLy,
                               Name = a.Name,
                               ChungLoai = a.ChungLoai,
                               TinhTrangDacDiem = a.TinhTrangDacDiem
                           }).OrderBy(o => o.TenLoaiTangVat).ToList().UpdateIndex();
            return listNguoiVP;
        }
    }
}
