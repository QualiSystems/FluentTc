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
        private readonly IUserHavingBuilderFactory m_UserHavingBuilderFactory;

        public BuildHavingBuilderFactory(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, IUserHavingBuilderFactory userHavingBuilderFactory)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_UserHavingBuilderFactory = userHavingBuilderFactory;
        }

        public IBuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder(m_BuildConfigurationHavingBuilderFactory, this, m_UserHavingBuilderFactory);
        }
    }
}