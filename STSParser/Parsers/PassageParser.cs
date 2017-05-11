using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon;
using STSCommon.Extensions;
using STSCommon.Models;
using STSCommon.Models.Passage;
using STSCommon.Utilities;

namespace STSParser.Parsers
{
    public static class PassageParser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                        Logger.LogError(new ErrorReportItem
                        {
                            Location = $"Passage Parsing: {node.OuterHtml}",
                            Severity = LogLevel.Error
                        }, "Unable to retrieve nodeInfo while parsing passage");
                        continue;
                    }
                    if (nodeInfo.Contains("Passage") && nodeInfo.Contains("Code"))
                    {
                        passage.Metadata.AddMetadata("PassageCode",
                            nodeInfo.RestrictToSingleWhiteSpace().Split(':').LastOrDefault());
                        Logger.Trace("Passage code parsed");
                    }
                    else if (nodeInfo.Contains("Passage") && nodeInfo.Contains("Title"))
                    {
                        passage.Metadata.AddMetadata("PassageTitle",
                            nodeInfo.RestrictToSingleWhiteSpace().Split(':').LastOrDefault());
                        Logger.Trace("Passage title parsed");
                    }
                }
                else
                {
                    Logger.Trace("Passage body parsed");
                    passage.Body.Elements.Add(HtmlNodeUtilities.BodyElementFromNode(ExtractionSettings.Input, node));
                }
            }
            return passage;
        }
    }
}