using Microsoft.AspNetCore.Http;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.FileUpload;

namespace NTS_ERP.Services.Cores.FileUpload
{
    public interface IFileTemplateService
    {
        /// <summary>
        /// Search biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<TemplateSearchResultModel>> SearchFileTemplate(TemplateSearchModel model);


        /// <summary>
        /// xem biểu mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TemplateSearchResultModel> GetFileTemplateById(string id);

        /// <summary>
        /// Thêm mới biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateFileTemplate(string userId, FileUploadCreateModel model);

        /// <summary>
        /// Update biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateFileTemplate(string userId, string id, FileUploadCreateModel model);
    }
}
