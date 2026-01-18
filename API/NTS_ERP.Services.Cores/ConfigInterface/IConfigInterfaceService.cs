using NTS_ERP.Models.Cores.ConfigInterface;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.ConfigInterface
{
    public interface IConfigInterfaceService
    {
        Task<ConfigInterfaceModel> GetConfigAsync();
        Task CreateOrUpdateAsync(ConfigInterfaceModel model);
    }
}
