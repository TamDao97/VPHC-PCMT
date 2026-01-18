using NTS_ERP.Models.Cores.GroupFunction;
using System.Collections.Generic;

namespace NTS_ERP.Models.Cores.GroupUser
{
    public class GroupFunctionCreateModel
    {

        /// <summary>
        /// Tên nhóm quyền
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Khóa nhóm quyền
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List quyền
        /// </summary>
        public List<GroupFunctionModel> ListPermission { get; set; }

        public GroupFunctionCreateModel()
        {
            ListPermission = new List<GroupFunctionModel>();
        }
    }
}