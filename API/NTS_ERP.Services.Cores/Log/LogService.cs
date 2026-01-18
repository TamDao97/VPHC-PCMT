using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Models.Entities;
using System;
using Wangkanai.Detection.Services;

namespace NTS_ERP.Services.Cores.Log
{
    public static class LogService
    {
        /// <summary>
        /// Log đang nhập
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>
        public static void Login(UserHistoryModel userHistory, IDetectionService _detection)
        {
            NTS_ERPContext db = new NTS_ERPContext();

            Nts_UserHistory model = new Nts_UserHistory();
            model.UserId = userHistory.UserId;
            model.CreateDate = DateTime.Now;
            model.Content = "Đăng nhập hệ thống";
            model.OS = userHistory.OS;
            model.Device = _detection.Device.Type.ToString();
            model.ClientIP = userHistory.ClientIP;
            model.BrowserName = _detection.Browser.Name.ToString();
            model.BrowserVersion = _detection.Browser.Version.ToString();
            db.Nts_UserHistory.Add(model);
            db.SaveChanges();
        }

        /// <summary>
        /// Log đăng xuất 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>
        public static void Logout(UserHistoryModel userHistory, IDetectionService _detection)
        {
            NTS_ERPContext db = new NTS_ERPContext();

            Nts_UserHistory model = new Nts_UserHistory();
            model.UserId = userHistory.UserId;
            model.CreateDate = DateTime.Now;
            model.Content = "Đăng xuất hệ thống";
            model.OS = userHistory.OS;
            model.Device = _detection.Device.Type.ToString();
            model.ClientIP = userHistory.ClientIP;
            model.BrowserName = _detection.Browser.Name.ToString();
            model.BrowserVersion = _detection.Browser.Version.ToString();
            db.Nts_UserHistory.Add(model);
            db.SaveChanges();
        }

        /// <summary>
        /// Log thao tác dữ liệu
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>

        public static void Event(NTS_ERPContext sqlContext, Microsoft.AspNetCore.Http.HttpRequest request, string content, IDetectionService _detection, string userId)
        {
            try
            {
                UserHistoryModel userHistory = new UserHistoryModel();
                userHistory = GetUserLogHistory(request, userId);

                Nts_UserHistory model = new Nts_UserHistory
                {
                    UserId = userHistory.UserId,
                    CreateDate = DateTime.Now,
                    Content = content,
                    OS = userHistory.OS,
                    Device = _detection.Device.Type.ToString(),
                    ClientIP = userHistory.ClientIP,
                    BrowserName = _detection.Browser.Name.ToString(),
                    BrowserVersion = _detection.Browser.Version.ToString()
                };
                sqlContext.Nts_UserHistory.Add(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserHistoryModel GetUserLogHistory(Microsoft.AspNetCore.Http.HttpRequest request, string userId)
        {

            UserHistoryModel model = new UserHistoryModel();

            model.ClientIP = request.HttpContext.Connection.RemoteIpAddress.ToString();
            model.OS = GetUserData(request);
            model.UserId = userId;
            return model;
        }

        public static string GetUserData(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var platform = GetUserPlatform(request);
            return platform;
            //return string.Format("{0} {1} / {2}", browser.Browser, browser.Version, platform);
        }

        public static string GetUserPlatform(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var ua = request.Headers["User-Agent"];

            if (ua[0].Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua[0], "Android"));

            if (ua[0].Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua[0], "OS"));

            if (ua[0].Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua[0], "OS"));

            if (ua[0].Contains("Linux") && ua[0].Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua[0].Contains("RIM Tablet") || (ua[0].Contains("BB") && ua[0].Contains("Mobile")))
                return "Black Berry";

            if (ua[0].Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua[0], "Windows Phone"));

            if (ua[0].Contains("Mac OS"))
                return "Mac OS";

            if (ua[0].Contains("Windows NT 5.1") || ua[0].Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua[0].Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua[0].Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua[0].Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua[0].Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua[0].Contains("Windows NT 10"))
                return "Windows 10";

            if (ua[0].Contains("Windows NT 11"))
                return "Windows 11";

            //fallback to basic platform:
            return null;
        }

        public static string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
    }
}
