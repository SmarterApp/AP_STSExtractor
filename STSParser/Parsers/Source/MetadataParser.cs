using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using STSParser.Models.Source;
using STSParser.Utilities;

namespace STSParser.Parsers.Source
{
    public static class MetadataParser
    {
        public static StsMetadata Parse(IList<HtmlNode> nodes, bool isPassage = false)
        {
            var metadata = isPassage
                ? new PassageMetadata()
                : (StsMetadata) new ItemMetadata();
            for (var i = 0; i < nodes.Count;)
            {
                if (metadata.Keys.Contains(nodes[i].InnerText.RemoveSpecialCharacters()))
                {
                    metadata.AddMetadata(nodes[i].InnerText.RemoveSpecialCharacters(),
                        nodes[i + 1].InnerText.RemoveSpecialCharacters());
                    i += 2;
                }
                else
                {
                    i++;
                }
            }
            return metadata;
        }
    }
}