using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;
using NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao;
using NTS_ERP.Services.Cores.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NTS.Common.NTSConstants;

namespace NTS_ERP.Services.VPHC.KeHoachKiemTraPhong
{
    public class KeHoachKiemTraPhongService : IKeHoachKiemTraPhongService
    {
        private readonly NTS_ERPContext _sqlContext;
        private IComboboxService _comboboxService;

        public KeHoachKiemTraPhongService(NTS_ERPContext sqlContext, IComboboxService comboboxService)
        {
            _sqlContext = sqlContext;
            _comboboxService = comboboxService;
        }

        public async Task<KeHoachKiemTraBaseResultModel> SearchAsync(KeHoachKiemTraSearchModel searchModel)
        {
            KeHoachKiemTraBaseResultModel searchResult = new KeHoachKiemTraBaseResultModel();

            var query = (from a in _sqlContext.KeHoachKiemTra.AsNoTracking()
                         join b in _sqlContext.DonVi.AsNoTracking() on a.IdDonVi equals b.IdDonVi into tmp
                         from t in tmp.DefaultIfEmpty()
                         where !a.IsDelete && a.CapKeHoach == (int)CapKeHoachEnum.Phong
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
                var dataFileChoDuyet = dataFileKeHoach.Where(x => x.LoaiFile == 1).ToList();
                var dataFileDaDuyet = dataFileKeHoach.Where(x => x.LoaiFile == 2).ToList();
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
                    {
                        var listFile = _sqlContext.FileKeHoachKiemTra.Where(f => f.IdKeHoachKiemTra == entityRemove.Id).ToList();
                        if (listFile != null && listFile.Any())
                        {
                            foreach (var file in listFile)
                            {
                                file.IsDelete = true;
                            }
                        }
                        //lấy all danh sách phân giao ra xóa
                        var lstDonViPhanGiaoOld = _sqlContext.KeHoachKiemTraPhanGiao.Where(i => i.IdKeHoachKiemTra.Equals(entityRemove.Id)).ToList();
                        if (lstDonViPhanGiaoOld != null && lstDonViPhanGiaoOld.Any())
                        {
                            foreach (var phangiao in lstDonViPhanGiaoOld)
                            {
                                phangiao.IsDelete = true;
                            }
                        }
                        entityRemove.IsDelete = true;
                    }
                    else
                    {
                        //lấy all danh sách file kế hoạch ra xóa
                        var listFile = _sqlContext.FileKeHoachKiemTra.Where(f => f.IdKeHoachKiemTra == entityRemove.Id).ToList();
                        if (listFile != null && listFile.Any())
                        {
                            _sqlContext.FileKeHoachKiemTra.RemoveRange(listFile);
                        }
                        //lấy all danh sách phân giao ra xóa
                        var lstDonViPhanGiaoOld = _sqlContext.KeHoachKiemTraPhanGiao.Where(i => i.IdKeHoachKiemTra.Equals(entityRemove.Id)).ToList();
                        if (lstDonViPhanGiaoOld != null && lstDonViPhanGiaoOld.Any())
                        {
                            _sqlContext.KeHoachKiemTraPhanGiao.RemoveRange(lstDonViPhanGiaoOld);
                        }
                        _sqlContext.KeHoachKiemTra.Remove(entityRemove);
                    }
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

        

        public async Task<List<KeHoachKiemTraPhanGiaoModel>> GetDetailAssigneeTaskByIdKeHoachAsync(string idKeHoach, CurrentUserModel currentUser)
        {
            var lstDonViPhanGiao = _sqlContext.KeHoachKiemTraPhanGiao.Where(x => x.IdKeHoachKiemTra == idKeHoach && x.IsDelete == false).Select(i => new KeHoachKiemTraPhanGiaoModel
            {
                IdDonVi = i.IdDonVi,
                NgayNhanPhanGiao = i.NgayNhanPhanGiao,
                NgayKetThuc = i.NgayKetThuc,
                SoDoiTuong = i.SoDoiTuong,
                SoVu = i.SoVu,
                TongTienXuPhat = i.TongTienXuPhat,

            }).ToList();
            return lstDonViPhanGiao;


        }
        public async Task<string> UpdateAsync(
    KeHoachKiemTraUpdateRequestModel request,
    CurrentUserModel user)
        {
            var entity = await _sqlContext.KeHoachKiemTra
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDelete);

            if (entity == null)
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            entity.SoQuyetDinhBanHanh = request.SoQuyetDinhBanHanh;
            entity.NoiDungKiemTra = request.NoiDungKiemTra;
            entity.CanCu = request.CanCu;
            entity.MucDich = request.MucDich;
            entity.YeuCau = request.YeuCau;
            entity.TuNgayThucHienKeHoach = request.TuNgayThucHienKeHoach;
            entity.DenNgayThucHienKeHoach = request.DenNgayThucHienKeHoach;
            entity.UpdateBy = user.UserId;
            entity.UpdateDate = DateTime.Now;

            // ❗ File: soft delete
            var oldFiles = _sqlContext.FileKeHoachKiemTra
                .Where(x => x.IdKeHoachKiemTra == entity.Id && !x.IsDelete);

            foreach (var f in oldFiles)
                f.IsDelete = true;

            foreach (var f in request.DataFileSoanThao)
            {
                _sqlContext.FileKeHoachKiemTra.Add(new FileKeHoachKiemTra
                {
                    Id = Guid.NewGuid().ToString(),
                    IdKeHoachKiemTra = entity.Id,
                    FileName = f.FileName,
                    FileUrl = f.FileUrl,
                    FileSize = f.FileSize,
                    LoaiFile = 1,
                    CreateBy = user.UserId,
                    CreateDate = DateTime.Now
                });
            }
            foreach (var f in request.DataFileBanHanh)
            {
                _sqlContext.FileKeHoachKiemTra.Add(new FileKeHoachKiemTra
                {
                    Id = Guid.NewGuid().ToString(),
                    IdKeHoachKiemTra = entity.Id,
                    FileName = f.FileName,
                    FileUrl = f.FileUrl,
                    FileSize = f.FileSize,
                    LoaiFile = 2,
                    CreateBy = user.UserId,
                    CreateDate = DateTime.Now
                });
            }


            await _sqlContext.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<string> AssigneeTaskAsync(
    KeHoachKiemTraPhanGiaoCreateRequestModel request,
    CurrentUserModel currentUser)
        {
            var keHoachCuc = await _sqlContext.KeHoachKiemTra
                .FirstOrDefaultAsync(x => x.Id == request.IdKeHoachKiemTra
                                       && !x.IsDelete);

            if (keHoachCuc == null)
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);

            // 🔹 1. LẤY DANH SÁCH PHÂN GIAO HIỆN TẠI
            var lstPhanGiaoDb = await _sqlContext.KeHoachKiemTraPhanGiao
                .Where(x => x.IdKeHoachKiemTra == keHoachCuc.Id && !x.IsDelete)
                .ToListAsync();

            var lstDonViRequest = request.LstDonViPhanGiao
                .Select(x => x.IdDonVi)
                .ToList();

            // ❌ 2. XÓA (soft) CÁC ĐƠN VỊ KHÔNG CÒN
            foreach (var old in lstPhanGiaoDb)
            {
                if (!lstDonViRequest.Contains(old.IdDonVi))
                {
                    old.IsDelete = true;
                    old.UpdateBy = currentUser.UserId;
                    old.UpdateDate = DateTime.Now;
                }
            }

            // 🔁 3. UPDATE + INSERT
            foreach (var item in request.LstDonViPhanGiao)
            {
                var phanGiao = lstPhanGiaoDb
                    .FirstOrDefault(x => x.IdDonVi == item.IdDonVi);

                if (phanGiao != null)
                {
                    // ✏️ UPDATE
                    phanGiao.NgayNhanPhanGiao = item.NgayNhanPhanGiao;
                    phanGiao.NgayKetThuc = item.NgayKetThuc;
                    phanGiao.SoVu = item.SoVu;
                    phanGiao.SoDoiTuong = item.SoDoiTuong;
                    phanGiao.TongTienXuPhat = item.TongTienXuPhat;
                    phanGiao.UpdateBy = currentUser.UserId;
                    phanGiao.UpdateDate = DateTime.Now;
                }
                else
                {
                    // ➕ INSERT phân giao
                    _sqlContext.KeHoachKiemTraPhanGiao.Add(
                        new KeHoachKiemTraPhanGiao
                        {
                            Id = Guid.NewGuid().ToString(),
                            IdKeHoachKiemTra = keHoachCuc.Id,
                            IdDonVi = item.IdDonVi,
                            NgayNhanPhanGiao = item.NgayNhanPhanGiao ?? DateTime.Now,
                            NgayKetThuc = item.NgayKetThuc,
                            SoVu = item.SoVu,
                            SoDoiTuong = item.SoDoiTuong,
                            TongTienXuPhat = item.TongTienXuPhat,
                            CreateBy = currentUser.UserId,
                            CreateDate = DateTime.Now
                        });

                    // ➕ INSERT kế hoạch PHÒNG (CHỈ TẠO 1 LẦN)
                    bool existKeHoachPhong = await _sqlContext.KeHoachKiemTra.AnyAsync(x =>
                        x.IdKeHoachCha == keHoachCuc.Id &&
                        x.IdDonVi == item.IdDonVi &&
                        !x.IsDelete);

                    if (!existKeHoachPhong)
                    {
                        _sqlContext.KeHoachKiemTra.Add(
                            new Models.Entities.KeHoachKiemTra
                            {
                                Id = Guid.NewGuid().ToString(),
                                IdKeHoachCha = keHoachCuc.Id,
                                CapKeHoach = 3, // Phòng
                                IdDonVi = item.IdDonVi,
                                NoiDungKiemTra = keHoachCuc.NoiDungKiemTra,
                                CanCu = keHoachCuc.CanCu,
                                MucDich = keHoachCuc.MucDich,
                                YeuCau = keHoachCuc.YeuCau,
                                TuNgayThucHienKeHoach = keHoachCuc.TuNgayThucHienKeHoach,
                                DenNgayThucHienKeHoach = keHoachCuc.DenNgayThucHienKeHoach,
                                TrangThaiKeHoachKiemTra = (int)TrangThaiKHKTEnum.SoanThao,
                                CreateBy = currentUser.UserId,
                                CreateDate = DateTime.Now
                            });
                    }
                }
            }

            await _sqlContext.SaveChangesAsync();
            return keHoachCuc.Id;
        }
    }
}
