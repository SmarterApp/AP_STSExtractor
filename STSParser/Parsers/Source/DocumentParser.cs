using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using STSCommon;
using STSCommon.Utilities;
using STSParser.Models.Source;
using STSParser.Models.Source.Item;
using STSParser.Models.Source.Passage;

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
            foreach (var div in children.Where(x => x.Name.Equals("div", StringComparison.OrdinalIgnoreCase)))
            {
                var isItemMetadata = false;
                var nodes = div.ChildNodes;
                for (var i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Name.Equals("h1", StringComparison.OrdinalIgnoreCase))
                    {
                        var remainingParagraphNodes =
                            nodes.Skip(i)
                                .Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase)).ToList();
                        passages.Add(PassageParser.Parse(remainingParagraphNodes));
                        break;
                    }
                    if (nodes[i].Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodes[i].InnerText.Contains("Item")
                            && nodes[i].InnerText.Contains("Information"))
                        {
                            isItemMetadata = true;
                        }
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isItemMetadata)
                    {
                        items.Add(new Item {Metadata = (ItemMetadata) ItemMetadataParser.Parse(nodes[i])});
                        isItemMetadata = false;
                    }
                    else if (nodes[i].Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata)
                    {
                        var itemBody = new ItemBody();
                        foreach (
                            var p in
                            nodes[i].ChildNodes.Where(x => x.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
                                .SelectMany(y =>
                                    y.ChildNodes.Where(x => x.Name.Equals("td", StringComparison.OrdinalIgnoreCase)))
                                .SelectMany(z =>
                                    z.ChildNodes.Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase))))
                        {
                            var answer =
                                p.ChildNodes.Where(x => x.Name.Equals("b", StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(
                                        x =>
                                            x.ChildNodes.Where(
                                                y => y.Name.Equals("span", StringComparison.OrdinalIgnoreCase)))
                                    .FirstOrDefault(
                                        x => StringUtilities.MatchesCharacterInRange(x.InnerText, 'A', 'D'))?
                                    .InnerText.Trim();

                            if (!string.IsNullOrEmpty(answer))
                            {
                                var stringChoice =
                                    p.ChildNodes.FirstOrDefault(
                                            x => x.Name.Equals("span", StringComparison.OrdinalIgnoreCase))?
                                        .InnerText.Trim();
                                var imageChoice =
                                    p.ChildNodes.FirstOrDefault(
                                            x => x.Name.Equals("img", StringComparison.OrdinalIgnoreCase))?
                                        .GetAttributeValue("src", string.Empty);
                                if (!string.IsNullOrEmpty(stringChoice) && imageChoice == null)
                                {
                                    itemBody.AnswerChoices.Add(answer, new BodyElement
                                    {
                                        Text = stringChoice
                                    });
                                }
                                else if (!string.IsNullOrEmpty(imageChoice))
                                {
                                    var path = Path.Combine(new FileInfo(ExtractionSettings.Input).DirectoryName,
                                        imageChoice.ReverseSlashes());
                                    itemBody.AnswerChoices.Add(answer, new BodyElement
                                    {
                                        Image = Image.FromFile(path)
                                    });
                                }
                            }
                        }
                        items.Last().Body = itemBody;
                    }
                }
            }
            var test = items.SelectMany(x => x.Body.AnswerChoices).Where(x => x.Value.IsResource());
            Console.ReadKey();
        }
    }
}