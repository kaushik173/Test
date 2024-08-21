using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LALoDep.Core.Custom.Extensions
{
    public static class TypeExtension
    {
        public static string StripHTML(this string input)
        {
            if (input.IsNullOrEmpty())
                return "";
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string ToDataTableSortType(this Type type)
        {
            var retVal = "string";
            if (type == typeof(DateTime))
                retVal = "date";
            else if (type == typeof(Int32) && type == typeof(decimal))
                retVal = "numeric";
            return retVal;
        }
        public static string ToShortDescription(this object text, int length)
        {
            string data = "";

            string value = "";
            if (text == null)
                return "";
            else
                data = text.ToString();
            try
            {

                if (data.Length > length)
                {
                    value = data.Remove(length);

                }
                else
                    value = data;
            }
            catch
            {
                value = "";
            }

            return value;
        }
        public static string ToShortDescription(this object text, int length, string attachText)
        {

            string data = "";

            string value = "";
            if (text == null)
                return "";
            else
                data = text.ToString();
            try
            {

                if (data.Length > (length - attachText.Length))
                {
                    value = data.Remove((length - attachText.Length)) + attachText;

                }
                else
                    value = data;
            }
            catch
            {
                value = "";
            }

            return value;
        }
        public static bool StringStartsWith(this object text, string value, bool lowerCase = true)
        {
            if (text == null)
                return false;

            var flag = false;
            try
            {
                if (lowerCase)
                    flag = text.ToString().ToLower().StartsWith(value);
                else
                    flag = text.ToString().StartsWith(value);

            }
            catch
            {
                flag = false;
            }

            return flag;
        }

        public static bool IsNullOrEmpty(this string text)
        {

            bool flag = false;
            try
            {
                flag = string.IsNullOrEmpty(text);
            }
            catch
            {
                flag = false;
            }

            return flag;
        }

        public static bool ToBoolean<T>(this T t)
        {

            bool value = false;
            try
            {
                value = Convert.ToBoolean(t);
            }
            catch
            {
                value = false;
            }

            return value;
        }
        public static int ToInt<T>(this T t)
        {

            int value = 0;
            try
            {
                value = Convert.ToInt32(t);
            }
            catch
            {
                value = 0;
            }

            return value;
        }
        public static int? ToIntNullable<T>(this T t)
        {

            int? value = null;
            try
            {
                value = Convert.ToInt32(t);
            }
            catch
            {

            }

            return value;
        }
        public static int ToInt<T>(this int? t)
        {

            int value = 0;
            try
            {
                if (t.HasValue)
                    value = t.Value;
            }
            catch
            {
                value = 0;
            }

            return value;
        }
        public static decimal ToDecimal<T>(this T t)
        {

            decimal value = 0;
            try
            {
                value = Convert.ToDecimal(t);
            }
            catch
            {
                value = 0;
            }

            return value;
        }
        public static string ToDecrypt<T>(this T t)
        {

            var value = "";
            try
            {
                if (t != null)
                    value = CodeComet.Crypto.Encryption.Decrypt(t.ToString());
            }
            catch
            {
                value = "";
            }

            return value;
        }
        public static string ToEncrypt<T>(this T t)
        {

            var value = "";
            try
            {
                value = CodeComet.Crypto.Encryption.Encrypt(t.ToString());
            }
            catch
            {
                value = "";
            }

            return value;
        }

        public static DateTime ToDateTime<T>(this T t)
        {

            DateTime value = DateTime.MinValue;
            try
            {
                value = Convert.ToDateTime(t);
            }
            catch
            {
                value = DateTime.MinValue;
            }

            return value;
        }
        public static DateTime? ToDateTimeValue<T>(this T t)
        {

            DateTime? value = null;
            try
            {
                if (t != null)
                    value = Convert.ToDateTime(t);
            }
            catch
            {

            }

            return value;
        }
        public static string ToShortDateString(this DateTime? t)
        {


            try
            {
                if (t.HasValue)
                {
                    return t.Value.ToString("MM/dd/yyyy");
                }
            }
            catch
            {
                return "";
            }

            return "";
        }
        public static string ToDefaultFormat(this DateTime? t)
        {

            if (!t.HasValue)
                return "";
            return t.Value.ToString("MM/dd/yyyy");


        }
        public static string ToDefaultFormat(this DateTime? t, string format)
        {

            if (!t.HasValue)
                return "";
            return t.Value.ToString(format);


        }
        public static DateTime? ToDateTimeNullableValue(this string t)
        {

            DateTime? value = null;
            try
            {
                if (!t.IsNullOrEmpty())
                    value = Convert.ToDateTime(t);
            }
            catch
            {

            }

            return value;
        }
        public static string ToPhoneFormat(this string text)
        {


            string rt = "";


            try
            {
                rt = text;

                if (text.Length > 0 && !text.Contains("("))
                {
                    rt += '(';
                    int n = 1;
                    foreach (char c in text)
                    {
                        rt += c;
                        if (n == 3) { rt += ") "; }
                        else if (n == 6) { rt += "-"; }
                        n++;
                    }
                }
            }
            catch { }

            return rt;
        }


        public static string ToRemoveSplChars(this string text)
        {
            string asAscii = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8,
                    Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback("-"), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(text)));
            //StringBuilder sb = new StringBuilder();
            //foreach (char c in text)
            //{
            //    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
            //    {
            //        sb.Append(c);
            //    }else
            //    {
            //        sb.Append("_");
            //    }
            //}
            //return sb.ToString();
            return asAscii;
        }

        public static T GetPropertyValue<T>(this object obj, string propName)
        {
            var value = obj.GetType().GetProperty(propName).GetValue(obj);
            return (T)value;
        }

        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public static string ToCurrency(this decimal? value)
        {


            string val = "";


            try
            {
                if (value.HasValue)
                    val = value.Value.ToString("C");
            }
            catch { }

            return val;
        }
        public static string ToCellString(this object t)
        {

            string value = "";
            try
            {

                value = t != DBNull.Value ? t.ToString() : "";
            }
            catch
            {

            }

            return value;
        }
        public static string ToSerialize<T>(this T tData)
        {
            return JsonConvert.SerializeObject(tData);
        }
    }
}