using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class ArtifactDependencies
    {
        public override string ToString()
        {
            return "artifact-dependencies";
        }

        public List<ArtifactDependency> ArtifactDependency { get; set; }
    }
}