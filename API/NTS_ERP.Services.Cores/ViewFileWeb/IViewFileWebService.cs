using NTS.Common.Files;

namespace NTS_ERP.Services.Cores.ViewFileWeb
{
    public interface IViewFileWebService
    {
        /// <summary>
        /// Lấy thông tin view file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<FileResultModel> GetFileViewAsync(string path);
    }
}
