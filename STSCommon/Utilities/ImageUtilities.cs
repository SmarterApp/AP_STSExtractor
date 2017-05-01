using System;
using System.Drawing;
using System.IO;
using HtmlAgilityPack;
using STSCommon.Extensions;

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

        private static string RetrieveChildImageSource(HtmlNode node)
        {
            if (node.Attributes.Contains("src"))
            {
                return
                    node.GetAttributeValue("src", string.Empty).ReverseSlashes();
            }
            throw new ArgumentException("Attempted retrieval of img without a src attribute");
        }
    }
}