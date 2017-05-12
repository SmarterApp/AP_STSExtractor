using System;
using System.Collections.Generic;
using System.Linq;

namespace STSCommon.Models
{
    public class StsAssessment
    {
        public StsAssessment()
        {
            Items = new List<Item.Item>();
            Passages = new List<Passage.Passage>();
        }

        public List<Item.Item> Items { get; set; }
        public List<Passage.Passage> Passages { get; set; }

        public StsAssessment AssignIdentifiers()
        {
            Items.ForEach(x => x.Id = ExtractionSettings.ItemId++.ToString());
            Items.ForEach(x => x.Metadata.PurgeEmpties());
            Passages.ForEach(x => x.Id = ExtractionSettings.ItemId++.ToString());
            Passages.ForEach(x => x.Metadata.PurgeEmpties());
            Items
                .Where(x => !string.IsNullOrEmpty(x.Metadata["PassageCode"]))
                .ToList()
                .ForEach(x => x.PassageId = Passages
                    .First(y => y.Metadata["PassageCode"].Trim()
                        .Equals(x.Metadata["PassageCode"].Trim(), StringComparison.OrdinalIgnoreCase)).Id);
            return this;
        }
    }
}