using System.Collections.Generic;
using STSParser.Models.Item;
using STSParser.Models.Passage;

namespace STSParser.Parsers
{
    public class STSAssessment
    {
        public STSAssessment()
        {
            Items = new List<Item>();
            Passages = new List<Passage>();
        }

        public List<Item> Items { get; set; }
        public List<Passage> Passages { get; set; }
    }
}