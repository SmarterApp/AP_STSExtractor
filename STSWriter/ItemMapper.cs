using System.Collections.Generic;
using System.Linq;
using System.Xml;
using STSCommon;
using STSParser.Models.Item;

namespace STSWriter
{
    public static class ItemMapper
    {
        public static XmlDocument Map(Item item)
        {
            var document = new XmlDocument();
            var itemReleaseElement = (XmlElement) document.AppendChild(document.CreateElement("itemrelease"));
            itemReleaseElement.SetAttribute("version", "2.0");

            var itemElement = GenerateItemElement(document, item);
            itemReleaseElement.AppendChild(itemElement);

            return document;
        }

        private static XmlElement GenerateItemElement(XmlDocument document, Item item)
        {
            var itemElement = document.CreateElement("item");
            itemElement.SetAttribute("format", "mc");
            itemElement.SetAttribute("id", item.Id);
            itemElement.SetAttribute("version", "0");
            itemElement.SetAttribute("bankkey", ExtractionSettings.BankKey);

            var associatedPassage = document.CreateElement("associatedpassage");
            if (!string.IsNullOrEmpty(item.PassageId))
            {
                associatedPassage.InnerText = item.PassageId;
            }
            itemElement.AppendChild(associatedPassage);

            itemElement.AppendChild(GenerateAttribList(document, item));

            itemElement.AppendChild(document.CreateElement("resourcelist"));
            itemElement.AppendChild(document.CreateElement("statistic"));

            itemElement.AppendChild(GenerateContentElement(document, item));

            return itemElement;
        }

        private static XmlElement GenerateContentElement(XmlDocument document, Item item)
        {
            var contentElement = document.CreateElement("content");
            contentElement.SetAttribute("language", "ESN");
            contentElement.SetAttribute("version", "2.0");
            contentElement.SetAttribute("approvedVersion", "0");

            contentElement.AppendChild(GenerateNameAndValueForParent(document, "concept",
                "What Knowledge Do Students Need to Understand This Concept?"));
            contentElement.AppendChild(GenerateNameAndValueForParent(document, "es",
                "Explanation of Correct Answer"));
            contentElement.AppendChild(GenerateNameAndValueForParent(document, "himi",
                "Explanation of Correct Answer"));

            var rationaleOptListElement = document.CreateElement("rationaleoptlist");
            rationaleOptListElement.AppendChild(GenerateNameAndValueForParent(document, "rationale",
                "Rationale for Option A"));
            rationaleOptListElement.AppendChild(GenerateNameAndValueForParent(document, "rationale",
                "Rationale for Option B"));
            rationaleOptListElement.AppendChild(GenerateNameAndValueForParent(document, "rationale",
                "Rationale for Option C"));
            rationaleOptListElement.AppendChild(GenerateNameAndValueForParent(document, "rationale",
                "Rationale for Option D"));
            contentElement.AppendChild(rationaleOptListElement);

            GenerateContent(document, item).ToList().ForEach(x => contentElement.AppendChild(x));

            return contentElement;
        }

        private static IEnumerable<XmlElement> GenerateContent(XmlDocument document, Item item)
        {
            var imageCount = 0;
            var result =
                item.Body.Elements.Select(
                    bodyElement =>
                        bodyElement.IsResource()
                            ? GenerateIllustration(document, item.Id, imageCount++)
                            : GenerateStem(document, bodyElement.Text)).ToList();

            var optionListElement = document.CreateElement("optionlist");
            foreach (var key in item.Body.AnswerChoices.Keys)
            {
                var optionElement = document.CreateElement("option");
                var optionNameElement = document.CreateElement("name");
                optionNameElement.InnerText = $"Option {key}";
                var optionValueElement = document.CreateElement("val");
                optionValueElement = item.Body.AnswerChoices[key].IsResource()
                    ? GenerateIllustration(document, item.Id, imageCount++)
                    : GenerateStem(document, item.Body.AnswerChoices[key].Text);

                optionElement.AppendChild(optionNameElement);
                optionElement.AppendChild(optionValueElement);
                optionElement.AppendChild(GenerateFeedback(document));

                optionListElement.AppendChild(optionElement);
            }

            result.Add(optionListElement);
            return result;
        }

        private static XmlElement GenerateFeedback(XmlDocument document)
        {
            var feedbackElement = document.CreateElement("feedback");
            feedbackElement.InnerText = "<![CDATA[< p style = \"\" > &#xA0;</p>]]>";
            return feedbackElement;
        }

        private static XmlElement GenerateIllustration(XmlDocument document, string itemId, int count)
        {
            var illustrationElement = document.CreateElement("illustration");
            illustrationElement.InnerText = $"<![CDATA[<p style=\"\"><img src=\"{itemId}_{count}.png\"/></p>]]>";
            return illustrationElement;
        }

        private static XmlElement GenerateStem(XmlDocument document, string content)
        {
            var stemElement = document.CreateElement("stem");
            stemElement.InnerText =
                $"<![CDATA[<p style=\"\">{content}</p>]]>";
            return stemElement;
        }

        private static XmlElement GenerateNameAndValueForParent(XmlDocument document, string parentElementName,
            string nameText, string valText = "&#xA0;")
        {
            var parentElement = document.CreateElement(parentElementName);

            var nameElement = document.CreateElement("name");
            nameElement.InnerText = nameText;

            var valElement = document.CreateElement("val");
            valElement.InnerText = $"<![CDATA[<p style=\"\">{valText}</p>]]>";

            parentElement.AppendChild(nameElement);
            parentElement.AppendChild(valElement);

            return parentElement;
        }

        private static XmlElement GenerateAttribList(XmlDocument document, Item item)
        {
            var attributeListElement = document.CreateElement("attriblist");

            attributeListElement.AppendChild(GenerateAttribute(document, "itm_item_id", "Item: ITS ID", item.Id));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_item_subject", "Item: Subject", "ELA"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_item_desc", "Item: Item Description"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_FTUse", "Fieldtest Use"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_OPUse", "Operational Use"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Answer Key", "Item: Answer Key",
                item.Metadata["CorrectAnswer"]));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Grade", "Item: Grade", "3", "3"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Item Format", "Item: Item Format",
                "MC", "MC4 [1]"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Page Layout", "Item: Page Layout",
                "21"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Response Type", "Item: Response Type",
                "Vertical"));
            attributeListElement.AppendChild(GenerateAttribute(document, "itm_att_Item Point", "Item: Item Point",
                "1 pt.", "1 Point"));
            attributeListElement.AppendChild(GenerateAttribute(document, "stm_pass_id", "Stim: ITS ID",
                !string.IsNullOrEmpty(item.PassageId) ? item.PassageId : string.Empty));

            return attributeListElement;
        }

        private static XmlElement GenerateAttribute(XmlDocument document, string id, string name, string value = "",
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