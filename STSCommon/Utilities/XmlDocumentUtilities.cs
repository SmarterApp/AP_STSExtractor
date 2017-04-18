using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace STSCommon.Utilities
{
    public static class XmlDocumentUtilities
    {
        public static XmlElement CreateElementWithValueAndAttributes(XmlDocument document, string name, string value,
            IDictionary<string, string> attributes = null)
        {
            var element = document.CreateElement(name);
            element.InnerText = value;
            attributes?.Keys.ToList().ForEach(x => element.SetAttribute(x, attributes[x]));
            return element;
        }
    }
}
