namespace FluentTc.Domain
{
    public class TestOccurrences
    {
        public int Count { get; set; }

        public string Href { get; set; }

        public int Passed { get; set; }
        public int Failed { get; set; }
        public int NewFailed { get; set; }
        public int Muted { get; set; }
        public int Ignored { get; set; }
    }
}