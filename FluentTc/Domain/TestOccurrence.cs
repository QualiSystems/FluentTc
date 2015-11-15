namespace FluentTc.Domain
{
    public class TestOccurrence
    {
        public int Duration { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Details { get; set; }

        public Test Test { get; set; }
    }
}