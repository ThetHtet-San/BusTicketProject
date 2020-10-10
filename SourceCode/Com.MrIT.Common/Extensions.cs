using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.MrIT.Common
{
    public static class Extensions
    {
        public static string StripHTML(this string input)
        {
            input = Regex.Replace(input, @"&lt;.+?&gt;|&nbsp;", " ");
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty(this DateTime value)
        {
            return (value == null);
        }

        public static string ToApplicationShortDateTimeFormat(this DateTime value)
        {
            if (value.IsNullOrEmpty()) return null;

            return value.ToString("dd/MM/yyyy");
        }

        public static string ToShortString(this Guid value)
        {
            return Convert.ToBase64String(value.ToByteArray());
        }

        public static string ToApplicationLongDateTimeFormat(this DateTime value)
        {
            if (value.IsNullOrEmpty()) return null;

            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string EmptyIfNull(this string str)
        {
            if (str == null) return string.Empty;

            return str;
        }
    }
}
