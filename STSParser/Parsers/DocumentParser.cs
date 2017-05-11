using System;
using System.Linq;
using HtmlAgilityPack;
using STSCommon.Models;
using STSCommon.Models.Item;

namespace STSParser.Parsers
{
    public class DocumentParser
    {
        public DocumentParser(HtmlDocument navigator)
        {
            Navigator = navigator;
        }

        private HtmlDocument Navigator { get; }

        public StsAssessment Parse()
        {
            var result = new StsAssessment();
            foreach (var div in Navigator.DocumentNode.SelectNodes("//div"))
            {
                var isItemMetadata = false;
                var nodes = div.ChildNodes;
                for (var i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Name.Equals("h1", StringComparison.OrdinalIgnoreCase))
                    {
                        var remainingParagraphNodes =
                            nodes.Skip(i)
                                .Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase)).ToList();
                        result.Passages.Add(PassageParser.Parse(remainingParagraphNodes));
                        break;
                    }
                    if (nodes[i].Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodes[i].InnerText.Contains("Item")
                            && nodes[i].InnerText.Contains("Information"))
                        {
                            isItemMetadata = true;
                        }
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isItemMetadata)
                    {
                        result.Items.Add(new Item {Metadata = (ItemMetadata) ItemMetadataParser.Parse(nodes[i])});
                        isItemMetadata = false;
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata)
                    {
                        result.Items.Last().Body = ItemBodyParser.Parse(nodes[i]);
                    }
                }
            }
            return result;
        }
    }
}