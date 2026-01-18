namespace NTS_ERP.Models.Cores.Auth
{

    public class NtsLogInModel
    {
        /// <summary>
        /// Tài khoản
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }

        public bool IsRemember { get; set; }
       
    }
}
