using System.Linq;
using HtmlAgilityPack;
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
                        var text = itemBody.AnswerChoices[answer].Text;
                        text = text.Trim().Substring(1).RestrictToSingleWhiteSpace();
                        itemBody.AnswerChoices[answer].Text = text;
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