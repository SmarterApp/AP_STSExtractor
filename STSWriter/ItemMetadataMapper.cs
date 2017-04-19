﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using STSCommon;
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
            var smarterAppMetadata = document.CreateElement("smarterAppMetadata")
                .AppendChild(document, "Identifier", item.Id)
                .AppendChild(document, "Subject", "Spanish")
                .AppendChild(document, "Version", "1.0")
                .AppendChild(document, "AssociatedStimulus", item.PassageId)
                .AppendChild(document, "ItemAuthorIdentifier")
                .AppendChild(document, "ItemSpecFormat")
                .AppendChild(document, "LastModifiedBy")
                .AppendChild(document, "SecurityStatus")
                .AppendChild(document, "SmarterAppItemDescriptor")
                .AppendChild(document, "Status")
                .AppendChild(document, "StimulusFormat")
                .AppendChild(document, "IntendedGrade", ExtractionSettings.Grade)
                .AppendChild(document, "DepthOfKnowledge", item.Metadata["DOK"])
                .AppendChild(document, "TargetAssessmentType")
                .AppendChild(document, "InteractionType")
                .AppendChild(document, "EducationalDifficulty")
                .AppendChild(document, "MaximumNumberOfPoints")
                .AppendChild(document, "EvidenceStatement",
                    $"{item.Metadata["StandardCode"]} {item.Metadata["ReportCategory"]} {item.Metadata["Standard"]}")
                .AppendChild(document, "SufficientEvidenceOfClaim")
                .AppendChild(document, "BrailleType")
                .AppendChild(document, "MinimumGrade", ExtractionSettings.Grade)
                .AppendChild(document, "MaximumGrade", ExtractionSettings.Grade)
                .AppendChild(document, "ScorePoints")
                .AppendChild(document, "StimulusName")
                .AppendChild(document, "StimulusType")
                .AppendChild(document, "AssociatedTutorial")
                .AppendChild(document, "Language", "ESN")
                .AppendChild(document, "ExternalItemId", item.Metadata["ItemCode"]);
            smarterAppMetadata.AppendChild(GenerateStandardPublication(document, item));
            smarterAppMetadata.AppendChild(GenerateIrtDimension(document));

            return smarterAppMetadata;
        }

        private static XmlElement GenerateStandardPublication(XmlDocument document, Item item)
        {
            var standardPublicationElement = document.CreateElement("StandardPublication")
                .AppendChild(document, "Publication", "STS")
                .AppendChild(document, "PrimaryStandard", item.Metadata["StandardCode"]);
            return standardPublicationElement;
        }

        private static XmlElement GenerateIrtDimension(XmlDocument document)
        {
            var irtDimensionElement = document.CreateElement("IrtDimension")
                .AppendChild(document, "IrtStatDomain")
                .AppendChild(document, "IrtModelType")
                .AppendChild(document, "IrtDimensionPurpose")
                .AppendChild(document, "IrtScore")
                .AppendChild(document, "IrtWeight");
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