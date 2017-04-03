using System.Collections.Generic;
using System.Linq;

namespace STSParser.Models.Source
{
    public class StsMetadata : Dictionary<string, string>
    {
        public bool AddMetadata(string key, string value)
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