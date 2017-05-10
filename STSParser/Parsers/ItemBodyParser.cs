using System.Linq;
using HtmlAgilityPack;
using STSCommon.Extensions;
using STSCommon.Utilities;
using STSParser.Models.Item;
using STSParser.Utilities;

namespace STSParser.Parsers
{
    public static class ItemBodyParser
    {
        public static ItemBody Parse(HtmlNode table)
        {
            var itemBody = new ItemBody();
            foreach (var p in table.SelectNodes(".//p"))
            {
                var answer = p.SelectNodes(".//span")?
                    .FirstOrDefault(
                        x => StringUtilities.MatchesCharacterInRange(x.InnerText, 'A', 'D'))?
                    .InnerText.Trim();

                if (!string.IsNullOrEmpty(answer))
                {
                    itemBody.AnswerChoices.Add(answer, HtmlNodeUtilities.BodyElementFromNode(p));
                    if (!itemBody.AnswerChoices[answer].IsResource())
                    {
                        var document = new HtmlDocument();
                        document.LoadHtml(itemBody.AnswerChoices[answer].Text);
                        var span = document.DocumentNode.SelectNodes("//span").Last();
                        itemBody.AnswerChoices[answer].Text = span.OuterHtml.Trim();
                    }
                }
                else
                {
                    itemBody.Elements.Add(HtmlNodeUtilities.BodyElementFromNode(p));
                }
            }
            return itemBody;
        }
    }
}