namespace FluentTc.Locators
{
    public interface IBuildConfigurationHavingBuilderFactory
    {
        IBuildConfigurationHavingBuilder CreateBuildConfigurationHavingBuilder();
    }

    public class BuildConfigurationHavingBuilderFactory : IBuildConfigurationHavingBuilderFactory
    {
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildConfigurationHavingBuilderFactory(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public IBuildConfigurationHavingBuilder CreateBuildConfigurationHavingBuilder()
        {
            return new BuildConfigurationHavingBuilder(m_BuildProjectHavingBuilderFactory);
        }
    }
}