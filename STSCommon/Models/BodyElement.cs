using System.Drawing;

namespace STSCommon.Models
{
    public class BodyElement
    {
        public string Text { get; set; }
        public Image Image { get; set; }

        public bool IsResource()
        {
            return Image != null;
        }
    }
}