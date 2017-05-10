using System.Linq;
using NUnit.Framework;
using STSCommon.Utilities;

namespace STSCommon.Test.Utilities
{
    [TestFixture]
    public class StringUtilitiesTests
    {
        [Test]
        public void MatchCharactersInRangeValidMatchShouldReturnTrue()
        {
            // Arrange
            const string input = "A";

            // Act
            var result = StringUtilities.MatchesCharacterInRange(input, 'A', 'A');

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MatchCharactersInRangeValidWithExtraWhitespaceMatchShouldReturnTrue()
        {
            // Arrange
            const string input = "  A  \n\t";

            // Act
            var result = StringUtilities.MatchesCharacterInRange(input, 'A', 'A');

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void MatchCharactersInRangeUnmatchedCharShouldReturnFalse()
        {
            // Arrange
            const string input = "a";

            // Act
            var result = StringUtilities.MatchesCharacterInRange(input, 'A', 'A');

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MatchCharactersInRangeLongStringShouldReturnFalse()
        {
            // Arrange
            const string input = "AAA";

            // Act
            var result = StringUtilities.MatchesCharacterInRange(input, 'A', 'A');

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MatchCharactersInRangeMultipleExecutionsOverValidCharactersShouldReturnTrue()
        {
            // Arrange
            const string input = "ABCDEFGHIJK";

            // Act
            var result = input.All(x => StringUtilities.MatchesCharacterInRange(x.ToString(), 'A', 'K'));

            // Assert
            Assert.IsTrue(result);
        }
    }
}