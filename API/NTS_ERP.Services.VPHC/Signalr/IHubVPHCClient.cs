using System.Threading.Tasks;

namespace NTS_ERP.Services.VPHC.Signalr
{
    public interface IHubVPHCClient
    {
        Task NotifyVuViec(string vuViecId,string donVi, string noiDung);
    }
}