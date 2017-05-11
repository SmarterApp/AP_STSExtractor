using NUnit.Framework;
using STSCommon.Extensions;

namespace STSCommon.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void ProduceSingleAttributeImgElementFromImgElementTest()
        {
            // Arrange
            const string testString = "<img width=313 height=169 src = \"STS_RLASimulatedItems_files/image001.png\" />";

            // Act
            var result = testString.ProduceImgElementWithSourceFromStringElement("puppy.jpg");

            // Assert
            Assert.AreEqual(result, "<img src=\"puppy.jpg\">");
        }

        [Test]
        public void RemoveSpecialCharactersTest()
        {
            // Arrange
            const string testString = "~!@#$%^&*()4WEaZ..,;: ";

            // Act
            var result = testString.RemoveSpecialCharacters();

            // Assert
            Assert.AreEqual(result, "4WEaZ..");
        }

        [Test]
        public void RestrictToSingleWhitespaceTest()
        {
            // Arrange
            const string testString = "  TEST  2Space   ThreeSpace  \n\t";

            // Act
            var result = testString.RestrictToSingleWhiteSpace();

            // Assert
            Assert.AreEqual(result, "TEST 2Space ThreeSpace");
        }
    }
}