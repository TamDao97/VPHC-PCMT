using NTS_ERP.Models.Cores.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.User
{
    public class UserInfoModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string? IdDonVi { get; set; }
        public List<string>? Permission { get; set; }
        public List<MenuViewModel>? MenuView { get; set; }
        /// <summary>
        /// Đơn vị
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// Phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? WardId { get; set; }
        
    }
}
