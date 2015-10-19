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
        private readonly IBranchHavingBuilderFactory m_BranchHavingBuilderFactory;
        private readonly BuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildHavingBuilderFactory(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, IUserHavingBuilderFactory userHavingBuilderFactory, IBranchHavingBuilderFactory branchHavingBuilderFactory, BuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_UserHavingBuilderFactory = userHavingBuilderFactory;
            m_BranchHavingBuilderFactory = branchHavingBuilderFactory;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public IBuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder(m_BuildConfigurationHavingBuilderFactory, this, m_UserHavingBuilderFactory, m_BranchHavingBuilderFactory, m_BuildProjectHavingBuilderFactory);
        }
    }
}