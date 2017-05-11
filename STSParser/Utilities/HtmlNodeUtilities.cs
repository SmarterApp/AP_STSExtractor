using System;
using System.Linq;
using HtmlAgilityPack;
using STSCommon.Utilities;
using STSParser.Models;

namespace STSParser.Utilities
{
    public static class HtmlNodeUtilities
    {
        public static BodyElement BodyElementFromNode(string path, HtmlNode node)
        {
            var element = new BodyElement();
            var imgNode =
                node.Descendants().FirstOrDefault(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase));
            element.Text = node.OuterHtml;
            if (imgNode != null)
            {
                element.Image = ImageUtilities.ImageFromParentHtmlNode(path, imgNode);
            }
            return element;
        }
    }
}