using System;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;

namespace STSCommon.Test.Utilities
{
    [TestFixture]
    public class ImageUtilitiesTests
    {
        [SetUp]
        public void Setup()
        {
            ExtractionSettings.Input = Path.Combine(ResourcesDirectory, "puppy.jpg");
        }

        public static readonly string ResourcesDirectory =
            Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Resources");

        public void RetrieveValidImageFromValidNode()
        {
            // Arrange
            const string stringNode = "<img src=\"puppy.jpg\" />";
            var document = new HtmlDocument();
            document.LoadHtml(stringNode);
            var imageNode = document.DocumentNode.SelectSingleNode("//img");

            // Act
            // Assert
        }
    }
}