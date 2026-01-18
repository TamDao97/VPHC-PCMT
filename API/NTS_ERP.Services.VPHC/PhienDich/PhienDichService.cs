using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.PhienDich;
using NTS_ERP.Models.VPHC.PhienDich;
using System.Drawing;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.PhienDich
{
    public class PhienDichService : IPhienDichService
    {

        private readonly NTS_ERPContext _sqlContext;

        public PhienDichService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm phiên dịch
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<PhienDichResultModel>> Search(PhienDichSearchModel searchModel)
        {
            SearchBaseResultModel<PhienDichResultModel> searchResult = new SearchBaseResultModel<PhienDichResultModel>();
            var dataQuery = (from a in _sqlContext.PhienDichVienVPHC.AsNoTracking()
                             orderby a.HoVaTen
                             select new
                             {
                                 a.IdPhienDichVienVPHC,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new PhienDichResultModel()
            {
                HoVaTen = s.HoVaTen
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm phiên dịch
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(PhienDichModifyModel model, string userId)
        {
            model.IdPhienDichVienVPHC = Guid.NewGuid().ToString();
            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhienDichVienVPHC>(JsonConvert.SerializeObject(model));

            _sqlContext.PhienDichVienVPHC.Add(toChucVPEntity);

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
        public async Task<PhienDichModifyModel> GetById(string id)
        {
            var toChucVPModel = _sqlContext.PhienDichVienVPHC.FirstOrDefault(u => u.IdPhienDichVienVPHC.Equals(id));

            if (toChucVPModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            PhienDichModifyModel outputModel = JsonConvert.DeserializeObject<PhienDichModifyModel>(JsonConvert.SerializeObject(toChucVPModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            return outputModel;
        }

        /// <summary>
        /// Update phiên dịch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Update(string id, PhienDichModifyModel model, string userId)
        {
            var toChucVPUpdate = _sqlContext.PhienDichVienVPHC.FirstOrDefault(i => i.IdPhienDichVienVPHC.Equals(id));

            if (toChucVPUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhienDichVienVPHC>(JsonConvert.SerializeObject(model));
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
        /// Xóa phiên dịch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var toChucVPEntity = _sqlContext.PhienDichVienVPHC.FirstOrDefault(u => u.IdPhienDichVienVPHC.Equals(id));
            if (toChucVPEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.PhienDichVienVPHC.Remove(toChucVPEntity);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm phiên dịch
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Add(NTS_ERPContext sqlContext, List<PhienDichModifyModel> models, string idVuViec, string userId)
        {
            foreach (var model in models)
            {
                var toChucVPUpdate = sqlContext.PhienDichVienVPHC.FirstOrDefault(i => i.IdPhienDichVienVPHC.Equals(model.IdPhienDichVienVPHC));
                Models.Entities.PhienDichVienVPHC toChucVPEntity;
                if (toChucVPUpdate == null)
                {
                    model.IdPhienDichVienVPHC = Guid.NewGuid().ToString();
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhienDichVienVPHC>(JsonConvert.SerializeObject(model));
                    toChucVPEntity.IdViPhamHC = idVuViec;

                    sqlContext.PhienDichVienVPHC.Add(toChucVPEntity);
                }
                else
                {
                    toChucVPEntity = JsonConvert.DeserializeObject<Models.Entities.PhienDichVienVPHC>(JsonConvert.SerializeObject(model));
                    sqlContext.Entry(toChucVPUpdate).CurrentValues.SetValues(toChucVPEntity);
                }
            }

            //Thực hiện xóa phiên dịch
            var phienDichs = sqlContext.PhienDichVienVPHC.Where(i => i.IdViPhamHC.Equals(idVuViec)).ToList();
            if ((models == null || models.Count == 0) && phienDichs.Count > 0)
            {
                sqlContext.PhienDichVienVPHC.RemoveRange(phienDichs);
            }
            else if (phienDichs.Count > 0 && models?.Count > 0)
            {
                var toChucVPRemove = phienDichs.Where(s => !models.Select(s => s.IdPhienDichVienVPHC).Contains(s.IdPhienDichVienVPHC));
                sqlContext.PhienDichVienVPHC.RemoveRange(toChucVPRemove);
            }
        }

        public List<PhienDichModifyModel> GetPhienDich(NTS_ERPContext sqlContext, string idVuViec)
        {
            List<PhienDichModifyModel> listNguoiVP = new List<PhienDichModifyModel>();

            listNguoiVP = (from a in sqlContext.PhienDichVienVPHC.AsNoTracking()
                           where a.IdViPhamHC.Equals(idVuViec)
                           select new PhienDichModifyModel()
                           {
                               IdPhienDichVienVPHC = a.IdPhienDichVienVPHC,
                               IdViPhamHC = a.IdViPhamHC,
                               HoVaTen = a.HoVaTen,
                               GioiTinh = a.GioiTinh,
                               TenGioiTinh =  a.GioiTinh == 1 ? "Nam" : a.GioiTinh == 2 ? "Nữ" : "Khác",
                               NgaySinh = a.NgaySinh,
                               Cmnd = a.CMND,
                               DiaChi = a.DiaChi,
                               GhiChu = a.GhiChu,
                               SoDienThoai = a.SoDienThoai,
                               TrinhDoChuyenMon = a.TrinhDoChuyenMon,
                               NgayCap = a.NgayCap,
                               NoiCap = a.NoiCap,
                               NgheNghiep = a.NgheNghiep

                           }).OrderBy(o => o.HoVaTen).ToList().UpdateIndex();
            return listNguoiVP;
        }
    }
}
