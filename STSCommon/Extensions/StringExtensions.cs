using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace STSCommon.Extensions
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


        public static string ProduceImgElementWithSourceFromStringElement(this string imageElement,
            string replacementSource)
        {
            var document = new HtmlDocument();
            document.LoadHtml(imageElement);
            var imageNode = document.DocumentNode.SelectSingleNode("//img");
            imageNode.Attributes.RemoveAll();
            imageNode.SetAttributeValue("src", replacementSource);
            return imageNode.OuterHtml;
        }
    }
}