using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class BuildTriggers
    {
        public override string ToString()
        {
            return "triggers";
        }

        public List<BuildTrigger> Trigger { get; set; }
    }
}