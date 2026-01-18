using NTS_ERP.Models.Cores.SystemParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.SystemParams
{
    public interface ISystemParamService
    {
        Task<SystemParamInfoModel> GetSystemParamInfoAsync(string paramName);
    }
}
