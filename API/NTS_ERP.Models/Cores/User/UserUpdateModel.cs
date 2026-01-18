using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.User
{
    public class UserUpdateModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageLink { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
    }
}
