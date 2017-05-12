using System.Collections.Generic;

namespace STSCommon.Models.Passage
{
    public class PassageBody
    {
        public PassageBody()
        {
            Elements = new List<BodyElement>();
        }

        public List<BodyElement> Elements { get; set; }
    }
}