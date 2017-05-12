using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;
using STSCommon;
using STSCommon.Extensions;
using STSCommon.Models.Passage;

namespace STSWriter.Mappers
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

            var spanishContent = GenerateContent(document, passage);
            var englishContent = (XmlElement)
                spanishContent.CloneNode(true);
            englishContent.SetAttribute("language", "ENU");

            passageElement.AppendChild(spanishContent);
            passageElement.AppendChild(englishContent);

            return passageElement;
        }

        private static XmlElement GenerateContent(XmlDocument document, Passage passage)
        {
            var contentElement = document.CreateElement("content");
            contentElement.SetAttribute("language", "ESN");
            contentElement.SetAttribute("version", "2.0");
            contentElement.SetAttribute("approvedVersion", "0");

            var titleElement = document.CreateElement("title");
            titleElement.AppendChild(document.CreateCDataSection("&#xA0;"));

            var authorElement = document.CreateElement("author");
            authorElement.AppendChild(document.CreateCDataSection("&#xA0;"));

            contentElement.AppendChild(titleElement);
            contentElement.AppendChild(authorElement);

            var stemElement = document.CreateElement("stem");
            var elementCount = 0;
            passage.Body.Elements.Where(x => x.IsResource()).ToList().ForEach(x =>
            {
                var i = 0;
                var path = $"./{ExtractionSettings.Output}/Stimuli/stim-{ExtractionSettings.BankKey}-{passage.Id}";
                Directory.CreateDirectory(path);
                x.Image.Save($"{path}/{passage.Id}_{i++}.png", ImageFormat.Png);
            });

            stemElement.AppendChild(
                document.CreateCDataSection(
                    $"{passage.Body.Elements.ToList().Select(x => x.IsResource() ? x.Text.ProduceImgElementWithSourceFromStringElement($"{passage.Id}_{elementCount++}.png") : x.Text).Aggregate((x, y) => $"{x}{y}")}>"));
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