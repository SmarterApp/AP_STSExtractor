using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace STSCommon.Extensions
{
    public static class XmlElementExtensions
    {
        public static XmlElement AppendChild(this XmlElement element, XmlDocument document, string name,
            string value = "",
            IDictionary<string, string> attributes = null)
        {
            var childElement = document.CreateElement(name);
            childElement.InnerText = value;
            attributes?.Keys.ToList().ForEach(x => childElement.SetAttribute(x, attributes[x]));
            element.AppendChild(childElement);
            return element;
        }
    }
}