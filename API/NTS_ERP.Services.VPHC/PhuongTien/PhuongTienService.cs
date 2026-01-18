using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.PhuongTien;
using NTS_ERP.Models.VPHC.PhuongTien;
using System.Drawing;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.PhuongTien
{
    public class PhuongTienService : IPhuongTienService
    {

        private readonly NTS_ERPContext _sqlContext;

        public PhuongTienService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm phương tiện
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<PhuongTienResultModel>> Search(PhuongTienSearchModel searchModel)
        {
            SearchBaseResultModel<PhuongTienResultModel> searchResult = new SearchBaseResultModel<PhuongTienResultModel>();
            var dataQuery = (from a in _sqlContext.PhuongTienVPHC.AsNoTracking()
                             join b in _sqlContext.LoaiPhuongTien.AsNoTracking() on a.IdLoaiPhuongTien equals b.Id
                             orderby b.Name
                             select new
                             {
                                 a.IdPhuongTienVPHC,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new PhuongTienResultModel()
            {
                Name = s.Name
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm phương tiện
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(PhuongTienModifyModel model, string userId)
        {
            model.IdPhuongTienVPHC = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhuongTienVPHC>(JsonConvert.SerializeObject(model));

            await _sqlContext.PhuongTienVPHC.AddAsync(toChucVPEntity);

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
        public async Task<PhuongTienModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.PhuongTienVPHC.FirstOrDefault(u => u.IdPhuongTienVPHC.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            PhuongTienModifyModel outputModel = JsonConvert.DeserializeObject<PhuongTienModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update phương tiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, PhuongTienModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.PhuongTienVPHC.FirstOrDefault(i => i.IdPhuongTienVPHC.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhuongTienVPHC>(JsonConvert.SerializeObject(model));
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
        /// Xóa phương tiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.PhuongTienVPHC.FirstOrDefault(u => u.IdPhuongTienVPHC.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.PhuongTienVPHC.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm phương tiện
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<PhuongTienModifyModel> models, string idVuViec, string userId)
        {
            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.PhuongTienVPHC.FirstOrDefault(i => i.IdPhuongTienVPHC.Equals(model.IdPhuongTienVPHC));
                Models.Entities.PhuongTienVPHC toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdPhuongTienVPHC = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhuongTienVPHC>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;

                    sqlContext.PhuongTienVPHC.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhuongTienVPHC>(JsonConvert.SerializeObject(model));
                    sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);
                }
            }

            //Thực hiện xóa phương tiện
            var phuongTiens = sqlContext.PhuongTienVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && phuongTiens.Count > 0)
            {
                sqlContext.PhuongTienVPHC.RemoveRange(phuongTiens);
            }
            else if (phuongTiens.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = phuongTiens.Where(s => !models.Select(s => s.IdPhuongTienVPHC).Contains(s.IdPhuongTienVPHC));
                sqlContext.PhuongTienVPHC.RemoveRange(toChucVPRemove);
            }
        }

        /// <summary>
        /// Xử lý phương tiện
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddXuLy(NTS_ERPContext sqlContext, List<PhuongTienModifyModel> models, string idVuViec, string userId)
        {
            var phuongTienVPHCList = sqlContext.PhuongTienVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec));
            foreach (var model in models)
            {
                var phuongTienUpdate = phuongTienVPHCList.FirstOrDefault(i => i.IdPhuongTienVPHC.Equals(model.IdPhuongTienVPHC));
                if (phuongTienUpdate != null)
                {
                    phuongTienUpdate.XuLy = model.XuLy;
                    phuongTienUpdate.GhiChu = model.GhiChu;
                }
            }
        }

        public List<PhuongTienModifyModel> GetPhuongTien(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<PhuongTienModifyModel> listNguoiVP = new List<PhuongTienModifyModel>();

            listNguoiVP = (from a in sqlContext.PhuongTienVPHC.AsNoTracking()
                           join b in sqlContext.LoaiPhuongTien.AsNoTracking() on a.IdLoaiPhuongTien equals b.Id into ab
                           from b in ab.DefaultIfEmpty()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new PhuongTienModifyModel()
                           {
                               IdPhuongTienVPHC = a.IdPhuongTienVPHC,
                               IdViPhamHC = a.IdViPhamHC,
                               TenLoaiPhuongTien = b.Name,
                               IdLoaiPhuongTien = a.IdLoaiPhuongTien,
                               BienSo = a.BienSo,
                               TiepNhan = a.TiepNhan,
                               BoSung = a.BoSung,
                               GhiChu = a.GhiChu,
                               XuLy = a.XuLy,
                               NhanHieu = a.NhanHieu,
                               XuatXu = a.XuatXu,
                               TinhTrangDacDiem = a.TinhTrangDacDiem,
                               Name = a.Name

                           }).OrderBy(o => o.TenLoaiPhuongTien).ToList().UpdateIndex();
            return listNguoiVP;
        }
    }
}
