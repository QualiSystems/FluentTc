namespace FluentTc.Domain
{
    public interface ITestOccurrences
    {
        int Count { get; }
        string Href { get; }
        int Passed { get; }
        int Failed { get; }
        int NewFailed { get; }
        int Muted { get; }
        int Ignored { get; }
    }

    public class TestOccurrences : ITestOccurrences
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