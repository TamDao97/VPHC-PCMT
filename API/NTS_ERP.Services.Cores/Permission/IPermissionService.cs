using NTS_ERP.Models.Cores.Permission;
using NTS_ERP.Models.Cores.Common;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.ConfigInterface
{
    public interface IPermissionService
    {
        Task<SearchBaseResultModel<UserPermissionModel>> GetUserInfoAsync(PermissionSearchModel modelSearch);
        Task DeletePermissionAsync(string userid);
    }
}
