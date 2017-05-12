using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSCommon;
using STSCommon.Extensions;
using STSCommon.Models;
using STSCommon.Utilities;
using STSParser.Parsers;
using STSWriter;

namespace STSExtractor
{
    internal class Program
    {
        private const string HelpMessage =
            "AP_STSExtractor \n\n" +
            "The STS Extractor is an application designed to translate California Standards-based Test for Spanish (STS) \n\n " +
            "assessment information to the resource item and stimuli formats consumable by Smarter Balanced's Test Delivery System (TDS). \n\n" +
            "The application is a console application written on top of the .NET framework in C#. It is designed to be run on Windows from the command line.\n\n" +
            "The application requires several inputs: \n" +
            " -i  The STS assessments generally come as word documents. The STS extractor requires .htm document input. \n" +
            "\tTo provide the documents in the correct format, use the \"save as\" dialog in word to save the STS document as \"web page filtered\". \n" +
            "\tThis will produce the necessary .htm file and resources directory for this project. The input to this command line parameter is the \n" +
            "\tpath to that .htm file. **Be sure to rename the file to a path without any special characters!** (Ex. \"C:\\Projects\\SmarterBalanced\\Resources\\GRADE3.htm\")\n" +
            "-o The name of the output directory that the STS extractor will write out its results to(Ex. \"prod-test\")\n" +
            "-b The bank key to be applied to each item.Currently, the only valid bank keys are 187 & 200. \n" +
            "\tThese two values are located in the app.config contained in a comma delineated list. \n" +
            "\tCurrently, those two values are the only two corresponding to valid clients in the TDS system. \n" +
            "\tUsing other bank keys will cause errors at runtime.In future (as more clients and associated bank keys are added) \n" +
            "\tthe app.config may be modified to include those new bank keys as valid inputs. (Ex. \"187\")" +
            "-s Seed item ID.The first item extracted will be assigned this seed ID.\n" +
            "\tAll other items will receive the subsequent IDs incrementing from this seed. \n" +
            "\tEnsure that your item seed will not produce items with IDs that match existing items. (Ex. \"300\")\n" +
            "-g This is grade level.All items produced will default to this grade. (Ex. \"7\")\n\n" +
            "Attempting to run the application without any command line inputs, or running it with the -h flag will print these instructions to the console.";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            Logger.Debug(string.Concat(Enumerable.Repeat("-", 60)));
            Logger.Debug("STS Extractor Initialized");
            try
            {
                var inputFilenames = new List<string>();
                string outputFilename = null;
                var applicationSettings = new ApplicationSettings();
                applicationSettings.Initialize();

                var help = false;

                for (var i = 0; i < args.Length; ++i)
                {
                    switch (args[i])
                    {
                        case "-h":
                            help = true;
                            Logger.Debug("Application run in help mode");
                            break;

                        case "-b":
                            ++i;
                            ExtractionSettings.BankKey = args[i];
                            if (applicationSettings["ValidBankKeys"].Split(',').All(x => !x.Equals(args[i])))
                            {
                                Logger.LogError(new ErrorReportItem
                                    {
                                        Location = "app.config",
                                        Severity = LogLevel.Warn
                                    },
                                    $"BankKey value {args[i]} is not verified. Client with associated Bank Key must be present in the TDS database for items generated using this key to display property.");
                            }
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
                            Logger.Trace($"Input found: {args[i]}");
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
                            if (outputFilename != null)
                            {
                                Logger.Error(
                                    $"Output filename already set to: {outputFilename}, cannot set to {args[i]}");
                                throw new ArgumentException("Only one item output filename may be specified.");
                            }
                            outputFilename = args[i];
                            Logger.Trace($"Output filename set to: {args[i]}");
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
                else if (inputFilenames.Count == 0 || outputFilename == null)
                {
                    Logger.Error(
                        "Invalid command line. One output filename and at least one input filename must be specified.");
                }
                else if (string.IsNullOrEmpty(ExtractionSettings.ItemId.ToString()))
                {
                    Logger.LogError(new ErrorReportItem
                    {
                        Location = "Application input",
                        Severity = LogLevel.Fatal
                    }, "This application requires a seed ItemId as a command line parameter following flag '-s'");
                }
                else if (string.IsNullOrEmpty(ExtractionSettings.BankKey))
                {
                    Logger.LogError(new ErrorReportItem
                    {
                        Location = "Application input",
                        Severity = LogLevel.Fatal
                    }, "This application requires a BankKey as a command line parameter following flag '-b'");
                }
                else if (string.IsNullOrEmpty(ExtractionSettings.ItemId.ToString()))
                {
                    Logger.LogError(new ErrorReportItem
                    {
                        Location = "Application input",
                        Severity = LogLevel.Fatal
                    }, "This application requires a valid Grade as a command line parameter following flag '-g'");
                }
                else
                {
                    inputFilenames.ToList().ForEach(x =>
                    {
                        var documentParser = new DocumentParser(new HtmlDocument().LoadFromPath(x));
                        var result = documentParser
                            .Parse()
                            .AssignIdentifiers();
                        StsAssessmentWriter.Write(result);
                    });
                }
            }
            catch (Exception e)
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = e.Source,
                    Severity = LogLevel.Fatal
                }, $"A fatal error occurred: {e.Message}");
            }
            finally
            {
                Console.Write("Press any key to exit.");
                Console.ReadKey(true);
            }
        }
    }
}