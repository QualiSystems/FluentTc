using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class AgentRequirements
    {
        public override string ToString()
        {
            return "agent-requirements";
        }

        public List<AgentRequirement> AgentRequirement { get; set; }
    }
}