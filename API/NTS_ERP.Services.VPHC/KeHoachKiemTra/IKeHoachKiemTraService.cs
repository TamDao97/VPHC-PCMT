using NTS_ERP.Models.Cores.Auth;
using NTS_ERP.Models.VPHC.KeHoachKiemTra;

namespace NTS_ERP.Services.VPHC.KeHoachKiemTra
{
    public interface IKeHoachKiemTraService
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
        /// Thêm mới
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<string> CreateAsync(KeHoachKiemTraCreateRequestModel requestModel, CurrentUserModel currentUser);

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
    }
}
