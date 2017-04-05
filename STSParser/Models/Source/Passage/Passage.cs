namespace STSParser.Models.Source.Passage
{
    public class Passage
    {
        public Passage()
        {
            Metadata = new PassageMetadata();
            Body = new PassageBody();
        }

        public PassageMetadata Metadata { get; set; }
        public PassageBody Body { get; set; }
    }
}