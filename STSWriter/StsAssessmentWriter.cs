using System.IO;
using System.Linq;
using STSCommon;
using STSParser.Models;

namespace STSWriter
{
    public static class StsAssessmentWriter
    {
        public static void Write(StsAssessment stsAssessment)
        {
            var mappedItems = stsAssessment.Items.Select(ItemMapper.Map);
            mappedItems.ToList()
                .ForEach(
                    x =>
                    {
                        var fullItemId =
                            $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//item")?.Attributes?.GetNamedItem("id").Value}";
                        var path = $"./{ExtractionSettings.Output}/Items/Item-{fullItemId}";
                        Directory.CreateDirectory(path);
                        x.Save($"{path}/item-{fullItemId}.xml");
                    });

            var mappedItemMetadata = stsAssessment.Items.Select(ItemMetadataMapper.Map);
            mappedItemMetadata.ToList().ForEach(x =>
            {
                var fullItemId =
                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                var path = $"./{ExtractionSettings.Output}/Items/Item-{fullItemId}";
                x.Save($"{path}/metadata.xml");
            });

            var mappedStimuli = stsAssessment.Passages.Select(StimuliMapper.Map);
            mappedStimuli.ToList()
                .ForEach(
                    x =>
                    {
                        var fullStimuliId =
                            $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//passage")?.Attributes?.GetNamedItem("id").Value}";
                        var path = $"./{ExtractionSettings.Output}/Stimuli/stim-{fullStimuliId}";
                        Directory.CreateDirectory(path);
                        x.Save($"{path}/stim-{fullStimuliId}.xml");
                    });

            var mappedStimuliMetadata = stsAssessment.Passages.Select(StimuliMetadataMapper.Map);
            mappedStimuliMetadata.ToList().ForEach(x =>
            {
                var fullStimulusId =
                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                var path = $"./{ExtractionSettings.Output}/Stimuli/stim-{fullStimulusId}";
                x.Save($"{path}/metadata.xml");
            });
        }
    }
}