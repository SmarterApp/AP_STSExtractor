using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon.Extensions;
using STSCommon.Models;
using STSCommon.Models.Item;
using STSCommon.Models.Passage;
using STSCommon.Utilities;

namespace STSParser.Parsers
{
    public static class ItemMetadataParser
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static StsMetadata Parse(HtmlNode table, bool isPassage = false)
        {
            var nodes = table.ChildNodes
                .Where(x => x.Name.Equals("tr"))
                .SelectMany(x => x.ChildNodes
                    .Where(y => y.Name.Equals("td")))
                .ToList();
            Logger.Trace($"Found {nodes.Count} eligible nodes for item metadata parsing");
            var metadata = isPassage
                ? new PassageMetadata()
                : (StsMetadata) new ItemMetadata();
            for (var i = 0; i < nodes.Count;)
            {
                if (metadata.Keys.Contains(nodes[i].InnerText.RemoveSpecialCharacters()))
                {
                    Logger.Trace($"Found metadata for {nodes[i].InnerText.RemoveSpecialCharacters()}");
                    metadata.AddMetadata(nodes[i].InnerText.RemoveSpecialCharacters(),
                        nodes[i + 1].InnerText.RestrictToSingleWhiteSpace());
                    i += 2;
                }
                else
                {
                    var correctAnswer = nodes[i].InnerText.RestrictToSingleWhiteSpace();
                    if (StringUtilities.MatchesCharacterInRange(correctAnswer, 'A', 'D') &&
                        metadata.ContainsKey("CorrectAnswer"))
                    {
                        Logger.Trace("Found correct answer");
                        metadata.AddMetadata("CorrectAnswer", correctAnswer.Trim());
                    }
                    i++;
                }
            }
            return metadata;
        }
    }
}