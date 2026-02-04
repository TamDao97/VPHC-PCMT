using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;
using NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Services.VPHC.KeHoachKiemTraPhong
{
    public interface IKeHoachKiemTraPhongService
    {
        /// <summary>
        /// Tim kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<KeHoachKiemTraBaseResultModel> SearchAsync(KeHoachKiemTraSearchModel searchModel);

        /// <summary>
        /// Tìm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KeHoachKiemTraDetailResponseModel> GetByIdAsnyc(string id);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<string> UpdateAsync(KeHoachKiemTraUpdateRequestModel requestModel, CurrentUserModel currentUser);

        /// <summary>
        /// Xóa 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEntityAsync(string id, string userid, bool isSoftDelete = true);


        /// <summary>
        /// Duyệt kế hoạch kiểm tra
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<string> UpdateStatusAsync(KeHoachKiemTraUpdateStatusModel requestModel, CurrentUserModel currentUser);

        /// <summary>
        /// Phân giao kế hoặc đến các phòng
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<string> AssigneeTaskAsync(KeHoachKiemTraPhanGiaoCreateRequestModel requestModel, CurrentUserModel currentUser);

        /// <summary>
        /// Chi tiết phân giao kế hoạch
        /// </summary>
        /// <param name="idKeHoach"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<List<KeHoachKiemTraPhanGiaoModel>> GetDetailAssigneeTaskByIdKeHoachAsync(string idKeHoach, CurrentUserModel currentUser);
    }
}
