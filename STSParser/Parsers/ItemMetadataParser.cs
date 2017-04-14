using System.Linq;
using HtmlAgilityPack;
using STSCommon.Utilities;
using STSParser.Models;
using STSParser.Models.Item;
using STSParser.Models.Passage;

namespace STSParser.Parsers
{
    public static class ItemMetadataParser
    {
        public static StsMetadata Parse(HtmlNode table, bool isPassage = false)
        {
            var nodes = table.ChildNodes
                .Where(x => x.Name.Equals("tr"))
                .SelectMany(x => x.ChildNodes
                    .Where(y => y.Name.Equals("td")))
                .ToList();
            var metadata = isPassage
                ? new PassageMetadata()
                : (StsMetadata) new ItemMetadata();
            for (var i = 0; i < nodes.Count;)
            {
                if (metadata.Keys.Contains(nodes[i].InnerText.RemoveSpecialCharacters()))
                {
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
                        metadata.AddMetadata("CorrectAnswer", correctAnswer.Trim());
                    }
                    i++;
                }
            }
            return metadata;
        }
    }
}