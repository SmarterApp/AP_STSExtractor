using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using STSParser.Models.Source;

namespace STSParser.Parsers.Source
{
    public class DocumentParser
    {
        public DocumentParser(HtmlDocument navigator)
        {
            Navigator = navigator;
        }

        private HtmlDocument Navigator { get; }

        public void Parse()
        {
            var itemMetadata = new List<ItemMetadata>();
            var body = Navigator.DocumentNode.SelectSingleNode("//body");
            var children = body.ChildNodes;
            foreach (var top in children)
            {
                if (!top.Name.Equals("div", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                var isItemMetadata = false;
                var isPassageMetadata = false;
                foreach (var child in top.ChildNodes)
                {
                    if (child.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (child.InnerText.Contains("Item") && child.InnerText.Contains("Information"))
                        {
                            isItemMetadata = true;
                        }
                        else if (child.InnerText.Contains("Passage") && child.InnerText.Contains("Information"))
                        {
                            isPassageMetadata = true;
                        }
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isItemMetadata)
                    {
                        var data = child.ChildNodes
                            .Where(x => x.Name.Equals("tr"))
                            .SelectMany(x => x.ChildNodes
                                .Where(y => y.Name.Equals("td")))
                            .ToList();
                        itemMetadata.Add((ItemMetadata) MetadataParser.Parse(data));
                        isItemMetadata = false;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isPassageMetadata)
                    {
                        var data = child.ChildNodes
                            .Where(x => x.Name.Equals("tr"))
                            .SelectMany(x => x.ChildNodes
                                .Where(y => y.Name.Equals("td")))
                            .ToList();
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata &&
                             !isPassageMetadata)
                    {
                        foreach (
                            var p in
                            child.ChildNodes.First(x => x.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
                                .ChildNodes.First(x => x.Name.Equals("td", StringComparison.OrdinalIgnoreCase))
                                .ChildNodes.Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase)))
                        {
                            if (
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .Any(x => x.InnerText.Trim().Equals("A")))
                            {
                                Console.WriteLine("We have an A!");
                            }
                            else if (
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .Any(x => x.InnerText.Trim().Equals("B")))
                            {
                                Console.WriteLine("We have an B!");
                            }
                            else if (
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .Any(x => x.InnerText.Trim().Equals("C")))
                            {
                                Console.WriteLine("We have an C!");
                            }
                            else if (
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .Any(x => x.InnerText.Trim().Equals("D")))
                            {
                                Console.WriteLine("We have an D!");
                            }
                            else if (
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .Any(x => x.InnerText.Trim().Equals("E")))
                            {
                                Console.WriteLine("We have an E!");
                            }
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}