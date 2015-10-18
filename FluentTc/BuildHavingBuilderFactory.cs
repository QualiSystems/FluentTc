using FluentTc.Locators;

namespace FluentTc
{
    public interface IBuildHavingBuilderFactory
    {
        IBuildHavingBuilder CreateBuildHavingBuilder();
    }

    internal class BuildHavingBuilderFactory : IBuildHavingBuilderFactory
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;

        public BuildHavingBuilderFactory(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
        }

        public IBuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder(m_BuildConfigurationHavingBuilderFactory, this);
        }
    }
}