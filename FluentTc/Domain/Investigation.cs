namespace FluentTc.Domain
{
    public class Investigation
    {
        public User Assignee { get; set; }

        public Assignment Assignment { get; set; }

        public TestOccurrenceWrapper TargetWrap { get; set; }

        public ResolutionWrapper Resolution { get; set; }

        public string State { get; set; }
    }
}