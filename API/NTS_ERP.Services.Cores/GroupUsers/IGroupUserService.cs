using NTS_ERP.Models.Cores.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.GroupUser;

namespace NTS_ERP.Services.Cores.GroupUsers
{
    public interface IGroupUserService
    {
        /// <summary>
        /// Tìm kiếm nhóm người dùng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<GroupUserResultModel>> SearchGroupUser(GroupUserSearchModel modelSearch);

        /// <summary>
        /// Thêm mới nhóm người dùng
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task CreateGroupUser(GroupFunctionCreateModel model, string userId);

        /// <summary>
        /// Xóa nhóm người dùng
        /// </summary>
        /// <param name="id">Id bản ghi</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task DeleteGroupUserById(string id, string userId);

        /// <summary>
        /// lấy dữ liệu nhóm người dùng
        /// </summary>
        /// <param name="id">Id bản ghi</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task<GroupUserModel> GetGroupUserById(string id);

        /// <summary>
        /// Cập nhật nhóm người dùng
        /// </summary>
        /// <param name="id">id bản ghi</param>
        /// <param name="model">dữ liệu update</param>
        /// <param name="userInfo">thông tin người dùng đăng nhập</param>
        /// <returns></returns>
        Task UpdateGroupUser(string id, GroupFunctionCreateModel model, string updateby);
        List<PermissionModel> GetListGroupPermissions(List<string> lstCheck);

    }
}
