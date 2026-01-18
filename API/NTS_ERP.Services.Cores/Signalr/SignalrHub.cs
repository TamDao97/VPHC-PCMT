using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Signalr
{
    public class SignalrHub : Hub<IHubClient>
    {
        public async Task DashBoard()
        {
            await Clients.All.DashBoard();
        }
    }
}