namespace FluentTc.Domain
{
    public class Build
    {
        public int Id { get; set; }
        public string BuildTypeId { get; set; }
        public string State { get; set; }
        public string Href { get; set; }
    }
}