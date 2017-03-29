using System;
using System.Linq;
using HtmlAgilityPack;

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
            var body = Navigator.DocumentNode.SelectSingleNode("//body");
            var children = body.ChildNodes;
            foreach (var top in children)
            {
                if (top.Name.Equals("div", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("div");
                    foreach (var child in top.ChildNodes)
                    {
                        if (child.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("p");
                            if (child.InnerText.Contains("Item") && child.InnerText.Contains("Information"))
                            {
                                Console.WriteLine("Item Information");
                            }
                        }
                        else if (child.Name.Equals("table", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("table");
                        }
                    }
                }
            }
           
            Console.ReadKey();
        }
    }
}