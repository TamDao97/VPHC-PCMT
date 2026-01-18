using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using NTS_ERP.Models.VPHC.NguoiChungKien;
using System.Drawing;
using TrafficControl.Core;
using NTS.Common.Utils;

namespace NTS_ERP.Services.VPHC.NguoiChungKien
{
    public class NguoiChungKienService : INguoiChungKienService
    {

        private readonly NTS_ERPContext _sqlContext;

        public NguoiChungKienService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm chứng kiến
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<NguoiChungKienResultModel>> Search(NguoiChungKienSearchModel searchModel)
        {
            SearchBaseResultModel<NguoiChungKienResultModel> searchResult = new SearchBaseResultModel<NguoiChungKienResultModel>();
            var dataQuery = (from a in _sqlContext.NguoiChungKien.AsNoTracking()
                             orderby a.HoVaTen
                             select new
                             {
                                 a.IdNguoiChungKien,
                                 a.HoVaTen,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Ten))
            {
                dataQuery = dataQuery.Where(u => u.HoVaTen.ToUpper().Contains(searchModel.Ten.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new NguoiChungKienResultModel()
            {
                Ten = s.HoVaTen
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm chứng kiến
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(NguoiChungKienModifyModel model, string userId)
        {
            model.IdNguoiChungKien = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiChungKien>(JsonConvert.SerializeObject(model));

            _sqlContext.NguoiChungKien.Add(toChucVPEntity);

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
        public async Task<NguoiChungKienModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.NguoiChungKien.FirstOrDefault(u => u.IdNguoiChungKien.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            NguoiChungKienModifyModel outputModel = JsonConvert.DeserializeObject<NguoiChungKienModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update chứng kiến
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, NguoiChungKienModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.NguoiChungKien.FirstOrDefault(i => i.IdNguoiChungKien.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiChungKien>(JsonConvert.SerializeObject(model));
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
        /// Xóa chứng kiến
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.NguoiChungKien.FirstOrDefault(u => u.IdNguoiChungKien.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.NguoiChungKien.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm chứng kiến
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<NguoiChungKienModifyModel> models, string idVuViec, string userId)
        {
            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.NguoiChungKien.FirstOrDefault(i => i.IdNguoiChungKien.Equals(model.IdNguoiChungKien));
                Models.Entities.NguoiChungKien toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdNguoiChungKien = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiChungKien>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;

                    sqlContext.NguoiChungKien.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.NguoiChungKien>(JsonConvert.SerializeObject(model));
                }
            }

            //Thực hiện xóa người chứng kiến
            var chungKiens = sqlContext.NguoiChungKien.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && chungKiens.Count > 0)
            {
                sqlContext.NguoiChungKien.RemoveRange(chungKiens);
            }
            else if (chungKiens.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = chungKiens.Where(s => !models.Select(s => s.IdNguoiChungKien).Contains(s.IdNguoiChungKien));
                sqlContext.NguoiChungKien.RemoveRange(toChucVPRemove);
            }
        }

        public List<NguoiChungKienModifyModel> GetNguoiChungKien(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<NguoiChungKienModifyModel> listNguoiVP = new List<NguoiChungKienModifyModel>();

            listNguoiVP = (from a in sqlContext.NguoiChungKien.AsNoTracking()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new NguoiChungKienModifyModel()
                           {
                               IdNguoiChungKien = a.IdNguoiChungKien,
                               IdViPhamHC = a.IdViPhamHC,
                               HoVaTen = a.HoVaTen,
                               GioiTinh = a.GioiTinh,
                               TenGioiTinh = a.GioiTinh == 1 ? "Nam" : a.GioiTinh == 2 ? "Nữ" : "Khác",
                               NgaySinh = a.NgaySinh,
                               Cmnd = a.CMND,
                               DiaChi = a.DiaChi,
                               GhiChu = a.GhiChu,
                               SoDienThoai = a.SoDienThoai,
                               NgayCap = a.NgayCap,
                               NoiCap = a.NoiCap,
                               NgheNghiep = a.NgheNghiep,
                               TruongHop = a.TruongHop

                           }).OrderBy(o => o.HoVaTen).ToList().UpdateIndex();
            return listNguoiVP;
        }
    }
}
