using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class BuildSteps
    {
        public override string ToString()
        {
            return "steps";
        }

        public List<BuildStep> Step { get; set; }
    }
}