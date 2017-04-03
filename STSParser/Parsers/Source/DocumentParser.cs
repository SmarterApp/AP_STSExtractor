using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HtmlAgilityPack;
using STSParser.Models.Source;
using STSParser.Models.Source.Item;
using STSParser.Models.Source.Passage;
using STSParser.Utilities;

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
            var items = new List<Item>();
            var passages = new List<Passage>();
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
                        items.Add(new Item {Metadata = (ItemMetadata) MetadataParser.Parse(data)});
                        isItemMetadata = false;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isPassageMetadata)
                    {
                        var data = child.ChildNodes
                            .Where(x => x.Name.Equals("tr"))
                            .SelectMany(x => x.ChildNodes
                                .Where(y => y.Name.Equals("td")))
                            .ToList();
                        passages.Add(new Passage {Metadata = (PassageMetadata) MetadataParser.Parse(data, true)});
                        isPassageMetadata = false;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata &&
                             !isPassageMetadata)
                    {
                        var itemBody = new ItemBody();
                        var key = string.Empty;
                        foreach (
                            var p in
                            child.ChildNodes.Where(x => x.Name.Equals("tr", StringComparison.OrdinalIgnoreCase)).SelectMany(y =>
                                y.ChildNodes.Where(x => x.Name.Equals("td", StringComparison.OrdinalIgnoreCase))).SelectMany(z =>
                                z.ChildNodes.Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase))))
                        {

                            var answer = p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                .SelectMany(
                                    x =>
                                        x.ChildNodes.Where(
                                            y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                .FirstOrDefault(x => StringUtilities.MatchesCharacterInRange(x.InnerText, 'A', 'D'))?.InnerText;

                            if (!string.IsNullOrEmpty(answer))
                            {

                                var stringChoice =
                                    p.ChildNodes.FirstOrDefault(
                                        x => x.Name.Equals("span", StringComparison.OrdinalIgnoreCase));

                                if (stringChoice != null)
                                {
                                    itemBody.AnswerChoices.Add(answer, new BodyElement
                                    {
                                        Text = stringChoice.InnerText
                                    });
                                }
                                continue;
                            }

                            if (!string.IsNullOrEmpty(key))
                            {
                                var img =
                                    p.ChildNodes.First(
                                        x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase));
                                itemBody.AnswerChoices.Add(key, new BodyElement
                                {
                                    Image = Image.FromFile(img.InnerText)
                                });
                                key = string.Empty;
                            }
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}