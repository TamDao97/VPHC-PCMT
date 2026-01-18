using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.Auth
{
    public class NtsUserTokenModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public int ExpireDateAfter { get; set; }
        public string RefreshToken { get; set; }
        public int Type { get; set; }
        public int? TinhId { get; set; }
        public int? HuyenId { get; set; }
        public int? XaId { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public string? IdDonVi { get; set; }
    }
}
