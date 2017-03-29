using System.Collections.Generic;
using System.Linq;

namespace STSParser.Models
{
    public class ItemMetadata : Dictionary<string, string>
    {
        public ItemMetadata()
        {
            Add("ItemCode", string.Empty);
            Add("DOK", string.Empty);
            Add("StandardCode", string.Empty);
            Add("Strand", string.Empty);
            Add("ReportCategory", string.Empty);
            Add("Standard", string.Empty);
            Add("CorrectAnswer", string.Empty);
            Add("PassageTitle", string.Empty);
            Add("PassageCode", string.Empty);
        }

        public bool AddItemMetadata(string key, string value)
        {
            if (!Keys.Any(x => x.Equals(key)))
            {
                return false;
            }
            this[key] = value;
            return true;
        }
    }
}