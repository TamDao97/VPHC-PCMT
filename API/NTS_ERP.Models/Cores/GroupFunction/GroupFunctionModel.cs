using NTS_ERP.Models.Cores.Function;
using System.Collections.Generic;

namespace NTS_ERP.Models.Cores.GroupFunction
{
    public class GroupFunctionModel
    {
        /// <summary>
        /// Id nhóm 
        /// </summary>
        public string? Id { get; set; }

        public int Index { get; set; }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Mã nhóm
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Số quyền
        /// </summary>
        public int PermissionTotal { get; set; }

        /// <summary>
        /// Đếm
        /// </summary>
        public int CheckCount { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Check
        /// </summary>
        public bool? IsChecked { get; set; }
        public string? ParentId { get; set; }
        public string? CountPermission { get; set; }


        /// <summary>
        /// Danh sách quyền
        /// </summary>
        public List<FunctionModel>? Permissions { set; get; }
        public List<GroupFunctionModel>? Children { set; get; }

        public GroupFunctionModel()
        {
            Permissions = new List<FunctionModel>();
        }
    }
}
