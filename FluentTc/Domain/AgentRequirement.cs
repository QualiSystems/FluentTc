namespace FluentTc.Domain
{
    public class AgentRequirement
    {
        public override string ToString()
        {
            return "agent_requirement";
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public Properties Properties { get; set; }
    }
}