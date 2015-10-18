using System.Collections.Generic;

namespace FluentTc.Locators
{
    public class BuildConfigurationHavingBuilder
    {
        public BuildConfigurationHavingBuilder Id(string buildConfigurationId)
        {
            return this;
        }

        public BuildConfigurationHavingBuilder Name(string buildConfigurationName)
        {
            return this;
        }

        public IEnumerable<string> Get()
        {
            return new string[] {};
        }
    }
}