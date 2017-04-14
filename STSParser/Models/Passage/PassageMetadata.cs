namespace STSParser.Models.Passage
{
    public class PassageMetadata : StsMetadata
    {
        public PassageMetadata()
        {
            Add("PassageCode", string.Empty);
            Add("PassageTitle", string.Empty);
        }
    }
}