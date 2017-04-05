using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using STSCommon.Utilities;
using STSParser.Models.Source;
using STSParser.Models.Source.Passage;

namespace STSParser.Parsers.Source
{
    public static class PassageParser
    {
        public static Passage Parse(IList<HtmlNode> nodes)
        {
            var passage = new Passage();
            foreach (var node in nodes)
            {
                var nodeInfo =
                    node.ChildNodes.FirstOrDefault(
                        x => x.Name.Equals("span", StringComparison.OrdinalIgnoreCase))?.InnerText;
                if (passage.Metadata.Values.Any(string.IsNullOrEmpty))
                {
                    if (string.IsNullOrEmpty(nodeInfo))
                    {
                        // this is an error condition
                        continue;
                    }
                    if (nodeInfo.Contains("Passage") && nodeInfo.Contains("Code"))
                    {
                        passage.Metadata.AddMetadata("PassageCode",
                            nodeInfo.RestrictToSingleWhiteSpace().Split(':').LastOrDefault());
                    }
                    else if (nodeInfo.Contains("Passage") && nodeInfo.Contains("Title"))
                    {
                        passage.Metadata.AddMetadata("PassageTitle",
                            nodeInfo.RestrictToSingleWhiteSpace().Split(':').LastOrDefault());
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(nodeInfo))
                    {
                        passage.Body.Elements.Add(new BodyElement
                        {
                            Text = nodeInfo
                        });
                    }
                    else if (node.ChildNodes.Any(x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase)))
                    {
                        passage.Body.Elements.Add(new BodyElement
                        {
                            Image = ImageUtilities.ImageFromParentHtmlNode(node)
                        });
                    }
                }
            }
            return passage;
        }
    }
}