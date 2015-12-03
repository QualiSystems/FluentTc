using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildHavingBuilderFactory
    {
        BuildHavingBuilder CreateBuildHavingBuilder();
    }

    internal class BuildHavingBuilderFactory : IBuildHavingBuilderFactory
    {
        private readonly IUserHavingBuilderFactory m_UserHavingBuilderFactory;
        private readonly IBranchHavingBuilderFactory m_BranchHavingBuilderFactory;
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;
        private readonly ILocatorBuilder m_LocatorBuilder;

        public BuildHavingBuilderFactory(IUserHavingBuilderFactory userHavingBuilderFactory, IBranchHavingBuilderFactory branchHavingBuilderFactory, IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory, ILocatorBuilder locatorBuilder)
        {
            m_UserHavingBuilderFactory = userHavingBuilderFactory;
            m_BranchHavingBuilderFactory = branchHavingBuilderFactory;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
            m_LocatorBuilder = locatorBuilder;
        }

        public BuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder(this, m_UserHavingBuilderFactory, m_BranchHavingBuilderFactory, m_BuildProjectHavingBuilderFactory, m_LocatorBuilder);
        }
    }
}