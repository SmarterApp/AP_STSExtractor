using System.Text;
using System.Text.RegularExpressions;

namespace STSCommon.Utilities
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string RestrictToSingleWhiteSpace(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"\s+", " "), @"(\n+|\t+|\r+)", string.Empty).Trim();
        }

        public static string ReverseSlashes(this string str)
        {
            return str.Replace('/', '\\');
        }
    }
}