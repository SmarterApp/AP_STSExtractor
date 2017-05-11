using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using STSCommon.Extensions;

namespace STSCommon.Test.Extensions
{
    [TestFixture]
    public class XmlElementExtensions
    {
        [Test]
        public void TestAddingChildElement()
        {
            // Arrange
            var document = new XmlDocument();
            var parentNode = document.CreateElement("test");
            const string nodeName = "img";
            var attributes = new Dictionary<string, string>
            {
                {
                    "src",
                    "puppy.jpg"
                },
                {
                    "height",
                    "720"
                }
            };

            // Act
            parentNode.AppendChild(document, nodeName, string.Empty, attributes);

            // Assert
            Assert.IsTrue(parentNode.HasChildNodes);
            Assert.AreEqual(parentNode.FirstChild.Name, nodeName);
            Assert.IsTrue(((XmlElement) parentNode.FirstChild).HasAttribute("src"));
            Assert.IsTrue(((XmlElement) parentNode.FirstChild).HasAttribute("height"));
            Assert.AreEqual(((XmlElement) parentNode.FirstChild).Attributes["src"].InnerText, "puppy.jpg");
            Assert.AreEqual(((XmlElement) parentNode.FirstChild).Attributes["height"].InnerText, "720");
        }
    }
}