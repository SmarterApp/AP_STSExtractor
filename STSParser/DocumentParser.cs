using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using STSParser.Models;
using STSParser.Utilities;

namespace STSParser
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
                var isMetadata = false;
                foreach (var child in top.ChildNodes)
                {
                    if (child.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (child.InnerText.Contains("Item") && child.InnerText.Contains("Information"))
                        {
                            isMetadata = true;
                        }
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isMetadata)
                    {
                        var data = child.ChildNodes
                            .Where(x => x.Name.Equals("tr"))
                            .SelectMany(x => x.ChildNodes
                                .Where(y => y.Name.Equals("td")))
                            .ToList();
                        var metadata = new ItemMetadata();
                        for (var i = 0; i < data.Count;)
                        {
                            if (metadata.Keys.Contains(data[i].InnerText.RemoveSpecialCharacters()))
                            {
                                metadata.AddItemMetadata(data[i].InnerText.RemoveSpecialCharacters(),
                                    data[i + 1].InnerText.RemoveSpecialCharacters());
                                i += 2;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        itemMetadata.Add(metadata);
                        isMetadata = false;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isMetadata)
                    {
                        Console.WriteLine("We need the question's info!");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}