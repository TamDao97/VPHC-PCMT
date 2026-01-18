using NTS_ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.About;

namespace NTS_ERP.Services.Cores.Abouts
{
    public class AboutService : IAboutService
    {
        private readonly NTS_ERPContext _sqlContext;

        public AboutService(NTS_ERPContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        /// <summary>
        /// Lấy thông tin cấu hình
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AboutModel> GetAboutAsync()
        {
            var data = (from a in _sqlContext.Nts_About.AsNoTracking()
                        select new AboutModel()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content
                        }).FirstOrDefault();

            if (data == null)
            {
                data = new AboutModel()
                {
                    Content = ""
                };
            }

            return data;
        }

        /// <summary>
        /// Thêm mới giới thiệu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateAboutAsync(AboutModel model, string userId = null)
        {
            var abouts = _sqlContext.Nts_About.ToList();
            _sqlContext.Nts_About.RemoveRange(abouts);

            Nts_About about = new Nts_About()
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Content = model.Content
            };

            _sqlContext.Nts_About.Add(about);
            _sqlContext.SaveChanges();
        }
    }
}
