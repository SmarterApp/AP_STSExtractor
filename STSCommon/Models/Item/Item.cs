namespace STSCommon.Models.Item
{
    public class Item
    {
        public ItemMetadata Metadata { get; set; }
        public ItemBody Body { get; set; }
        public string Id { get; set; }
        public string PassageId { get; set; }
    }
}