using System.Linq;
using System.Xml;
using STSCommon;
using STSParser.Models.Passage;

namespace STSWriter
{
    public static class StimuliMapper
    {
        public static XmlDocument Map(Passage passage)
        {
            var document = new XmlDocument();
            var itemReleaseElement = (XmlElement) document.AppendChild(document.CreateElement("itemrelease"));
            itemReleaseElement.SetAttribute("version", "2.0");

            itemReleaseElement.AppendChild(GeneratePassage(document, passage));

            return document;
        }

        private static XmlElement GeneratePassage(XmlDocument document, Passage passage)
        {
            var passageElement = document.CreateElement("passage");
            passageElement.SetAttribute("id", passage.Id);
            passageElement.SetAttribute("version", "0");
            passageElement.SetAttribute("bankkey", ExtractionSettings.BankKey);

            passageElement.AppendChild(GenerateAttribList(document, passage));
            passageElement.AppendChild(document.CreateElement("resourcelist"));

            passageElement.AppendChild(GenerateContent(document, passage));

            return passageElement;
        }

        private static XmlElement GenerateContent(XmlDocument document, Passage passage)
        {
            var contentElement = document.CreateElement("content");
            contentElement.SetAttribute("language", "ESN");
            contentElement.SetAttribute("version", "2.0");
            contentElement.SetAttribute("approvedVersion", "0");

            var titleElement = document.CreateElement("title");
            titleElement.InnerText = "<![CDATA[&#xA0;]]>";

            var authorElement = document.CreateElement("title");
            authorElement.InnerText = "<![CDATA[&#xA0;]]>";

            contentElement.AppendChild(titleElement);
            contentElement.AppendChild(authorElement);

            var stemElement = document.CreateElement("stem");
            var elementCount = 0;
            stemElement.InnerText =
                $"<![CDATA[{passage.Body.Elements.ToList().Select(x => x.IsResource() ? $"<img src={passage.Id}_{elementCount++} />" : x.Text).Aggregate((x, y) => $"{x}{y}")}]]>";
            contentElement.AppendChild(stemElement);

            return contentElement;
        }

        private static XmlElement GenerateAttribList(XmlDocument document, Passage passage)
        {
            var attributeListElement = document.CreateElement("attriblist");

            attributeListElement.AppendChild(CommonMapper.GenerateAttribute(document, "stm_pass_id", "Stim: ITS ID",
                passage.Id));
            attributeListElement.AppendChild(CommonMapper.GenerateAttribute(document, "stm_pass_subject",
                "Stim: Subject", "ELA"));
            attributeListElement.AppendChild(CommonMapper.GenerateAttribute(document, "stm_pass_desc",
                "Stim: Description"));

            return attributeListElement;
        }
    }
}