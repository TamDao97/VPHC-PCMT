using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;
using NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao;
using NTS_ERP.Services.Cores;
using NTS_ERP.Services.Cores.Combobox;
using Syncfusion.DocIO.DLS;
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


        public async Task<KeHoachKiemTraBaseResultModel> SearchAsync(KeHoachKiemTraSearchModel searchModel, CurrentUserModel user)
        {
            KeHoachKiemTraBaseResultModel searchResult = new KeHoachKiemTraBaseResultModel();

            //check idDonVi
            var listDonVi = GlobalData.ListAllDonVi.Count > 0 ? GlobalData.ListAllDonVi : _sqlContext.DonVi.ToList();
            var donViSearch = listDonVi.Where(s => s.IdDonVi.Equals(user.DonViId)).FirstOrDefault();
            //Nếu là bộ chỉ huy tỉnh cấp cao nhất
            // lấy kế hoạch cấp cục và cấp phòng
            //cấp cục phải trạng thái != soạn thảo vì soạn thảo là chưa trình lên để duyệt
            //cấp phòng phải trạng thái != tiếp nhận vì cấp phòng chưa trình lên để duyệt
            if (donViSearch.Level == 1 || (donViSearch.Level == 2 && donViSearch.IdLoaiDonVi == "DV03"))
            {
                //lấy all data các đơn vị n quản lý có capKeHoach = 1,2 và trạng thái !=(int)TrangThaiKHKTEnum.TiepNhan và !=(int)TrangThaiKHKTEnum.SoanThao
                //lấy đơn vị cấp dưới
                var donViCapDuoiIds = _comboboxService
                        .GetDonViByIdDonVi(user.DonViId).Result
                        .Select(x => x.Id)
                        .ToList();

                var queryCap1 =
                    from kh in _sqlContext.KeHoachKiemTra.AsNoTracking()
                    join b in _sqlContext.DonVi.AsNoTracking() on kh.IdDonVi equals b.IdDonVi into tmp
                    from t in tmp.DefaultIfEmpty()
                    where !kh.IsDelete

                          // 🔥 ĐIỀU KIỆN PHÂN QUYỀN – PHẢI ĐẶT ĐẦU TIÊN
                          && (
                                kh.IdDonVi == user.DonViId
                             || donViCapDuoiIds.Contains(kh.IdDonVi)
                          )
                    select new KeHoachKiemTraSearchResultModel()
                    {
                        Id = kh.Id,
                        IdDonVi = kh.IdDonVi,
                        DonVi = t.Ten,
                        NoiDungKiemTra = kh.NoiDungKiemTra,
                        SoQuyetDinhBanHanh = kh.SoQuyetDinhBanHanh,
                        CanCu = kh.CanCu,
                        MucDich = kh.MucDich,
                        YeuCau = kh.YeuCau,
                        TuNgayThucHienKeHoach = kh.TuNgayThucHienKeHoach,
                        DenNgayThucHienKeHoach = kh.DenNgayThucHienKeHoach,
                        TrangThaiKeHoachKiemTra = kh.TrangThaiKeHoachKiemTra,
                        CapKeHoach = kh.CapKeHoach,                        
                    };
                queryCap1 = queryCap1.Where(x =>
                    // kế hoạch Cục
                    (x.CapKeHoach == (int)CapKeHoachEnum.Cuc
                        && x.TrangThaiKeHoachKiemTra != (int)TrangThaiKHKTEnum.SoanThao)

                    // kế hoạch Phòng
                    || (x.CapKeHoach == (int)CapKeHoachEnum.Phong
                        && x.TrangThaiKeHoachKiemTra != (int)TrangThaiKHKTEnum.TiepNhan)
                );

                if (!string.IsNullOrEmpty(searchModel.SoQuyetDinhBanHanh))
                {
                    queryCap1 = queryCap1.Where(a => a.SoQuyetDinhBanHanh.ToUpper().Contains(searchModel.SoQuyetDinhBanHanh.ToUpper()));
                }

                if (!string.IsNullOrEmpty(searchModel.CanCu))
                {
                    queryCap1 = queryCap1.Where(a => a.CanCu.ToUpper().Contains(searchModel.CanCu.ToUpper()));
                }

                if (!string.IsNullOrEmpty(searchModel.MucDich))
                {
                    queryCap1 = queryCap1.Where(a => a.MucDich.ToUpper().Contains(searchModel.MucDich.ToUpper()));
                }

                if (!string.IsNullOrEmpty(searchModel.YeuCau))
                {
                    queryCap1 = queryCap1.Where(a => a.YeuCau.ToUpper().Contains(searchModel.YeuCau.ToUpper()));
                }

                if (searchModel.TuNgayThucHienKeHoach.HasValue || searchModel.DenNgayThucHienKeHoach.HasValue)
                {
                    var searchFrom = searchModel.TuNgayThucHienKeHoach?.ToStartDate();
                    var searchTo = searchModel.DenNgayThucHienKeHoach?.ToEndDate();

                    queryCap1 = queryCap1.Where(a =>
                        (!searchFrom.HasValue || a.DenNgayThucHienKeHoach >= searchFrom) &&
                        (!searchTo.HasValue || a.TuNgayThucHienKeHoach <= searchTo)
                    );
                }

                searchResult.TotalItems = queryCap1.Select(r => r.Id).Count();
                searchResult.DataResults = queryCap1.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) =>
                {
                    s.Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1;
                    return s;
                }).ToList();

            }
            else
            {
                //cấp cục
                if(donViSearch.Level == 2)
                {
                    // lấy kế hoạch cấp cục của đơn vị user.DonViId thôi

                    var queryCap2 =
                        from kh in _sqlContext.KeHoachKiemTra.AsNoTracking()
                        join b in _sqlContext.DonVi.AsNoTracking() on kh.IdDonVi equals b.IdDonVi into tmp
                        from t in tmp.DefaultIfEmpty()
                        where !kh.IsDelete && kh.IdDonVi == user.DonViId && kh.CapKeHoach == (int)CapKeHoachEnum.Cuc
                        select new KeHoachKiemTraSearchResultModel()
                        {
                            Id = kh.Id,
                            IdDonVi = kh.IdDonVi,
                            DonVi = t.Ten,
                            NoiDungKiemTra = kh.NoiDungKiemTra,
                            SoQuyetDinhBanHanh = kh.SoQuyetDinhBanHanh,
                            CanCu = kh.CanCu,
                            MucDich = kh.MucDich,
                            YeuCau = kh.YeuCau,
                            TuNgayThucHienKeHoach = kh.TuNgayThucHienKeHoach,
                            DenNgayThucHienKeHoach = kh.DenNgayThucHienKeHoach,
                            TrangThaiKeHoachKiemTra = kh.TrangThaiKeHoachKiemTra,
                            CapKeHoach = kh.CapKeHoach,
                        };
                    if (!string.IsNullOrEmpty(searchModel.SoQuyetDinhBanHanh))
                    {
                        queryCap2 = queryCap2.Where(a => a.SoQuyetDinhBanHanh.ToUpper().Contains(searchModel.SoQuyetDinhBanHanh.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.CanCu))
                    {
                        queryCap2 = queryCap2.Where(a => a.CanCu.ToUpper().Contains(searchModel.CanCu.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.MucDich))
                    {
                        queryCap2 = queryCap2.Where(a => a.MucDich.ToUpper().Contains(searchModel.MucDich.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.YeuCau))
                    {
                        queryCap2 = queryCap2.Where(a => a.YeuCau.ToUpper().Contains(searchModel.YeuCau.ToUpper()));
                    }

                    if (searchModel.TuNgayThucHienKeHoach.HasValue || searchModel.DenNgayThucHienKeHoach.HasValue)
                    {
                        var searchFrom = searchModel.TuNgayThucHienKeHoach?.ToStartDate();
                        var searchTo = searchModel.DenNgayThucHienKeHoach?.ToEndDate();

                        queryCap2 = queryCap2.Where(a =>
                            (!searchFrom.HasValue || a.DenNgayThucHienKeHoach >= searchFrom) &&
                            (!searchTo.HasValue || a.TuNgayThucHienKeHoach <= searchTo)
                        );
                    }

                    searchResult.TotalItems = queryCap2.Select(r => r.Id).Count();
                    searchResult.DataResults = queryCap2.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) =>
                    {
                        s.Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1;
                        return s;
                    }).ToList();
                }

                //cấp phòng
                if(donViSearch.Level == 3)
                {
                    
                    var queryCap3 =
                        from kh in _sqlContext.KeHoachKiemTra.AsNoTracking()
                        join pg in _sqlContext.KeHoachKiemTraPhanGiao.AsNoTracking().Where(x=>x.IsDelete == false)
                            on kh.IdKeHoachCha equals pg.IdKeHoachKiemTra into pgGroup
                        from pg in pgGroup.DefaultIfEmpty()
                        join b in _sqlContext.DonVi.AsNoTracking() on kh.IdDonVi equals b.IdDonVi into tmp
                        from t in tmp.DefaultIfEmpty()
                        where !kh.IsDelete && kh.IdDonVi == user.DonViId && pg.IdDonVi == user.DonViId && kh.CapKeHoach == (int)CapKeHoachEnum.Phong

                        select new KeHoachKiemTraSearchResultModel()
                        {
                            Id = kh.Id,
                            IdDonVi = kh.IdDonVi,
                            DonVi = t.Ten,
                            NoiDungKiemTra = kh.NoiDungKiemTra,
                            SoQuyetDinhBanHanh = kh.SoQuyetDinhBanHanh,
                            CanCu = kh.CanCu,
                            MucDich = kh.MucDich,
                            YeuCau = kh.YeuCau,
                            TuNgayThucHienKeHoach = kh.TuNgayThucHienKeHoach,
                            DenNgayThucHienKeHoach = kh.DenNgayThucHienKeHoach,
                            TrangThaiKeHoachKiemTra = kh.TrangThaiKeHoachKiemTra,
                            CapKeHoach = kh.CapKeHoach,

                            SoDoiTuong = pg.SoDoiTuong,
                            SoVu = pg.SoVu,
                            TongTienXuPhat = pg.TongTienXuPhat,
                            TrangThaiPhanGiao = pg.TrangThai,

                        };

                    if (!string.IsNullOrEmpty(searchModel.SoQuyetDinhBanHanh))
                    {
                        queryCap3 = queryCap3.Where(a => a.SoQuyetDinhBanHanh.ToUpper().Contains(searchModel.SoQuyetDinhBanHanh.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.CanCu))
                    {
                        queryCap3 = queryCap3.Where(a => a.CanCu.ToUpper().Contains(searchModel.CanCu.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.MucDich))
                    {
                        queryCap3 = queryCap3.Where(a => a.MucDich.ToUpper().Contains(searchModel.MucDich.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.YeuCau))
                    {
                        queryCap3 = queryCap3.Where(a => a.YeuCau.ToUpper().Contains(searchModel.YeuCau.ToUpper()));
                    }

                    if (searchModel.TuNgayThucHienKeHoach.HasValue || searchModel.DenNgayThucHienKeHoach.HasValue)
                    {
                        var searchFrom = searchModel.TuNgayThucHienKeHoach?.ToStartDate();
                        var searchTo = searchModel.DenNgayThucHienKeHoach?.ToEndDate();

                        queryCap3 = queryCap3.Where(a =>
                            (!searchFrom.HasValue || a.DenNgayThucHienKeHoach >= searchFrom) &&
                            (!searchTo.HasValue || a.TuNgayThucHienKeHoach <= searchTo)
                        );
                    }

                    searchResult.TotalItems = queryCap3.Select(r => r.Id).Count();
                    searchResult.DataResults = queryCap3.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) =>
                    {
                        s.Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1;
                        return s;
                    }).ToList();

                }

                //cấp đồn: chỉ có phân giao k có bản ghi kế hoạch
                if(donViSearch.Level == 4)
                {
                    var queryCap4 =
                        from pg in _sqlContext.KeHoachKiemTraPhanGiao.AsNoTracking().Where(x=>x.IsDelete == false)
                        join kh in _sqlContext.KeHoachKiemTra.AsNoTracking().Where(x => x.IsDelete == false) 
                            on pg.IdKeHoachKiemTra equals kh.Id
                        join b in _sqlContext.DonVi.AsNoTracking() on pg.IdDonVi equals b.IdDonVi into tmp
                        from t in tmp.DefaultIfEmpty()
                        where pg.IdDonVi == user.DonViId && kh.CapKeHoach == (int)CapKeHoachEnum.Phong
                        select new KeHoachKiemTraSearchResultModel()
                        {
                            Id = kh.Id,
                            IdDonVi = pg.IdDonVi,
                            DonVi = t.Ten,
                            NoiDungKiemTra = kh.NoiDungKiemTra,
                            SoQuyetDinhBanHanh = kh.SoQuyetDinhBanHanh,
                            CanCu = kh.CanCu,
                            MucDich = kh.MucDich,
                            YeuCau = kh.YeuCau,
                            TuNgayThucHienKeHoach = pg.NgayNhanPhanGiao,
                            DenNgayThucHienKeHoach = pg.NgayKetThuc,
                            TrangThaiKeHoachKiemTra = kh.TrangThaiKeHoachKiemTra,
                            CapKeHoach = (int)CapKeHoachEnum.Don,

                            SoDoiTuong = pg.SoDoiTuong,
                            SoVu = pg.SoVu,
                            TongTienXuPhat = pg.TongTienXuPhat,
                            TrangThaiPhanGiao = pg.TrangThai,

                        };
                    if (!string.IsNullOrEmpty(searchModel.SoQuyetDinhBanHanh))
                    {
                        queryCap4 = queryCap4.Where(a => a.SoQuyetDinhBanHanh.ToUpper().Contains(searchModel.SoQuyetDinhBanHanh.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.CanCu))
                    {
                        queryCap4 = queryCap4.Where(a => a.CanCu.ToUpper().Contains(searchModel.CanCu.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.MucDich))
                    {
                        queryCap4 = queryCap4.Where(a => a.MucDich.ToUpper().Contains(searchModel.MucDich.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(searchModel.YeuCau))
                    {
                        queryCap4 = queryCap4.Where(a => a.YeuCau.ToUpper().Contains(searchModel.YeuCau.ToUpper()));
                    }

                    if (searchModel.TuNgayThucHienKeHoach.HasValue || searchModel.DenNgayThucHienKeHoach.HasValue)
                    {
                        var searchFrom = searchModel.TuNgayThucHienKeHoach?.ToStartDate();
                        var searchTo = searchModel.DenNgayThucHienKeHoach?.ToEndDate();

                        queryCap4 = queryCap4.Where(a =>
                            (!searchFrom.HasValue || a.DenNgayThucHienKeHoach >= searchFrom) &&
                            (!searchTo.HasValue || a.TuNgayThucHienKeHoach <= searchTo)
                        );
                    }

                    searchResult.TotalItems = queryCap4.Select(r => r.Id).Count();
                    searchResult.DataResults = queryCap4.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select((s, index) =>
                    {
                        s.Index = (searchModel.PageNumber - 1) * searchModel.PageSize + index + 1;
                        return s;
                    }).ToList();
                }


            }
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
        #region Tạm thời comment

        //public async Task<string> CreateAsync(KeHoachKiemTraCreateRequestModel requestModel, CurrentUserModel currentUser)
        //{
        //    if (requestModel.DataFileChoDuyet == null || !requestModel.DataFileChoDuyet.Any())
        //    {
        //        throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
        //    }
        //    Models.Entities.KeHoachKiemTra entity = new Models.Entities.KeHoachKiemTra
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        IdDonVi = requestModel.IdDonVi,
        //        NoiDungKiemTra = requestModel.NoiDungKiemTra,
        //        SoQuyetDinhBanHanh = requestModel.SoQuyetDinhBanHanh,
        //        CanCu = requestModel.CanCu,
        //        MucDich = requestModel.MucDich,
        //        YeuCau = requestModel.YeuCau,
        //        TuNgayThucHienKeHoach = requestModel.TuNgayThucHienKeHoach,
        //        DenNgayThucHienKeHoach = requestModel.DenNgayThucHienKeHoach,
        //        TrangThaiKeHoachKiemTra = (int)TrangThaiKHKTEnum.SoanThao,
        //        CreateBy = currentUser.UserId,
        //        CreateDate = DateTime.Now,
        //        UpdateBy = currentUser.UserId,
        //        UpdateDate = DateTime.Now,
        //    };

        //    if (string.IsNullOrEmpty(entity.SoQuyetDinhBanHanh))
        //    {
        //        entity.SoQuyetDinhBanHanh = $"{entity.IdDonVi}.{entity.CreateDate.Value.ToString(NTSConstants.DateFormatKey)}";
        //    }
        //    _sqlContext.KeHoachKiemTra.Add(entity);

        //    //add file
        //    //file gỗc
        //    foreach (var fileGoc in requestModel.DataFileChoDuyet)
        //    {
        //        Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            IdKeHoachKiemTra = entity.Id,
        //            FileName = fileGoc.FileName,
        //            FileUrl = fileGoc.FileUrl,
        //            FileSize = fileGoc.FileSize,
        //            LoaiFile = 1,
        //            CreateBy = currentUser.UserId,
        //            CreateDate = DateTime.Now,
        //            UpdateBy = currentUser.UserId,
        //            UpdateDate = DateTime.Now,

        //        };
        //        _sqlContext.FileKeHoachKiemTra.Add(file);
        //    }

        //    _sqlContext.SaveChanges();
        //    return entity.Id;
        //}

        //public async Task<string> UpdateAsync(KeHoachKiemTraUpdateRequestModel requestModel, CurrentUserModel currentUser)
        //{
        //    var entity = _sqlContext.KeHoachKiemTra.FirstOrDefault(i => i.Id.Equals(requestModel.Id));

        //    if (entity == null)
        //    {
        //        throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
        //    }

        //    if (requestModel.TrangThaiKeHoachKiemTra == (int)TrangThaiKHKTEnum.BanHanh)
        //    {
        //        if (requestModel.DataFileBanHanh == null || !requestModel.DataFileBanHanh.Any() || requestModel.DataFileSoanThao == null || !requestModel.DataFileSoanThao.Any())
        //        {
        //            throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
        //        }
        //    }

        //    entity.IdDonVi = requestModel.IdDonVi;
        //    entity.NoiDungKiemTra = requestModel.NoiDungKiemTra;
        //    entity.SoQuyetDinhBanHanh = requestModel.SoQuyetDinhBanHanh;
        //    entity.CanCu = requestModel.CanCu;
        //    entity.MucDich = requestModel.MucDich;
        //    entity.YeuCau = requestModel.YeuCau;
        //    entity.TuNgayThucHienKeHoach = requestModel.TuNgayThucHienKeHoach;
        //    entity.DenNgayThucHienKeHoach = requestModel.DenNgayThucHienKeHoach;
        //    entity.TrangThaiKeHoachKiemTra = requestModel.TrangThaiKeHoachKiemTra;
        //    entity.UpdateBy = currentUser.UserId;
        //    entity.UpdateDate = DateTime.Now;

        //    _sqlContext.KeHoachKiemTra.Update(entity);

        //    //xóa tất cả file và add mới lại
        //    var listFile = _sqlContext.FileKeHoachKiemTra.Where(f => f.IdKeHoachKiemTra == entity.Id).ToList();
        //    if (listFile != null && listFile.Any())
        //    {
        //        _sqlContext.FileKeHoachKiemTra.RemoveRange(listFile);
        //    }

        //    //add lại
        //    if (requestModel.DataFileSoanThao != null && requestModel.DataFileSoanThao.Any())
        //    {
        //        foreach (var fileGoc in requestModel.DataFileSoanThao)
        //        {
        //            Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                IdKeHoachKiemTra = entity.Id,
        //                FileName = fileGoc.FileName,
        //                FileUrl = fileGoc.FileUrl,
        //                FileSize = fileGoc.FileSize,
        //                LoaiFile = 1,
        //                CreateBy = currentUser.UserId,
        //                CreateDate = DateTime.Now,
        //                UpdateBy = currentUser.UserId,
        //                UpdateDate = DateTime.Now
        //            };
        //            _sqlContext.FileKeHoachKiemTra.Add(file);
        //        }
        //    }
        //    if (requestModel.DataFileBanHanh != null && requestModel.DataFileBanHanh.Any())
        //    {
        //        foreach (var fileDuyet in requestModel.DataFileBanHanh)
        //        {
        //            Models.Entities.FileKeHoachKiemTra file = new FileKeHoachKiemTra
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                IdKeHoachKiemTra = entity.Id,
        //                FileName = fileDuyet.FileName,
        //                FileUrl = fileDuyet.FileUrl,
        //                FileSize = fileDuyet.FileSize,
        //                LoaiFile = 2,
        //                CreateBy = currentUser.UserId,
        //                CreateDate = DateTime.Now,
        //                UpdateBy = currentUser.UserId,
        //                UpdateDate = DateTime.Now
        //            };
        //            _sqlContext.FileKeHoachKiemTra.Add(file);
        //        }
        //    }
        //    _sqlContext.SaveChanges();

        //    return entity.Id;
        //}

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

        //public async Task<string> AssigneeTaskAsync(KeHoachKiemTraPhanGiaoCreateRequestModel requestModel, CurrentUserModel currentUser)
        //{
        //    var entity = _sqlContext.KeHoachKiemTra.FirstOrDefault(i => i.Id.Equals(requestModel.IdKeHoachKiemTra));

        //    if (entity == null)
        //    {
        //        throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
        //    }

        //    var lstDonViPhanGiaoOld = _sqlContext.KeHoachKiemTraPhanGiao.Where(i => i.IdKeHoachKiemTra.Equals(requestModel.IdKeHoachKiemTra)).ToList();
        //    var lstDonViPhanGiaoAdd = new List<KeHoachKiemTraPhanGiao>();
        //    foreach (var item in requestModel.LstDonViPhanGiao)
        //    {
        //        lstDonViPhanGiaoAdd.Add(new KeHoachKiemTraPhanGiao
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            IdKeHoachKiemTra = requestModel.IdKeHoachKiemTra,
        //            IdDonVi = item.IdDonVi,
        //            NgayNhanPhanGiao = item.NgayNhanPhanGiao,
        //            NgayKetThuc = item.NgayKetThuc,
        //            SoDoiTuong = item.SoDoiTuong,
        //            SoVu = item.SoVu,
        //            TongTienXuPhat = item.TongTienXuPhat,
        //            CreateBy = currentUser.UserId,
        //            CreateDate = DateTime.Now,
        //            UpdateBy = currentUser.UserId,
        //            UpdateDate = DateTime.Now,
        //        });
        //    }

        //    _sqlContext.KeHoachKiemTraPhanGiao.RemoveRange(lstDonViPhanGiaoOld);
        //    _sqlContext.KeHoachKiemTraPhanGiao.AddRange(lstDonViPhanGiaoAdd);

        //    //tạo bản ghi kế hoạch phòng

        //    _sqlContext.SaveChanges();
        //    return entity.Id;
        //}

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
        #endregion
        public async Task<string> CreateAsync(
    KeHoachKiemTraCreateRequestModel request,
    CurrentUserModel user)
        {
            if (request.DataFileChoDuyet == null || !request.DataFileChoDuyet.Any())
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);

            var entity = new Models.Entities.KeHoachKiemTra
            {
                Id = Guid.NewGuid().ToString(),

                CapKeHoach = (int)CapKeHoachEnum.Cuc,
                IdKeHoachCha = null,

                IdDonVi = request.IdDonVi,
                SoQuyetDinhBanHanh = request.SoQuyetDinhBanHanh,
                NoiDungKiemTra = request.NoiDungKiemTra,
                CanCu = request.CanCu,
                MucDich = request.MucDich,
                YeuCau = request.YeuCau,

                TuNgayThucHienKeHoach = request.TuNgayThucHienKeHoach,
                DenNgayThucHienKeHoach = request.DenNgayThucHienKeHoach,

                TrangThaiKeHoachKiemTra = (int)TrangThaiKeHoachEnum.SoanThao,

                CreateBy = user.UserId,
                CreateDate = DateTime.Now,
                UpdateBy = user.UserId,
                UpdateDate = DateTime.Now,
                IsDelete = false
            };

            _sqlContext.KeHoachKiemTra.Add(entity);

            foreach (var f in request.DataFileChoDuyet)
            {
                _sqlContext.FileKeHoachKiemTra.Add(new FileKeHoachKiemTra
                {
                    Id = Guid.NewGuid().ToString(),
                    IdKeHoachKiemTra = entity.Id,
                    FileName = f.FileName,
                    FileUrl = f.FileUrl,
                    FileSize = f.FileSize,
                    LoaiFile = (int)LoaiFileKeHoachEnum.SoanThao,
                    CreateBy = user.UserId,
                    CreateDate = DateTime.Now,
                    IsDelete = false
                });
            }

            await _sqlContext.SaveChangesAsync();
            return entity.Id;
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
            var listDonVi = GlobalData.ListAllDonVi.Count > 0 ? GlobalData.ListAllDonVi : _sqlContext.DonVi.ToList();
            var donViSearch = listDonVi.Where(s => s.IdDonVi.Equals(keHoachCuc.IdDonVi)).FirstOrDefault();
            var capKeHoach = 2;
            if(donViSearch.Level == 2) //cấp cục
            {
                capKeHoach = 2; // phân giao xuống cấp phòng
            }
            if(donViSearch.Level == 3) // cấp phòng
            {
                capKeHoach = 3; //phân giao xuống cấp đồn
            }
            //cấp cục phân giao
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
                    var trangThaiPhanGiao = (int)TrangThaiKHKTEnum.TiepNhan;
                    if(capKeHoach == 3)
                    {
                        trangThaiPhanGiao = (int)TrangThaiKeHoachEnum.DangTrienKhai;
                    }
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
                            TrangThai = trangThaiPhanGiao,
                            CreateBy = currentUser.UserId,
                            CreateDate = DateTime.Now,

                        });

                    // ➕ INSERT kế hoạch PHÒNG (CHỈ TẠO 1 LẦN)
                    bool existKeHoachPhong = await _sqlContext.KeHoachKiemTra.AnyAsync(x =>
                        x.IdKeHoachCha == keHoachCuc.Id &&
                        x.IdDonVi == item.IdDonVi &&
                        !x.IsDelete);
                    if(capKeHoach != 3)
                    {
                        if (!existKeHoachPhong)
                        {
                            _sqlContext.KeHoachKiemTra.Add(
                                new Models.Entities.KeHoachKiemTra
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    IdKeHoachCha = keHoachCuc.Id,
                                    CapKeHoach = capKeHoach, 
                                    IdDonVi = item.IdDonVi,
                                    SoQuyetDinhBanHanh = keHoachCuc.SoQuyetDinhBanHanh,
                                    NoiDungKiemTra = null,//keHoachCuc.NoiDungKiemTra,
                                    CanCu = null,//keHoachCuc.CanCu,
                                    MucDich = null,//keHoachCuc.MucDich,
                                    YeuCau = null,// keHoachCuc.YeuCau,
                                    TuNgayThucHienKeHoach = keHoachCuc.TuNgayThucHienKeHoach,
                                    DenNgayThucHienKeHoach = keHoachCuc.DenNgayThucHienKeHoach,
                                    TrangThaiKeHoachKiemTra = trangThaiPhanGiao,
                                    CreateBy = currentUser.UserId,
                                    CreateDate = DateTime.Now
                                });
                        }
                    }
                }
            }

            await _sqlContext.SaveChangesAsync();
            return keHoachCuc.Id;
        }



    }
}
