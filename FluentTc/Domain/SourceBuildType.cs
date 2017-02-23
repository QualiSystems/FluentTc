namespace FluentTc.Domain
{
    public class SourceBuildType
    {
        public override string ToString()
        {
            return "source-buildType";
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
    }
}