using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Document.Excel;
using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.CanBo;
using NTS_ERP.Models.VPHC.VuViec;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.VPHC.ChungChiGiayPhep;
using NTS_ERP.Services.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhuongTien;
using NTS_ERP.Services.VPHC.TangVat;
using NTS_ERP.Services.VPHC.ToChucVP;

namespace NTS_ERP.Services.VPHC.VuViecVPHC
{
    public class VuViecXuLyService : IVuViecXuLyService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IExcelService _excelService;
        private IComboboxService _comboboxService;
        private readonly INguoiVPService _nguoiVPService;
        private readonly IToChucVPService _toChucVPService;
        private readonly IPhienDichService _phienDichService;
        private readonly ITangVatService _tangVatService;
        private readonly IPhuongTienService _phuongTienService;
        private readonly INguoiChungKienService _chungKienService;
        private readonly IChungChiGiayPhepService _chungChiGiayPhep;
        public VuViecXuLyService(NTS_ERPContext sqlContext, IExcelService excelService, IComboboxService comboboxService, INguoiVPService nguoiVPService, IToChucVPService toChucVPService,
            ITangVatService tangVatService, IPhienDichService phienDichService, IPhuongTienService phuongTienService, INguoiChungKienService chungKienService,
            IChungChiGiayPhepService chungChiGiayPhep
            )
        {
            this._sqlContext = sqlContext;
            this._excelService = excelService;
            this._comboboxService = comboboxService;
            this._nguoiVPService = nguoiVPService;
            this._toChucVPService = toChucVPService;
            this._phienDichService = phienDichService;
            this._tangVatService = tangVatService;
            this._phuongTienService = phuongTienService;
            this._chungKienService = chungKienService;
            this._chungChiGiayPhep = chungChiGiayPhep;
        }

        /// <summary>
        /// Cập nhật thông tin vụ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, VuViecXuLyModifyModel model, CurrentUserModel currentUser)
        {
            var vuViecUpdate = _sqlContext.VuViecVPHC.FirstOrDefault(i => i.Id.Equals(id));

            if (vuViecUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }
            vuViecUpdate.IdXuLy = model.IdXuLy;
            vuViecUpdate.DonViKhacXuLy = model.DonViKhacXuLy;
            vuViecUpdate.SoBienBanDVKhac = model.SoBienBanDVKhac;
            vuViecUpdate.NgayBanGiaoDVKhac = model.NgayBanGiaoDVKhac;
            vuViecUpdate.DonViTiepNhanHS = model.DonViTiepNhanHS;
            vuViecUpdate.SoBienBanHS = model.SoBienBanHS;
            vuViecUpdate.NgayBanGiaoHS = model.NgayBanGiaoHS;
            vuViecUpdate.TaiLieuChuyenGiao = model.TaiLieuChuyenGiao;
            vuViecUpdate.NoiDungNhacNho = model.NoiDungNhacNho;
            vuViecUpdate.LyDoKhongQDXP = model.LyDoKhongQDXP;
            vuViecUpdate.TongQDXuPhat = model.TongQDXuPhat;
            vuViecUpdate.TongQDThiHanh = model.TongQDThiHanh;
            vuViecUpdate.TongQDKhieuKien = model.TongQDKhieuKien;
            vuViecUpdate.TongQDChuyen = model.TongQDChuyen;
            vuViecUpdate.TongQDDangXuLy = model.TongQDDangXuLy;
            vuViecUpdate.TongQDMienGiam = model.TongQDMienGiam;
            vuViecUpdate.TongQDCuongChe = model.TongQDCuongChe;
            vuViecUpdate.UpdateBy = currentUser.UserId;
            vuViecUpdate.UpdateDate = DateTime.Now;
            if (vuViecUpdate.TienTrinhHoSo <= 2)
            {
                vuViecUpdate.TienTrinhHoSo = 3;
            }

            await this._tangVatService.AddXuLy(_sqlContext, model.ListTangVat, vuViecUpdate.Id, currentUser.UserId);

            await this._phuongTienService.AddXuLy(_sqlContext, model.ListPhuongTien, vuViecUpdate.Id, currentUser.UserId);

            await this._chungChiGiayPhep.AddXuLy(_sqlContext, model.ListGiayPhepChungChi, vuViecUpdate.Id, currentUser.UserId);

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
        /// Lấy thông tin vụ việc
        /// </summary>
        /// <param name="VuViecId"></param>
        /// <returns></returns>
        public async Task<VuViecXuLyModifyModel> GetByIdAsnyc(string id)
        {
            var vuViec = _sqlContext.VuViecVPHC.Where(r => r.Id.Equals(id)).FirstOrDefault();

            if (vuViec == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            VuViecXuLyModifyModel outputModel = JsonConvert.DeserializeObject<VuViecXuLyModifyModel>(JsonConvert.SerializeObject(vuViec, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            outputModel.ListTangVat = this._tangVatService.GetTangVat(_sqlContext, id);

            outputModel.ListPhuongTien = this._phuongTienService.GetPhuongTien(_sqlContext, id);

            outputModel.ListGiayPhepChungChi = this._chungChiGiayPhep.GetChungChiGiayPhep(_sqlContext, id);

            return outputModel;
        }
    }
}
