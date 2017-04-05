using System;
using System.Drawing;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace STSCommon.Utilities
{
    public static class ImageUtilities
    {
        public static Image ImageFromParentHtmlNode(HtmlNode node)
        {
            return Image.FromFile(Path.Combine(GetSourceDirectory(), RetrieveChildImageSource(node)));
        }

        private static string GetSourceDirectory()
        {
            if (string.IsNullOrEmpty(ExtractionSettings.Input) || !File.Exists(ExtractionSettings.Input))
            {
                throw new ArgumentException("Bad input path specified");
            }
            return new FileInfo(ExtractionSettings.Input).DirectoryName;
        }

        private static bool ElementHasValidChildImage(HtmlNode node)
        {
            return node.HasChildNodes &&
                   node.ChildNodes.Any(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase)) &&
                   node.ChildNodes.First(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase)).HasAttributes &&
                   node.ChildNodes.First(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase))
                       .Attributes.Contains("src");
        }

        private static string RetrieveChildImageSource(HtmlNode node)
        {
            if (ElementHasValidChildImage(node))
            {
                return
                    node.ChildNodes.First(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase))
                        .GetAttributeValue("src", string.Empty).ReverseSlashes();
            }
            throw new ArgumentException("Attempted retrieval of img without a src attribute");
        }
    }
}