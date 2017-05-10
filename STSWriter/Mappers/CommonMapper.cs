using System.Xml;

namespace STSWriter.Mappers
{
    public static class CommonMapper
    {
        public static XmlElement GenerateAttribute(XmlDocument document, string id, string name, string value = "",
            string description = "")
        {
            var attribute = document.CreateElement("attrib");
            attribute.SetAttribute("attid", id);

            var nameElement = document.CreateElement("name");
            nameElement.InnerText = name;

            var valueElement = document.CreateElement("val");
            valueElement.InnerText = value;

            var descriptionElement = document.CreateElement("desc");
            descriptionElement.InnerText = description;

            attribute.AppendChild(nameElement);
            attribute.AppendChild(valueElement);
            attribute.AppendChild(descriptionElement);

            return attribute;
        }
    }
}