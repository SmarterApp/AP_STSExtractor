using System;
using System.Collections.Generic;
using System.Linq;

namespace STSCommon.Models
{
    public class StsMetadata : Dictionary<string, string>
    {
        public void PurgeEmpties()
        {
            Keys.ToList()
                .ForEach(x => this[x] = this[x]
                    .Equals("&nbsp;", StringComparison.OrdinalIgnoreCase)
                    ? string.Empty
                    : this[x]);
        }

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