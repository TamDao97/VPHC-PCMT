using NTS_ERP.Models.Cores.About;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Abouts
{
    public interface IAboutService
    {
        /// <summary>
        /// Lấy thông tin cấu hình
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AboutModel> GetAboutAsync();

        /// <summary>
        /// Thêm mới giới thiệu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateAboutAsync(AboutModel model, string userId = null);
    }
}
