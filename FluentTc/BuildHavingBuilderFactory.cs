using FluentTc.Locators;

namespace FluentTc
{
    public interface IBuildHavingBuilderFactory
    {
        IBuildHavingBuilder CreateBuildHavingBuilder();
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

        public IBuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder(this, m_UserHavingBuilderFactory, m_BranchHavingBuilderFactory, m_BuildProjectHavingBuilderFactory, m_LocatorBuilder);
        }
    }
}