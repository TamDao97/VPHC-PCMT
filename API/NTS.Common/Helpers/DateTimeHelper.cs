using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Common
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Convert datetime to HH:mm dd/MM/yyyy
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringHHMMDDMMYY(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("HH:mm dd/MM/yyyy");
        }

        /// <summary>
        /// Convert datetime to HH:mm
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringHHMM(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("HH:mm");
        }

        /// <summary>
        /// Convert datetime to dd/MM/yyyy
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringDDMMYY(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Convert datetime to yyyy-MM-dd
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringYYMMDD(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Convert datetime to dd
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringDD(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("dd");
        }

        /// <summary>
        /// Convert datetime to MM
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringMM(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("MM");
        }

        /// <summary>
        /// Convert datetime to yyyy
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringYYYY(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("yyyy");
        }

        /// <summary>
        /// Convert datetime to HH
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringHH(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("HH");
        }

        /// <summary>
        /// Convert datetime to mm
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringmm(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("mm");
        }

        /// <summary>
        /// Convert datetime to yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringFullDateTime(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// Convert datetime về cuối ngày
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static DateTime ToEndDate(this DateTime dateTimeValue)
        {
            return DateTime.ParseExact(dateTimeValue.ToStringDDMMYY() + " 23:59:59.999", "dd/MM/yyyy HH:mm:ss.fff", null);
        }

        /// <summary>
        /// Convert datetime về đầu ngày
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static DateTime ToStartDate(this DateTime dateTimeValue)
        {
            return DateTime.ParseExact(dateTimeValue.ToStringDDMMYY() + " 00:00:00.000", $"dd/MM/yyyy HH:mm:ss.fff", null);
        }

        /// <summary>
        /// Convert datetime về cuối ngày
        /// </summary>
        /// <param name="dateTimeValue">dd/MM/yyyy</param>
        /// <returns></returns>
        public static DateTime? ToEndDate(this string dateValue,string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.ParseExact(dateValue + " 23:59:59.999", $"{dateFormat} HH:mm:ss.fff", null);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert datetime về đầu ngày
        /// </summary>
        /// <param name="dateTimeValue">dd/MM/yyyy</param>
        /// <returns></returns>
        public static DateTime? ToStartDate(this string dateValue, string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.ParseExact(dateValue + " 00:00:00.000", $"{dateFormat} HH:mm:ss.fff", null);
            }
            catch
            {
                return null;
            }
        }

        public static bool BetweeenDate(this DateTime dateTime)
        {
            DateTime startDate = new DateTime(1753, 01, 01);
            DateTime endDate = new DateTime(9999, 12, 31);

            if (dateTime < startDate || dateTime > endDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}