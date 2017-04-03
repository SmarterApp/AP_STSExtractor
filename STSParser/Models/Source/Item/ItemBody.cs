using System.Collections.Generic;

namespace STSParser.Models.Source.Item
{
    public class ItemBody
    {
        public List<BodyElement> Elements { get; set; }
        public Dictionary<string, BodyElement> AnswerChoices { get; set; }
    }
}