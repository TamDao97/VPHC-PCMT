using NTS_ERP.Models.Cores.UserHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Log
{
    public interface ILogEventService
    {
        /// <summary>
        /// Lưu thao tác sử dụng hệ thống
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        void LogEventAsync(Microsoft.AspNetCore.Http.HttpRequest request, UserHistoryModel model, string userId);

        /// <summary>
        /// Lưu thao tác sử dụng hệ thống
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void LogEventDesktopAsync(UserHistoryModel model, string userId);
    }
}
