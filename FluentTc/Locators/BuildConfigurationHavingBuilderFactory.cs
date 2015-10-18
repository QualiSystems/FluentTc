namespace FluentTc.Locators
{
    public interface IBuildConfigurationHavingBuilderFactory
    {
        BuildConfigurationHavingBuilder CreateBuildConfigurationHavingBuilder();
    }

    public class BuildConfigurationHavingBuilderFactory : IBuildConfigurationHavingBuilderFactory
    {
        private readonly IBuildProjectHavingBuilderFactory m_buildProjectHavingBuilderFactory;

        public BuildConfigurationHavingBuilderFactory(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_buildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public BuildConfigurationHavingBuilder CreateBuildConfigurationHavingBuilder()
        {
            return new BuildConfigurationHavingBuilder(m_buildProjectHavingBuilderFactory);
        }
    }
}