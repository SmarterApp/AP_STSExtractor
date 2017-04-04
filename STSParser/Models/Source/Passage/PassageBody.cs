﻿using System.Collections.Generic;

namespace STSParser.Models.Source.Passage
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