﻿using System.Collections.Generic;

namespace STSCommon.Models.Item
{
    public class ItemBody
    {
        public ItemBody()
        {
            Elements = new List<BodyElement>();
            AnswerChoices = new Dictionary<string, BodyElement>();
        }

        public List<BodyElement> Elements { get; set; }
        public Dictionary<string, BodyElement> AnswerChoices { get; set; }
    }
}