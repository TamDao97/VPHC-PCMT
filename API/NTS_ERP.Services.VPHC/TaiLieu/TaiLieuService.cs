using Microsoft.AspNetCore.Http.HttpResults;
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
using NTS_ERP.Models.VPHC.TaiLieu;
using TrafficControl.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace NTS_ERP.Services.VPHC.TaiLieu
{
    public class TaiLieuService : ITaiLieuService
    {

        private readonly NTS_ERPContext _sqlContext;
        private readonly IWordService _wordService;

        public TaiLieuService(NTS_ERPContext sqlContext, IWordService wordService)
        {
            this._sqlContext = sqlContext;
            this._wordService = wordService;
        }

        /// <summary>
        /// Tìm kiếm tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<TaiLieuResultModel>> Search(TaiLieuSearchModel searchModel)
        {
            SearchBaseResultModel<TaiLieuResultModel> searchResult = new SearchBaseResultModel<TaiLieuResultModel>();
            var dataQuery = (from a in _sqlContext.TaiLieu.AsNoTracking()
                             join b in _sqlContext.DanhMucTaiLieu.AsNoTracking() on a.IdCategory equals b.Id
                             orderby a.CreateDate
                             select new
                             {
                                 a.Id,
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
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new TaiLieuResultModel()
            {
                HoVaTen = s.Name
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Create(TaiLieuModifyModel model, string userId)
        {
            if (model.ListFileUpload == null || model.ListFileUpload?.Count <= 0)
            {
                return;
            }

            Models.Entities.TaiLieu taiLieuEntity ;
            foreach (var itemFile in model.ListFileUpload)
            {
                taiLieuEntity = new Models.Entities.TaiLieu();
                taiLieuEntity.Id = Guid.NewGuid().ToString();
                taiLieuEntity.IdVuViec = model.IdVuViec;
                taiLieuEntity.IdCategory = model.IdCategory;
                taiLieuEntity.FileName = itemFile.FileName;
                taiLieuEntity.FilePath = itemFile.FileUrl;
                taiLieuEntity.FileSize = itemFile.FileSize;
                //taiLieuEntity.Thumbnail = itemFile.Thumbnail;
                taiLieuEntity.Extention = itemFile.Extension;
                taiLieuEntity.CreateBy = userId;
                taiLieuEntity.CreateDate = DateTime.Now;
                taiLieuEntity.UpdateBy = userId;
                taiLieuEntity.UpdateDate = DateTime.Now;
                _sqlContext.TaiLieu.Add(taiLieuEntity);
            }

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(model.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                var taiLieu = _sqlContext.TaiLieu.Where(s => s.IdVuViec.Equals(model.IdVuViec)).Select(s => s.Id);
                vuViecUpdate.TongTaiLieu = taiLieu.Count() + (model.ListFileUpload?.Count()??0);
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
        /// Lấy tài liệu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TaiLieuModifyModel> GetById(string id)
        {
            var quyetDinhModel = _sqlContext.TaiLieu.AsNoTracking().FirstOrDefault(u => u.Id.Equals(id));

            if (quyetDinhModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            TaiLieuModifyModel outputModel = JsonConvert.DeserializeObject<TaiLieuModifyModel>(JsonConvert.SerializeObject(quyetDinhModel, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            return outputModel;
        }

        /// <summary>
        /// Xóa tài liệu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(string id, string userId)
        {
            var taiLieuEntity = _sqlContext.TaiLieu.FirstOrDefault(u => u.Id.Equals(id));
            if (taiLieuEntity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            _sqlContext.TaiLieu.Remove(taiLieuEntity);

            var vuViecUpdate = _sqlContext.VuViecVPHC.Where(s => s.Id.Equals(taiLieuEntity.IdVuViec)).FirstOrDefault();
            if (vuViecUpdate != null)
            {
                vuViecUpdate.TongTaiLieu = _sqlContext.TaiLieu.Where(s => s.IdVuViec.Equals(taiLieuEntity.IdVuViec)).Select(s => s.Id).Count() - 1;
            }

            await _sqlContext.SaveChangesAsync();
        }

        public List<DanhMucTaiLieuModle> GetDanhMuc()
        {
            List<DanhMucTaiLieuModle> listDanhMuc = _sqlContext.DanhMucTaiLieu.AsNoTracking().OrderBy(o => o.Order).Select(s => new DanhMucTaiLieuModle()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            }).ToList();
            return listDanhMuc;
        }

        public List<DanhMucTaiLieuModle> GetDanhMucTotal(string idVuViec)
        {
            List<DanhMucTaiLieuModle> listDanhMuc = (from a in _sqlContext.DanhMucTaiLieu.AsNoTracking().OrderBy(o => o.Order)
                                                     join b in _sqlContext.TaiLieu.Where(s => s.IdVuViec.Equals(idVuViec)) on a.Id equals b.IdCategory into ab
                                                     select new DanhMucTaiLieuModle
                                                     {
                                                         Id = a.Id,
                                                         Code = a.Code,
                                                         Name = a.Name,
                                                         Total = ab != null ? ab.Count() : 0
                                                     }).ToList();
            
            listDanhMuc.Insert(0, new DanhMucTaiLieuModle { Name = "Tất cả", Total = listDanhMuc.Sum(s => s.Total) });
            return listDanhMuc;
        }

        /// <summary>
        /// Lấy tài liệu của vụ việc
        /// </summary>
        /// <param name="id">Id vụ việc</param>
        /// <returns></returns>
        public List<TaiLieuVuViecModel> GetTaiLieuVuViec(string id,string? idDanhMuc)
        {
            var taiLieuModel = (from a in _sqlContext.TaiLieu.AsNoTracking().Where(u => u.IdVuViec.Equals(id))
                                join d in _sqlContext.DanhMucTaiLieu.AsNoTracking() on a.IdCategory equals d.Id
                                where string.IsNullOrEmpty(idDanhMuc) || a.IdCategory.Equals(idDanhMuc)
                                select new TaiLieuVuViecModel()
                                {
                                    IdTaiLieu = a.Id,
                                    Ten = a.FileName,
                                    DanhMuc = d.Name,
                                    DinhDang = a.Extention,
                                    DungLuong = a.FileSize,
                                    NgayTaiLen = a.CreateDate
                                }).ToList().UpdateIndex();

            if (taiLieuModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            return taiLieuModel;
        }

        public FileStreamResultModel GetFile(string id)
        {
            var taiLieuModel = _sqlContext.TaiLieu.AsNoTracking().FirstOrDefault(u => u.Id.Equals(id));
            if (taiLieuModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            string filePathServer = Path.Combine(Directory.GetCurrentDirectory(), taiLieuModel.FilePath);

            FileStreamResultModel fileStreamResult = new FileStreamResultModel();
            fileStreamResult.FileName = taiLieuModel.FileName;
            fileStreamResult.Extention = taiLieuModel.Extention;
            using (FileStream fileStream = new FileStream(filePathServer, FileMode.Open, FileAccess.Read))
            {
                MemoryStream memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                memoryStream.Position = 0; // Đặt con trỏ về đầu của MemoryStream
                fileStreamResult.MemoryStreamFile = memoryStream;
            }
            return fileStreamResult;
        }
    }
}
