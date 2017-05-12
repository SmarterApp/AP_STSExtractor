using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;
using STSCommon.Utilities;

namespace STSCommon.Test.Utilities
{
    [TestFixture]
    public class ImageUtilitiesTests
    {
        public static readonly string ResourcesDirectory =
            Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Resources");

        [Test]
        public void AttemptedRetrievalFromBadNodeSrcThrowsArgumentException()
        {
            // Arrange
            var document = new HtmlDocument();
            document.LoadHtml("<img />");
            var imageNode = document.DocumentNode.SelectSingleNode("//img");
            var path = Path.Combine(ResourcesDirectory, "puppy.jpg");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageUtilities.ImageFromParentHtmlNode(path, imageNode));
        }

        [Test]
        public void AttemptedRetrievalFromBadPathThrowsArgumentException()
        {
            // Arrange
            var document = new HtmlDocument();
            document.LoadHtml("<img src=\"test/puppy.jpg\" />");
            var imageNode = document.DocumentNode.SelectSingleNode("//img");
            var path = Path.Combine(ResourcesDirectory, "test/puppy.jpg");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageUtilities.ImageFromParentHtmlNode(path, imageNode));
        }

        [Test]
        public void RetrieveValidImageFromValidNode()
        {
            // Arrange
            var document = new HtmlDocument();
            document.LoadHtml("<img src=\"puppy.jpg\" />");
            var imageNode = document.DocumentNode.SelectSingleNode("//img");
            var path = Path.Combine(ResourcesDirectory, "puppy.jpg");

            // Act
            var result = ImageUtilities.ImageFromParentHtmlNode(path, imageNode);

            // Assert
            Assert.IsTrue(result.RawFormat.Equals(ImageFormat.Jpeg));
            Assert.IsTrue(result.Size.Equals(new Size(720, 720)));
        }
    }
}