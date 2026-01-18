using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.VPHC.NguoiVP;
using NTS_ERP.Models.VPHC.CanBo;
using NTS_ERP.Models.VPHC.CanBo;
using System.Drawing;
using TrafficControl.Core;

namespace NTS_ERP.Services.VPHC.CanBo
{
    public class CanBoService : ICanBoService
    {

        private readonly NTS_ERPContext _sqlContext;

        public CanBoService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }
        /// <summary>
        /// Lấy bộ theo id đơn vị
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CanBoChooseModel>> GetByIdDonVi(string id)
        {
            var listCanBo = (from a in _sqlContext.CanBo.AsNoTracking().Where(u => u.IdDonVi.Equals(id))
                             join b in _sqlContext.CapBac.AsNoTracking() on a.IdCapBac equals b.Id
                             join c in _sqlContext.ChucVu.AsNoTracking() on a.IdChucVu equals c.Id
                             join d in _sqlContext.DonVi.AsNoTracking() on a.IdDonVi equals d.IdDonVi
                             select new CanBoChooseModel()
                             {
                                 Id = a.IdCanBo,
                                 Ten = a.HoVaTen,
                                 ChucVu = c.Name,
                                 CapBac = b.Name,
                                 GioiTinh = a.GioiTinh == 1 ? "Nam" : a.GioiTinh == 2 ? "Nữ" : "Khác",
                                 DonVi = d.Ten
                             }).ToList();

            return listCanBo;
        }
    }
}
