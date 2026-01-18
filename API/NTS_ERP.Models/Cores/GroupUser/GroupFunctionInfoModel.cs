using NTS_ERP.Models.Cores.GroupFunction;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.GroupUser
{
    public class GroupFunctionInfoModel
    {
        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên nhóm quyền
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Trạng thái
        /// 1: Đang sử dụng
        /// 2: Không sử dụng
        /// </summary>
        public bool? IsDisable { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        public List<GroupFunctionModel> ListGroupFunction { get; set; }

        public GroupFunctionInfoModel()
        {
            ListGroupFunction = new List<GroupFunctionModel>();
        }
    }
}
