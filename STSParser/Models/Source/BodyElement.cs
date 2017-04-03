﻿

using System.Drawing;

namespace STSParser.Models.Source
{
    public class BodyElement
    {
        public string Text { get; set; }
        public Image Image { get; set; }

        public bool IsResource()
        {
            return Image != null && string.IsNullOrEmpty(Text);
        }
    }
}