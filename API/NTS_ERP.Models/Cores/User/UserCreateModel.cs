using NTS_ERP.Models.Cores.GroupFunction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NTS_ERP.Models.Cores.User
{
    public class UserCreateModel
    {
        public string? Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageLink { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        [Required]
        public bool LockoutEnabled { get; set; }

        public string? Description { get; set; }
        [Required]
        public string UserGroupId { get; set; }
        public string? NameGroupUser { get; set; }
        [Required]
        public string IdDonVi { get; set; }
        /// <summary>
        /// Danh sách nhóm chức năng
        /// </summary>
        public List<GroupFunctionModel>? ListGroupFunction { get; set; }
        public UserCreateModel()
        {
            ListGroupFunction = new List<GroupFunctionModel>();
        }
    }
}
