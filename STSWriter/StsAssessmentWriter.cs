using System.IO;
using System.Linq;
using NLog;
using STSCommon;
using STSCommon.Extensions;
using STSCommon.Models;
using STSWriter.Mappers;

namespace STSWriter
{
    public static class StsAssessmentWriter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                        Logger.LogInfo(new ProcessingReportItem
                        {
                            Destination = path,
                            Type = "Item",
                            UniqueId = fullItemId
                        });
                        Directory.CreateDirectory(path);
                        x.Save($"{path}/item-{fullItemId}.xml");
                    });

            var mappedItemMetadata = stsAssessment.Items.Select(ItemMetadataMapper.Map);
            mappedItemMetadata.ToList().ForEach(x =>
            {
                var fullItemId =
                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                var path = $"./{ExtractionSettings.Output}/Items/Item-{fullItemId}";
                Logger.LogInfo(new ProcessingReportItem
                {
                    Destination = $"{path}/metadata.xml",
                    Type = "Item Metadata",
                    UniqueId = fullItemId
                });
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
                        Logger.LogInfo(new ProcessingReportItem
                        {
                            Destination = path,
                            Type = "Stimulus",
                            UniqueId = fullStimuliId
                        });
                        Directory.CreateDirectory(path);
                        x.Save($"{path}/stim-{fullStimuliId}.xml");
                    });

            var mappedStimuliMetadata = stsAssessment.Passages.Select(StimuliMetadataMapper.Map);
            mappedStimuliMetadata.ToList().ForEach(x =>
            {
                var fullStimulusId =
                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                var path = $"./{ExtractionSettings.Output}/Stimuli/stim-{fullStimulusId}";
                Logger.LogInfo(new ProcessingReportItem
                {
                    Destination = $"{path}/metadata.xml",
                    Type = "Stimulus Metadata",
                    UniqueId = fullStimulusId
                });
                x.Save($"{path}/metadata.xml");
            });
        }
    }
}