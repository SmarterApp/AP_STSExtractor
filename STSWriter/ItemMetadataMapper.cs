using System.Collections.Generic;
using System.Linq;
using System.Xml;
using STSCommon.Utilities;
using STSParser.Models.Item;

namespace STSWriter
{
    public static class ItemMetadataMapper
    {
        public static XmlDocument Map(Item item)
        {
            var document = new XmlDocument();
            var metadataElement = (XmlElement) document.AppendChild(document.CreateElement("metadata"));
            metadataElement.AppendChild(GenerateSmarterAppMetadata(document, item));

            return document;
        }

        private static XmlElement GenerateSmarterAppMetadata(XmlDocument document, Item item)
        {
            var smarterAppMetadata = document.CreateElement("smarterAppMetadata");
            var identifier = document.CreateElement("Identifier");
            identifier.InnerText = item.Id;
            smarterAppMetadata.AppendChild(identifier);
            smarterAppMetadata.AppendChild(document.CreateElement("Subject"));
            smarterAppMetadata.AppendChild(document.CreateElement("Version"));
            smarterAppMetadata.AppendChild(document.CreateElement("AssociatedStimulus"));
            smarterAppMetadata.AppendChild(document.CreateElement("ItemAuthorIdentifier"));
            smarterAppMetadata.AppendChild(document.CreateElement("ItemSpecFormat"));
            smarterAppMetadata.AppendChild(document.CreateElement("LastModifiedBy"));
            smarterAppMetadata.AppendChild(document.CreateElement("SecurityStatus"));
            smarterAppMetadata.AppendChild(document.CreateElement("SmarterAppItemDescriptor"));
            smarterAppMetadata.AppendChild(document.CreateElement("Status"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusFormat"));
            smarterAppMetadata.AppendChild(document.CreateElement("IntendedGrade"));
            smarterAppMetadata.AppendChild(XmlDocumentUtilities.CreateElementWithValueAndAttributes(document,
                "DepthOfKnowledge", item.Metadata["DOK"]));
            smarterAppMetadata.AppendChild(document.CreateElement("TargetAssessmentType"));
            smarterAppMetadata.AppendChild(document.CreateElement("InteractionType"));
            smarterAppMetadata.AppendChild(document.CreateElement("EducationalDifficulty"));
            smarterAppMetadata.AppendChild(document.CreateElement("MaximumNumberOfPoints"));
            smarterAppMetadata.AppendChild(XmlDocumentUtilities.CreateElementWithValueAndAttributes(document,
                "EvidenceStatement",
                $"{item.Metadata["StandardCode"]} {item.Metadata["ReportCategory"]} {item.Metadata["Standard"]}"));
            smarterAppMetadata.AppendChild(document.CreateElement("SufficientEvidenceOfClaim"));
            smarterAppMetadata.AppendChild(document.CreateElement("BrailleType"));
            smarterAppMetadata.AppendChild(document.CreateElement("MinimumGrade"));
            smarterAppMetadata.AppendChild(document.CreateElement("MaximumGrade"));
            smarterAppMetadata.AppendChild(document.CreateElement("ScorePoints"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusName"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusType"));
            smarterAppMetadata.AppendChild(document.CreateElement("AssociatedTutorial"));
            smarterAppMetadata.AppendChild(document.CreateElement("Language"));
            smarterAppMetadata.AppendChild(GenerateStandardPublication(document));
            smarterAppMetadata.AppendChild(GenerateIrtDimension(document));

            return smarterAppMetadata;
        }

        private static XmlElement GenerateStandardPublication(XmlDocument document)
        {
            var standardPublicationElement = document.CreateElement("StandardPublication");
            standardPublicationElement.AppendChild(document.CreateElement("Publication"));
            standardPublicationElement.AppendChild(document.CreateElement("PrimaryStandard"));
            return standardPublicationElement;
        }

        private static XmlElement GenerateIrtDimension(XmlDocument document)
        {
            var irtDimensionElement = document.CreateElement("IrtDimension");
            irtDimensionElement.AppendChild(document.CreateElement("IrtStatDomain"));
            irtDimensionElement.AppendChild(document.CreateElement("IrtModelType"));
            irtDimensionElement.AppendChild(document.CreateElement("IrtDimensionPurpose"));
            irtDimensionElement.AppendChild(document.CreateElement("IrtScore"));
            irtDimensionElement.AppendChild(document.CreateElement("IrtWeight"));
            GenerateIrtParameters(document).ToList().ForEach(x => irtDimensionElement.AppendChild(x));
            return irtDimensionElement;
        }

        private static IEnumerable<XmlElement> GenerateIrtParameters(XmlDocument document)
        {
            var result = new List<XmlElement>();
            for (var i = 'a'; i <= 'c'; i++)
            {
                var irtParameterElement = document.CreateElement("IrtParameter");
                var nameElement = document.CreateElement("Name");
                nameElement.InnerText = i.ToString();
                var valueElement = document.CreateElement("Value");
                valueElement.InnerText = "0";
                irtParameterElement.AppendChild(nameElement);
                irtParameterElement.AppendChild(valueElement);
                result.Add(irtParameterElement);
            }
            return result;
        }
    }
}