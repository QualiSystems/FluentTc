using System;

namespace FluentTc.Locators
{
    public interface ILocatorBuilder
    {
        string GetBuildConfigurationLocator(Action<IBuildConfigurationHavingBuilder> havingBuildConfig);
        string GetProjectLocator(Action<IBuildProjectHavingBuilder> projectHavingBuilder);
    }

    public class LocatorBuilder : ILocatorBuilder
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public LocatorBuilder(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public string GetBuildConfigurationLocator(Action<IBuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            havingBuildConfig.Invoke(buildConfigurationHavingBuilder);
            var locator = buildConfigurationHavingBuilder.GetLocator();
            return locator;
        }

        public string GetProjectLocator(Action<IBuildProjectHavingBuilder> projectHavingBuilder)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            projectHavingBuilder(buildProjectHavingBuilder);
            var locator = buildProjectHavingBuilder.GetLocator();
            return locator;
        }
    }
}