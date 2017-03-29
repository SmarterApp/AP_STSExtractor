using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NLog;
using STSParser;

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
                            oFilename = Path.GetFullPath(args[i]);
                            if (oFilename.EndsWith(".htm"))
                            {
                                oFilename = oFilename.Substring(0, oFilename.Length - 4);
                                Logger.Info($"Output filename set to: {args[i]}");
                            }
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
                    documentParser.Parse();
                }
            }
            catch
                (Exception err)
            {
                Logger.Fatal(err);
                Console.WriteLine();
#if DEBUG
                Console.WriteLine(err.ToString());
#else
                Console.WriteLine(err.Message);
#endif
                Console.ReadKey();
            }

            Console.Write("Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}