using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
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
    public class VuViecXacMinhService : IVuViecXacMinhService
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
        public VuViecXacMinhService(NTS_ERPContext sqlContext, IExcelService excelService, IComboboxService comboboxService, INguoiVPService nguoiVPService, IToChucVPService toChucVPService,
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
        public async Task UpdateAsync(string id, VuViecXacMinhModifyModel model, CurrentUserModel currentUser)
        {
            var vuViecUpdate = _sqlContext.VuViecVPHC.FirstOrDefault(i => i.Id.Equals(id));

            if (vuViecUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            if (model.ListCanBoPhanCong != null && model.ListCanBoPhanCong.Count > 0)
            {
                vuViecUpdate.CanBoDieuTra = JsonConvert.SerializeObject(model.ListCanBoPhanCong);
            }
            else
            {
                vuViecUpdate.CanBoDieuTra = "";
            }
            vuViecUpdate.ThietHai = model.ThietHai;
            vuViecUpdate.MucDoThietHai = model.MucDoThietHai;
            vuViecUpdate.GiamNhe = model.GiamNhe;
            vuViecUpdate.TangNang = model.TangNang;
            vuViecUpdate.YKienNguoiViPham = model.YKienNguoiViPham;
            vuViecUpdate.YKienNguoiLamChung = model.YKienNguoiLamChung;
            vuViecUpdate.YKienNguoiBiThietHai = model.YKienNguoiBiThietHai;
            vuViecUpdate.XacMinhKhac = model.XacMinhKhac;
            vuViecUpdate.NguoiUyQuyen = model.NguoiUyQuyen;
            vuViecUpdate.SoQDUyQuyen = model.SoQDUyQuyen;
            vuViecUpdate.NgayUyQuyen = model.NgayUyQuyen;
            vuViecUpdate.UpdateBy = currentUser.UserId;
            vuViecUpdate.UpdateDate = DateTime.Now;
            if(vuViecUpdate.TienTrinhHoSo <= 1)
            {
                vuViecUpdate.TienTrinhHoSo = 2;
            }    

            await this._nguoiVPService.AddXacMinh(_sqlContext, model.ListNguoiVP, vuViecUpdate.Id, currentUser);

            await this._toChucVPService.AddXacMinh(_sqlContext, model.ListToChucVP, vuViecUpdate.Id, currentUser.UserId);

            await this._phienDichService.Add(_sqlContext, model.ListPhienDich, vuViecUpdate.Id, currentUser.UserId);

            await this._chungKienService.Add(_sqlContext, model.ListChungKien, vuViecUpdate.Id, currentUser.UserId);

            await this._tangVatService.Add(_sqlContext, model.ListTangVat, vuViecUpdate.Id, currentUser.UserId);

            await this._phuongTienService.Add(_sqlContext, model.ListPhuongTien, vuViecUpdate.Id, currentUser.UserId);

            await this._chungChiGiayPhep.Add(_sqlContext, model.ListGiayPhepChungChi, vuViecUpdate.Id, currentUser.UserId);

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
        public async Task<VuViecXacMinhModifyModel> GetByIdAsnyc(string id)
        {
            var vuViec = _sqlContext.VuViecVPHC.Where(r => r.Id.Equals(id)).FirstOrDefault();

            if (vuViec == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            VuViecXacMinhModifyModel outputModel = JsonConvert.DeserializeObject<VuViecXacMinhModifyModel>(JsonConvert.SerializeObject(vuViec, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

            if (outputModel!=null && !string.IsNullOrEmpty(outputModel.CanBoDieuTra))
            {
                outputModel.ListCanBoPhanCong = JsonConvert.DeserializeObject<List<CanBoChooseModel>>(outputModel.CanBoDieuTra).UpdateIndex();
            }

            outputModel.ListNguoiVP = this._nguoiVPService.GetNguoiVP(_sqlContext, id);

            outputModel.ListToChucVP = this._toChucVPService.GetToChucVP(_sqlContext, id);

            outputModel.ListPhienDich = this._phienDichService.GetPhienDich(_sqlContext, id);

            outputModel.ListChungKien = this._chungKienService.GetNguoiChungKien(_sqlContext, id);

            outputModel.ListTangVat = this._tangVatService.GetTangVat(_sqlContext, id);

            outputModel.ListPhuongTien = this._phuongTienService.GetPhuongTien(_sqlContext, id);

            outputModel.ListGiayPhepChungChi = this._chungChiGiayPhep.GetChungChiGiayPhep(_sqlContext, id);

            return outputModel;
        }
    }
}
