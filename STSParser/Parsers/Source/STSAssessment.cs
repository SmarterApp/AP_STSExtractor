using System.Collections.Generic;
using STSParser.Models.Source.Item;
using STSParser.Models.Source.Passage;

namespace STSParser.Parsers.Source
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