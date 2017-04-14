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

            itemElement.AppendChild(GenerateContentElement(document));

            return itemElement;
        }

        private static XmlElement GenerateContentElement(XmlDocument document)
        {
            var contentElement = document.CreateElement("content");
            contentElement.SetAttribute("language", "ESN");
            contentElement.SetAttribute("version", "2.0");
            contentElement.SetAttribute("approvedVersion", "0");
            return contentElement;
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