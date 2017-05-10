using System.Xml;
using STSParser.Models.Passage;

namespace STSWriter.Mappers
{
    public static class StimuliMetadataMapper
    {
        public static XmlDocument Map(Passage passage)
        {
            var document = new XmlDocument();
            var metadataElement = (XmlElement) document.AppendChild(document.CreateElement("metadata"));
            metadataElement.AppendChild(GenerateSmarterAppMetadata(document, passage));

            return document;
        }

        private static XmlElement GenerateSmarterAppMetadata(XmlDocument document, Passage passage)
        {
            var smarterAppMetadata = document.CreateElement("smarterAppMetadata");
            var identifier = document.CreateElement("Identifier");
            identifier.InnerText = passage.Id;
            smarterAppMetadata.AppendChild(identifier);
            smarterAppMetadata.AppendChild(document.CreateElement("Subject"));
            smarterAppMetadata.AppendChild(document.CreateElement("Version"));
            smarterAppMetadata.AppendChild(document.CreateElement("InteractionType"));
            smarterAppMetadata.AppendChild(document.CreateElement("ItemAuthorIdentifier"));
            smarterAppMetadata.AppendChild(document.CreateElement("ItemSpecFormat"));
            smarterAppMetadata.AppendChild(document.CreateElement("LastModifiedBy"));
            smarterAppMetadata.AppendChild(document.CreateElement("SecurityStatus"));
            smarterAppMetadata.AppendChild(document.CreateElement("SmarterAppItemDescriptor"));
            smarterAppMetadata.AppendChild(document.CreateElement("Status"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusFormat"));
            smarterAppMetadata.AppendChild(document.CreateElement("MaximumGrade"));
            smarterAppMetadata.AppendChild(document.CreateElement("MinimumGrade"));
            smarterAppMetadata.AppendChild(document.CreateElement("IntendedGrade"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusLength"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusGenre"));
            smarterAppMetadata.AppendChild(document.CreateElement("StimulusGraphic"));
            smarterAppMetadata.AppendChild(document.CreateElement("BrailleType"));
            smarterAppMetadata.AppendChild(document.CreateElement("Language"));
            return smarterAppMetadata;
        }
    }
}