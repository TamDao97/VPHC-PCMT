using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using NTS.Common.Resource;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace NTS.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert string yyyy-MM-dd to dd/MM/yyyy
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>dd/MM/yyyy</returns>
        public static string ConvertDateYMDToDMY(this string stringValue)
        {
            try
            {
                DateTime datetTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                return datetTime.ToString("dd/MM/yyyy");
            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Convert date to string viet nam
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>string viet nam</returns>
        public static string ConvertDateVietNam(this DateTime? date)
        {
            if (date.HasValue)
            {
                return string.Format("Ngày {0} tháng {1} năm {2}", date.Value.Day.ToString("00"), date.Value.Month.ToString("00"), date.Value.Year);
            }
            else
            {
                return string.Format("Ngày  tháng  năm {0}", DateTime.Now.Year);
            }

        }
        public static string ConvertDateVietNam(this DateTime date)
        {
            return string.Format("Ngày {0} tháng {1} năm {2}", date.Day.ToString("00"), date.Month.ToString("00"), date.Year);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static int DateYMDDiffNowToDay(this string stringValue)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                if (DateTime.Now.Date <= dateTime.Date)
                {
                    return (dateTime - DateTime.Now).Days;
                }

                return 0;

            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        public static string NTSTrim(this string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return stringValue;
            }

            return stringValue.Trim();
        }

        public static string RemoveVietnameseString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;
        }

        public static string DoubleFomatString(this double value, bool isround = false)
        {
            if (isround)
            {
                value = Math.Round(value, 3);
            }
            string[] values = value.ToString().Split(new char[] { ',', '.' });
            if (values.Length == 1)
                return double.Parse(values[0]).ToString("N0", CultureInfo.GetCultureInfo("vn")).Replace(",", ".");
            else if (values.Length == 2)
            {
                return double.Parse(values[0]).ToString("N0", CultureInfo.GetCultureInfo("vn")).Replace(",", ".") + $",{values[1]}";
            }
            return "0";
        }

        public static string DecimalFomatString(this decimal value)
        {
            string[] values = value.ToString().Split(new char[] { ',', '.' });
            if (values.Length == 1)
                return decimal.Parse(values[0]).ToString("N0", CultureInfo.GetCultureInfo("vn")).Replace(",", ".");
            else if (values.Length == 2)
            {
                return decimal.Parse(values[0]).ToString("N0", CultureInfo.GetCultureInfo("vn")).Replace(",", ".") + $",{values[1]}";
            }
            return "0";
        }

        public static string NumberFormatString(this int value)
        {
            return value > 1000 ? string.Format("{0:#,##0}", value).Replace(",", ".") : value.ToString();
        }

        public static string NTSToStringCurrency(this int stringValue)
        {
            return stringValue > 1000 ? string.Format("{0:#,##0}", stringValue).Replace(",", ".") : stringValue.ToString();
        }

        public static string NTSToStringCurrency(this decimal stringValue)
        {
            return stringValue > 1000 ? string.Format("{0:#,##0}", stringValue).Replace(",", ".") : stringValue.ToString();
        }

        public static T JsonToObject<T>(this string stringJson)
        {
            T resultObject = default(T);
            resultObject = Activator.CreateInstance<T>();
            try
            {
                if (!string.IsNullOrEmpty(stringJson))
                {
                    resultObject = JsonConvert.DeserializeObject<T>(stringJson);
                }
                return resultObject;
            }
            catch (Exception ex)
            {
                return resultObject;
            }

        }

        /// <summary>
        /// Danh xưng Ông, Bà
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string ToStringGrandparents(this int value)
        {
            return value == 1 ? "Ông" : value == 2 ? "Bà" : "";
        }

        /// <summary>
        /// Giới tính
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string ToStringGender(this int value)
        {
            return value == 1 ? "Nam" : value == 2 ? "Nữ" : "Khác";
        }

        private static readonly string[] VietnameseSigns = new string[]{

            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string NumberToCurrencyTextVN(this decimal inputNumber, bool suffix = false)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber != 0 ? inputNumber.ToString("#") : "0";
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (string.IsNullOrEmpty(result))
                result = unitNumbers[0];
            if (isNegative) result = "Âm " + result;
            result = suffix ? result + " đồng chẵn" : result + " đồng";
            result = result.Substring(0, 1).ToUpper() + result.Substring(1);
            return result;
        }

        /// <summary>
        /// Mã hóa chuỗi có mật khẩu
        /// </summary>
        /// <param name="key">Key mã hóa</param>
        /// <param name="toEncrypt">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi đã mã hóa</returns>
        public static string EnCode(string key, string toEncrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Giản mã
        /// </summary>
        /// <param name="key">key mã hóa</param>
        /// <param name="toDecrypt">Chuỗi đã mã hóa</param>
        /// <returns>Chuỗi giản mã</returns>
        public static string DeCode(string key, string toDecrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            try
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                throw NTSException.CreateInstance("Tệp cấp quyền Sửa dữ liệu hồ sơ không đúng!");
            }
        }

        /// <summary>
        /// Lọc dấu cho chuỗi string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string str)
        {
            string[] VietNamChar = new string[] {
                        "aAeEoOuUiIdDyY",
                        "áàạảãâấầậẩẫăắằặẳẵ",
                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                        "éèẹẻẽêếềệểễ",
                        "ÉÈẸẺẼÊẾỀỆỂỄ",
                        "óòọỏõôốồộổỗơớờợởỡ",
                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                        "úùụủũưứừựửữ",
                        "ÚÙỤỦŨƯỨỪỰỬỮ",
                        "íìịỉĩ",
                        "ÍÌỊỈĨ",
                        "đ",
                        "Đ",
                        "ýỳỵỷỹ",
                        "ÝỲỴỶỸ" };
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// Chuyển đổi string sang Slug
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string input)
        {
            // Xóa khoảng trắng và các ký tự đặc biệt
            string slug = Regex.Replace(input.RemoveDiacritics(), @"[^a-zA-Z0-9]", "-");

            // Loại bỏ các ký tự gạch ngang liên tiếp
            slug = Regex.Replace(slug, @"-+", "-");

            // Loại bỏ các ký tự gạch ngang ở đầu và cuối chuỗi
            slug = slug.Trim('-');

            // Chuyển đổi thành chữ thường
            slug = slug.ToLower();

            return slug;
        }
    }
}