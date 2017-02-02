namespace FluentTc.Domain
{
    using JsonFx.Json;

    public class ArtifactDependency
    {
        public override string ToString()
        {
            return "artifact_dependency";
        }

        public string Id { get; set; }

        public string Type { get; set; }

        public Properties Properties { get; set; }

        [JsonName("source-buildType")]
        public SourceBuildType SourceBuildType { get; set; }
    }
}