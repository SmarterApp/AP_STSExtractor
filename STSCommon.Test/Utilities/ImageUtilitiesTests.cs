using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;
using STSCommon.Extensions;
using STSCommon.Utilities;

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

        [Test]
        public void RetrieveValidImageFromValidNode()
        {
            // Arrange
            var document = new HtmlDocument();
            document.LoadHtml("<img src=\"puppy.jpg\" />");
            var imageNode = document.DocumentNode.SelectSingleNode("//img");

            // Act
            var result = ImageUtilities.ImageFromParentHtmlNode(imageNode);

            // Assert
            Assert.IsTrue(result.RawFormat.Equals(ImageFormat.Jpeg));
            Assert.IsTrue(result.Size.Equals(new Size(720,720)));
        }
    }
}