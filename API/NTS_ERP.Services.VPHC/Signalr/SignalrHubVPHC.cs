using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NTS_ERP.Services.VPHC.Signalr
{
    public class SignalrHubVPHC : Hub<IHubVPHCClient>
    {
        public async Task NotifyVuViec(string vuViecId, string donVi, string noiDung)
        {
            await Clients.All.NotifyVuViec(vuViecId, donVi, noiDung);
        }
    }
}