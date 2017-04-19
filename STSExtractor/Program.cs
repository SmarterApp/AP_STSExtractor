using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon;
using STSParser.Parsers;
using STSWriter;

namespace STSExtractor
{
    internal class Program
    {
        private const string HelpMessage =
            @"This is a description of the application";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            Logger.Info(string.Concat(Enumerable.Repeat("-", 60)));
            Logger.Info("STS Extractor Initialized");
            try
            {
                var inputFilenames = new List<string>();
                string oFilename = null;

                var help = false;

                for (var i = 0; i < args.Length; ++i)
                {
                    switch (args[i])
                    {
                        case "-h":
                            help = true;
                            Logger.Info("Application run in help mode");
                            break;

                        case "-b":
                            ++i;
                            ExtractionSettings.BankKey = args[i];
                            break;

                        case "-s":
                            ++i;
                            ExtractionSettings.ItemId = int.Parse(args[i]);
                            break;
                        case "-g":
                            ++i;
                            ExtractionSettings.Grade = args[i];
                            break;
                        case "-i":
                        {
                            ++i;
                            if (i >= args.Length)
                            {
                                Logger.Error("-i option must be followed by a valid string filename");
                                throw new ArgumentException(
                                    "Invalid command line. '-i' option not followed by filename.");
                            }
                            Logger.Info($"Input found: {args[i]}");
                            ExtractionSettings.Input = args[i];
                            inputFilenames.Add(args[i]);
                        }
                            break;

                        case "-o":
                        {
                            ++i;
                            if (i >= args.Length)
                            {
                                Logger.Error("-o option must be followed by a valid string filename");
                                throw new ArgumentException(
                                    "Invalid command line. '-o' option not followed by filename.");
                            }
                            if (oFilename != null)
                            {
                                Logger.Error($"Output filename already set to: {oFilename}, cannot set to {args[i]}");
                                throw new ArgumentException("Only one item output filename may be specified.");
                            }
                            oFilename = args[i];
                            Logger.Info($"Output filename set to: {args[i]}");
                            Directory.CreateDirectory(args[i]);
                            ExtractionSettings.Output = args[i];
                        }
                            break;

                        default:
                            Logger.Error($"Unknown command line option '{args[i]}'. Use '-h' for syntax help.");
                            throw new ArgumentException(
                                $"Unknown command line option '{args[i]}'. Use '-h' for syntax help.");
                    }
                }


                if (help || args.Length == 0)
                {
                    Console.WriteLine(HelpMessage);
                }
                else if (inputFilenames.Count == 0 || oFilename == null)
                {
                    Logger.Error(
                        "Invalid command line. One output filename and at least one input filename must be specified.");
                    throw new ArgumentException(
                        "Invalid command line. One output filename and at least one input filename must be specified.");
                }
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(inputFilenames.First());
                    var documentParser = new DocumentParser(doc);
                    var result = documentParser.Parse();
                    result = AssignIdentifiers(result);

                    var mappedItems = result.Items.Select(ItemMapper.Map);
                    mappedItems.ToList()
                        .ForEach(
                            x =>
                            {
                                var fullItemId =
                                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//item")?.Attributes?.GetNamedItem("id").Value}";
                                var path = $"./{ExtractionSettings.Output}/Items/{fullItemId}";
                                Directory.CreateDirectory(path);
                                x.Save($"{path}/{fullItemId}.xml");
                            });

                    var mappedItemMetadata = result.Items.Select(ItemMetadataMapper.Map);
                    mappedItemMetadata.ToList().ForEach(x =>
                    {
                        var fullItemId =
                            $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                        var path = $"./{ExtractionSettings.Output}/Items/{fullItemId}";
                        x.Save($"{path}/metadata.xml");
                    });

                    var mappedStimuli = result.Passages.Select(StimuliMapper.Map);
                    mappedStimuli.ToList()
                        .ForEach(
                            x =>
                            {
                                var fullStimuliId =
                                    $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//passage")?.Attributes?.GetNamedItem("id").Value}";
                                var path = $"./{ExtractionSettings.Output}/Stimuli/{fullStimuliId}";
                                Directory.CreateDirectory(path);
                                x.Save($"{path}/{fullStimuliId}.xml");
                            });

                    var mappedStimuliMetadata = result.Passages.Select(StimuliMetadataMapper.Map);
                    mappedStimuliMetadata.ToList().ForEach(x =>
                    {
                        var fullStimulusId =
                            $"{ExtractionSettings.BankKey}-{x.SelectSingleNode(".//Identifier")?.InnerText}";
                        var path = $"./{ExtractionSettings.Output}/Stimuli/{fullStimulusId}";
                        x.Save($"{path}/metadata.xml");
                    });
                }
            }
            catch
                (Exception err)
            {
                Logger.Fatal(err);
                Console.WriteLine();
                Console.WriteLine(err.ToString());
                Console.ReadKey();
            }

            Console.Write("Press any key to exit.");
            Console.ReadKey(true);
        }

        private static STSAssessment AssignIdentifiers(STSAssessment assessment)
        {
            assessment.Items.ForEach(x => x.Id = ExtractionSettings.ItemId++.ToString());
            assessment.Items.ForEach(x => x.Metadata.PurgeEmpties());
            assessment.Passages.ForEach(x => x.Id = ExtractionSettings.ItemId++.ToString());
            assessment.Passages.ForEach(x => x.Metadata.PurgeEmpties());
            assessment.Items
                .Where(x => !string.IsNullOrEmpty(x.Metadata["PassageCode"]))
                .ToList()
                .ForEach(x => x.PassageId = assessment.Passages
                    .First(y => y.Metadata["PassageCode"].Trim()
                        .Equals(x.Metadata["PassageCode"].Trim(), StringComparison.OrdinalIgnoreCase)).Id);
            return assessment;
        }
    }
}