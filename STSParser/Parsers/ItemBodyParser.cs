using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon;
using STSCommon.Models.Item;
using STSCommon.Utilities;

namespace STSParser.Parsers
{
    public static class ItemBodyParser
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                    Logger.Trace($"Parsing answer for {answer}");
                    itemBody.AnswerChoices.Add(answer,
                        HtmlNodeUtilities.BodyElementFromNode(ExtractionSettings.Input, p));
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
                    Logger.Trace("Parsing item body");
                    itemBody.Elements.Add(HtmlNodeUtilities.BodyElementFromNode(ExtractionSettings.Input, p));
                }
            }
            return itemBody;
        }
    }
}