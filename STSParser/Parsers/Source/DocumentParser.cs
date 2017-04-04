using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using STSCommon;
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
            var isItem = true;
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
                        items.Add(new Item {Metadata = (ItemMetadata) MetadataParser.Parse(child)});
                        isItemMetadata = false;
                        isItem = true;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && isPassageMetadata)
                    {
                        passages.Add(new Passage {Metadata = (PassageMetadata) MetadataParser.Parse(child, true)});
                        isPassageMetadata = false;
                        isItem = false;
                    }
                    else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase) && !isItemMetadata &&
                             !isPassageMetadata)
                    {
                        var itemBody = new ItemBody();
                        var passageBody = new PassageBody();
                        foreach (
                            var p in
                            child.ChildNodes.Where(x => x.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
                                .SelectMany(y =>
                                    y.ChildNodes.Where(x => x.Name.Equals("td", StringComparison.OrdinalIgnoreCase)))
                                .SelectMany(z =>
                                    z.ChildNodes.Where(x => x.Name.Equals("p", StringComparison.OrdinalIgnoreCase))))
                        {
                            if (isItem)
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
                        }
                        if (isItem)
                        {
                            items.Last().Body = itemBody;
                        }
                        else
                        {
                            passages.Last().Body = passageBody;
                        }
                    }
                }
            }
            var test = items.SelectMany(x => x.Body.AnswerChoices).Where(x => x.Value.IsResource());
            Console.ReadKey();
        }
    }
}