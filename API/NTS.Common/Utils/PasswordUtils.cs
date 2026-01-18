using System;
using System.Security.Cryptography;

namespace NTS.Common.Utils
{
    public static class PasswordUtils
    {
        /// <summary>
        /// Tạo chuỗi bảo mật
        /// </summary>
        /// <returns></returns>
        public static string CreateSecurityStamp()
        {
            string securityStamp = string.Empty;
            Random random = new Random();
            while (securityStamp.Length < 32)
            {
                securityStamp = securityStamp + Convert.ToString(random.Next(0, 9));
            }
            return securityStamp;
        }

        /// <summary>
        /// Hàm băm mật khẩu
        /// </summary>
        /// <param name="target">chuỗi Password + SecurityStamp</param>
        /// <returns></returns>
        public static string ComputeHash(string target)
        {
            SHA256Managed hashAlgorithm = new SHA256Managed();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(target);

            byte[] bytes = hashAlgorithm.ComputeHash(data);

            return BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);
        }
    }
}
