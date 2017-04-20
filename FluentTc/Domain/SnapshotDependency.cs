namespace FluentTc.Domain
{
    using JsonFx.Json;

    public class SnapshotDependency
    {
        public override string ToString()
        {
            return "snapshot_dependency";
        }

        public string Id { get; set; }

        public string Type { get; set; }

        public Properties Properties { get; set; }

        [JsonName("source-buildType")]
        public SourceBuildType SourceBuildType { get; set; }
    }
}