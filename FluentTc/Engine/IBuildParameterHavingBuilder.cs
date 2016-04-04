namespace FluentTc.Engine
{
    public interface IBuildParameterHavingBuilder
    {
        void ParameterName(string parameterName);
    }

    public class BuildParameterHavingBuilder : IBuildParameterHavingBuilder
    {
        private string m_ParameterName;

        public string GetLocator()
        {
            return m_ParameterName;
        }

        public void ParameterName(string parameterName)
        {
            m_ParameterName = parameterName;
        }
    }
}