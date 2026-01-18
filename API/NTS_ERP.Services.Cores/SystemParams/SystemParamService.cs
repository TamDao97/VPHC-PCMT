using NTS_ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System.Linq;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.SystemParams;

namespace NTS_ERP.Services.Cores.SystemParams
{
    public class SystemParamService : ISystemParamService
    {
        private readonly NTS_ERPContext _sqlContext;
        public SystemParamService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;

        }
        public async Task<SystemParamInfoModel> GetSystemParamInfoAsync(string paramName)
        {
            // Lấy thông tin user
            var result = (from a in _sqlContext.Nts_SystemParams.AsNoTracking()
                               where a.ParamName.Equals(paramName)
                               select new SystemParamInfoModel()
                               {
                                   ParamName = a.ParamName,
                                   ParamValue = a.ParamValue
                               }).FirstOrDefault();

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0032);
            }

            return result;
        }
    }
}
