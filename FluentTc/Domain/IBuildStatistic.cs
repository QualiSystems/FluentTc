namespace FluentTc.Domain
{
    public interface IBuildStatistic
    {
        string Name { get; }
        string Value { get; }
    }

    public class BuildStatistic : IBuildStatistic
    {
        private readonly string m_Name;
        private readonly string m_Value;

        public BuildStatistic(string name, string value)
        {
            m_Name = name;
            m_Value = value;
        }

        public string Name
        {
            get { return m_Name; }
        }

        public string Value
        {
            get { return m_Value; }
        }
    }
}