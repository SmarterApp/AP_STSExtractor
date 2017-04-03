using System.Linq;

namespace STSParser.Utilities
{
    public static class StringUtilities
    {
        public static bool MatchesCharacterInRange(string str, char min, char max)
        {
            var item = str.Trim();
            return item.Length == 1 && item.All(x => x >= min && x <= max);
        }
    }
}