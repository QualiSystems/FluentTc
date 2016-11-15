namespace FluentTc.Domain
{
    using System.Collections.Generic;

    public class RevisionsWrapper
    {
        public int Count { get; set; }
        public List<Change> Revision { get; set; }
    }
}
