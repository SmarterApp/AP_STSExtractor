namespace STSParser.Models.Source.Item
{
    public class ItemMetadata : StsMetadata
    {
        public ItemMetadata()
        {
            Add("ItemCode", string.Empty);
            Add("DOK", string.Empty);
            Add("StandardCode", string.Empty);
            Add("Strand", string.Empty);
            Add("ReportCategory", string.Empty);
            Add("Standard", string.Empty);
            Add("CorrectAnswer", string.Empty);
            Add("PassageTitle", string.Empty);
            Add("PassageCode", string.Empty);
        }
    }
}