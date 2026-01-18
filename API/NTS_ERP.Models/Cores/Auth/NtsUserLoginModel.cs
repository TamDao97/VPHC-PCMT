using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.Auth
{
    public class NtsUserLoginModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? UserId { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PasswordHash { get; set; }
        public string? Secret { get; set; }
        public int ExpireDateAfter { get; set; }
        public string? Password { get; set; }
        public List<string>? Permission { get; set; }
        public string? IdDonVi { get; set; }
    }
}
