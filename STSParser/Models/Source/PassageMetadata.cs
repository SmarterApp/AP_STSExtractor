namespace STSParser.Models.Source
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