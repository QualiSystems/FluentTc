using System.Collections.Generic;

namespace FluentTc.Locators
{
    public class BuildConfigurationHavingBuilder
    {
        public BuildConfigurationHavingBuilder ConfigurationId(int buildConfigurationId)
        {
            return this;
        }

        public BuildConfigurationHavingBuilder ConfigurationName(string buildConfigurationName)
        {
            return this;
        }

        public IEnumerable<string> Get()
        {
            return new string[] {};
        }
    }
}