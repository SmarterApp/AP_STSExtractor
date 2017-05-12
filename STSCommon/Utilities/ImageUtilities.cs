using System;
using System.Drawing;
using System.IO;
using HtmlAgilityPack;
using STSCommon.Extensions;

namespace STSCommon.Utilities
{
    public static class ImageUtilities
    {
        public static Image ImageFromParentHtmlNode(string path, HtmlNode node)
        {
            return Image.FromFile(Path.Combine(GetSourceDirectory(path), RetrieveChildImageSource(node)));
        }

        private static string GetSourceDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new ArgumentException("Bad input path specified");
            }
            return new FileInfo(path).DirectoryName;
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