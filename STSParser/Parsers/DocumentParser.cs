using System;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon.Models;
using STSCommon.Models.Item;

namespace STSParser.Parsers
{
    public class DocumentParser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DocumentParser(HtmlDocument navigator)
        {
            Navigator = navigator;
            Logger.Trace("Document parser initialized...");
        }

        private HtmlDocument Navigator { get; }

        public StsAssessment Parse()
        {
            var result = new StsAssessment();
            foreach (var div in Navigator.DocumentNode.SelectNodes("//div"))
            {
                Logger.Trace("Processing div...");
                var isItemMetadata = false;
                var nodes = div.ChildNodes;
                for (var i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Name.Equals("h1", StringComparison.OrdinalIgnoreCase))
                    {
                        var remainingParagraphNodes =
                            nodes.Skip(i)
                                .Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase)).ToList();
                        Logger.Trace($"Processing passage with {remainingParagraphNodes.Count} nodes");
                        result.Passages.Add(PassageParser.Parse(remainingParagraphNodes));
                        break;
                    }
                    if (nodes[i].Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodes[i].InnerText.Contains("Item")
                            && nodes[i].InnerText.Contains("Information"))
                        {
                            isItemMetadata = true;
                            Logger.Trace("Found item metadata");
                        }
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isItemMetadata)
                    {
                        Logger.Trace("Processing item metadata");
                        result.Items.Add(new Item {Metadata = (ItemMetadata) ItemMetadataParser.Parse(nodes[i])});
                        isItemMetadata = false;
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata)
                    {
                        Logger.Trace("Processing item body");
                        result.Items.Last().Body = ItemBodyParser.Parse(nodes[i]);
                    }
                }
            }
            return result;
        }
    }
}