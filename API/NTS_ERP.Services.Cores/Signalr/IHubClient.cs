using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Signalr
{
    public interface IHubClient
    {
        //Task BroadcastMessage(MessageInstance msg);
        Task DashBoard();
    }
}