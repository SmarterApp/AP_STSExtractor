using System.Text;

namespace STSParser.Utilities
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
    }
}