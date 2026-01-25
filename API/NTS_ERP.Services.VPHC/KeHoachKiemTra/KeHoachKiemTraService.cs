using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;
using NTS_ERP.Services.Cores.Combobox;
using TrafficControl.Core;
using static NTS.Common.NTSConstants;

namespace NTS_ERP.Services.VPHC.KeHoachKiemTra
{
    public class KeHoachKiemTraService : IKeHoachKiemTraService
    {
        private readonly NTS_ERPContext _sqlContext;
        private IComboboxService _comboboxService;

        public KeHoachKiemTraService(NTS_ERPContext sqlContext
            , IComboboxService comboboxService)
        {
            this._sqlContext = sqlContext;
            _comboboxService = comboboxService;
        }


        public async Task<KeHoachKiemTraBaseResultModel> SearchAsync(KeHoachKiemTraSearchModel searchModel)
        {
            KeHoachKiemTraBaseResultModel searchResult = new KeHoachKiemTraBaseResultModel();

            var query = (from a in _sqlContext.KeHoachKiemTra.AsNoTracking()
                         join b in _sqlContext.DonVi.AsNoTracking() on a.IdDonVi equals b.IdDonVi into tmp
                         from t in tmp.DefaultIfEmpty()
                         where !a.IsDelete
                         select new KeHoachKiemTraSearchResultModel()
                         {
                             Id = a.Id,
                             IdDonVi = a.IdDonVi,
                             DonVi = t.Ten,
                             NoiDungKiemTra = a.NoiDungKiemTra,
                             SoQuyetDinhBanHanh = a.SoQuyetDinhBanHanh,
                             CanCu = a.CanCu,
                             MucDich = a.MucDich,
                             YeuCau = a.YeuCau,
                             TuNgayThucHienKeHoach = a.TuNgayThucHienKeHoach,
                             DenNgayThucHienKeHoach = a.DenNgayThucHienKeHoach,
                             TrangThaiKeHoachKiemTra = a.TrangThaiKeHoachKiemTra
                         }).AsQueryable();

            //if (!string.IsNullOrEmpty(searchModel.IdDonVi))
            //{
            //    var listDonVi = GlobalData.ListAllDonVi.Count > 0 ? GlobalData.ListAllDonVi : _sqlContext.DonVi.ToList();
            //    var donViSearch = listDonVi.Where(s => s.IdDonVi.Equals(searchModel.IdDonVi)).FirstOrDefault();
            //    //Nếu là bộ chỉ huy tỉnh
            //    if (donViSearch.Level == 1 || (donViSearch.Level == 2 && donViSearch.IdLoaiDonVi == "DV03"))
            //    {
            //        string idDonViCapDuoiList = String.Join(";", _comboboxService.GetDonViByIdDonVi(searchModel.IdDonVi).Result.Select(r => r.Id).ToList());
            //        if (!string.IsNullOrEmpty(idDonViCapDuoiList))
            //        {
            //            query = query.Where(i => idDonViCapDuoiList.Contains(i.IdDonVi));
            //        }
            //    }
            //    else
            //    {
            //        query = query.Where(a => searchModel.IdDonVi.Equals(a.IdDonVi));
            //    }
            //}

            if (!string.IsNullOrEmpty(searchModel.SoQuyetDinhBanHanh))
            {
                query = query.Where(a => a.SoQuyetDinhBanHanh.ToUpper().Contains(searchModel.SoQuyetDinhBanHanh.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.CanCu))
            {
                query = query.Where(a => a.CanCu.ToUpper().Contains(searchModel.CanCu.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.MucDich))
            {
                query = query.Where(a => a.MucDich.ToUpper().Contains(searchModel.MucDich.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.YeuCau))
            {
                query = query.Where(a => a.YeuCau.ToUpper().Contains(searchModel.YeuCau.ToUpper()));
            }

            if (searchModel.TuNgayThucHienKeHoach.HasValue || searchModel.DenNgayThucHienKeHoach.HasValue)
            {
                var searchFrom = searchModel.TuNgayThucHienKeHoach?.ToStartDate();
                var searchTo = searchModel.DenNgayThucHienKeHoach?.ToEndDate();

                query = query.Where(a =>
                    (!searchFrom.HasValue || a.DenNgayThucHienKeHoach >= searchFrom) &&
                    (!searchTo.HasValue || a.TuNgayThucHienKeHoach <= searchTo)
                );
            }

            searchResult.TotalItems = query.Select(r => r.Id).Count();
            searchResult.DataResults = query.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) =>
            {
                s.Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1;
                return s;
            }).ToList();

            return searchResult;
        }

        public async Task<KeHoachKiemTraDetailResponseModel> GetByIdAsnyc(string id)
        {
            var item = _sqlContext.KeHoachKiemTra.AsNoTracking().FirstOrDefault(r => r.Id.Equals(id));

            if (item == null) throw NTSException.CreateInstance(MessageResourceKey.ERR0003);

            KeHoachKiemTraDetailResponseModel outputModel = JsonConvert.DeserializeObject<KeHoachKiemTraDetailResponseModel>(JsonConvert.SerializeObject(item, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            var dataFileKeHoach = _sqlContext.FileKeHoachKiemTra.AsNoTracking().Where(x => x.IdKeHoachKiemTra == item.Id && x.IsDelete == false).ToList();
            if (dataFileKeHoach.Any())
            {
                var dataFileChoDuyet = dataFileKeHoach.Where(x => x.KieuLoai == 1).ToList();
                var dataFileDaDuyet = dataFileKeHoach.Where(x => x.KieuLoai == 2).ToList();
                outputModel.DataFileChoDuyet = JsonConvert.DeserializeObject<List<FileKeHoachKiemTraModel>>(JsonConvert.SerializeObject(dataFileChoDuyet, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                outputModel.DataFileDaDuyet = JsonConvert.DeserializeObject<List<FileKeHoachKiemTraModel>>(JsonConvert.SerializeObject(dataFileDaDuyet, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            }
            return outputModel;
        }

        public async Task<string> CreateAsync(KeHoachKiemTraCreateRequestModel requestModel, CurrentUserModel currentUser)
        {
            if (requestModel.DataFileChoDuyet == null || !requestModel.DataFileChoDuyet.Any())
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            Models.Entities.KeHoachKiemTra entity = new Models.Entities.KeHoachKiemTra
            {
                Id = Guid.NewGuid().ToString(),
                IdDonVi = requestModel.IdDonVi,
                NoiDungKiemTra = requestModel.NoiDungKiemTra,
                SoQuyetDinhBanHanh = requestModel.SoQuyetDinhBanHanh,
                CanCu = requestModel.CanCu,
                MucDich = requestModel.MucDich,
                YeuCau = requestModel.YeuCau,
                TuNgayThucHienKeHoach = requestModel.TuNgayThucHienKeHoach,
                DenNgayThucHienKeHoach = requestModel.DenNgayThucHienKeHoach,
                TrangThaiKeHoachKiemTra = (int)TrangThaiKHKTEnum.SoanThao,
                CreateBy = currentUser.UserId,
                CreateDate = DateTime.Now,
                UpdateBy = currentUser.UserId,
                UpdateDate = DateTime.Now,
            };

            if (string.IsNullOrEmpty(entity.SoQuyetDinhBanHanh))
            {
                entity.SoQuyetDinhBanHanh = $"{entity.IdDonVi}.{entity.CreateDate.Value.ToString(NTSConstants.DateFormatKey)}";
            }
            _sqlContext.KeHoachKiemTra.Add(entity);

            //add file
            //file gỗc
            foreach (var fileGoc in requestModel.DataFileChoDuyet)
            {
                Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
                {
                    Id = Guid.NewGuid().ToString(),
                    IdKeHoachKiemTra = entity.Id,
                    FileName = fileGoc.FileName,
                    FileUrl = fileGoc.FileUrl,
                    FileSize = fileGoc.FileSize,
                    KieuLoai = 1,
                    CreateBy = currentUser.UserId,
                    CreateDate = DateTime.Now,
                    UpdateBy = currentUser.UserId,
                    UpdateDate = DateTime.Now,

                };
                _sqlContext.FileKeHoachKiemTra.Add(file);
            }

            _sqlContext.SaveChanges();
            return entity.Id;
        }

        public async Task<string> UpdateAsync(KeHoachKiemTraUpdateRequestModel requestModel, CurrentUserModel currentUser)
        {
            var entity = _sqlContext.KeHoachKiemTra.FirstOrDefault(i => i.Id.Equals(requestModel.Id));

            if (entity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            if (requestModel.TrangThaiKeHoachKiemTra == (int)TrangThaiKHKTEnum.BanHanh)
            {
                if (requestModel.DataFileDaBanHanh == null || !requestModel.DataFileDaBanHanh.Any() || requestModel.DataFileSoanThao == null || !requestModel.DataFileSoanThao.Any())
                {
                    throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
                }
            }

            entity.IdDonVi = requestModel.IdDonVi;
            entity.NoiDungKiemTra = requestModel.NoiDungKiemTra;
            entity.SoQuyetDinhBanHanh = requestModel.SoQuyetDinhBanHanh;
            entity.CanCu = requestModel.CanCu;
            entity.MucDich = requestModel.MucDich;
            entity.YeuCau = requestModel.YeuCau;
            entity.TuNgayThucHienKeHoach = requestModel.TuNgayThucHienKeHoach;
            entity.DenNgayThucHienKeHoach = requestModel.DenNgayThucHienKeHoach;
            entity.TrangThaiKeHoachKiemTra = requestModel.TrangThaiKeHoachKiemTra;
            entity.UpdateBy = currentUser.UserId;
            entity.UpdateDate = DateTime.Now;

            _sqlContext.KeHoachKiemTra.Update(entity);

            //xóa tất cả file và add mới lại
            var listFile = _sqlContext.FileKeHoachKiemTra.Where(f => f.IdKeHoachKiemTra == entity.Id).ToList();
            if (listFile != null && listFile.Any())
            {
                _sqlContext.FileKeHoachKiemTra.RemoveRange(listFile);
            }

            //add lại
            if (requestModel.DataFileSoanThao != null && requestModel.DataFileSoanThao.Any())
            {
                foreach (var fileGoc in requestModel.DataFileSoanThao)
                {
                    Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdKeHoachKiemTra = entity.Id,
                        FileName = fileGoc.FileName,
                        FileUrl = fileGoc.FileUrl,
                        FileSize = fileGoc.FileSize,
                        KieuLoai = 1,
                        CreateBy = currentUser.UserId,
                        CreateDate = DateTime.Now,
                        UpdateBy = currentUser.UserId,
                        UpdateDate = DateTime.Now
                    };
                    _sqlContext.FileKeHoachKiemTra.Add(file);
                }
            }
            if (requestModel.DataFileDaBanHanh != null && requestModel.DataFileDaBanHanh.Any())
            {
                foreach (var fileDuyet in requestModel.DataFileDaBanHanh)
                {
                    Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdKeHoachKiemTra = entity.Id,
                        FileName = fileDuyet.FileName,
                        FileUrl = fileDuyet.FileUrl,
                        FileSize = fileDuyet.FileSize,
                        KieuLoai = 2,
                        CreateBy = currentUser.UserId,
                        CreateDate = DateTime.Now,
                        UpdateBy = currentUser.UserId,
                        UpdateDate = DateTime.Now
                    };
                    _sqlContext.FileKeHoachKiemTra.Add(file);
                }
            }
            _sqlContext.SaveChanges();

            return entity.Id;
        }

        public async Task DeleteEntityAsync(string id, string userid, bool isSoftDelete = true)
        {
            var entityRemove = _sqlContext.KeHoachKiemTra.FirstOrDefault(i => i.Id.Equals(id));

            if (entityRemove == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                if (entityRemove != null)
                {
                    if (isSoftDelete)
                        entityRemove.IsDelete = true;
                    else
                        _sqlContext.KeHoachKiemTra.Remove(entityRemove);
                }
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

        public async Task<string> UpdateStatusAsync(KeHoachKiemTraUpdateStatusModel requestModel, CurrentUserModel currentUser)
        {
            var entity = _sqlContext.KeHoachKiemTra.FirstOrDefault(i => i.Id.Equals(requestModel.Id));

            if (entity == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            if (!requestModel.TrangThaiKeHoachKiemTra.HasValue)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0002);
            }

            entity.TrangThaiKeHoachKiemTra = requestModel.TrangThaiKeHoachKiemTra;
            entity.UpdateBy = currentUser.UserId;
            entity.UpdateDate = DateTime.Now;
            _sqlContext.KeHoachKiemTra.Update(entity);
            _sqlContext.SaveChanges();
            return entity.Id;
        }
    }
}
