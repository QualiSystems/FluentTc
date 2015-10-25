using System;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IBuildConfigurationCreator
    {
        BuildConfiguration Create(Action<IBuildProjectHavingBuilder> having, string buildConfigurationName);
    }

    internal class BuildConfigurationCreator : IBuildConfigurationCreator
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildConfigurationCreator(ITeamCityCaller teamCityCaller, IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public BuildConfiguration Create(Action<IBuildProjectHavingBuilder> having, string buildConfigurationName)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            return m_TeamCityCaller.PostFormat<BuildConfiguration>(buildConfigurationName, HttpContentTypes.TextPlain, HttpContentTypes.ApplicationJson, "/app/rest/projects/{0}/buildTypes", buildProjectHavingBuilder.GetLocator());
        }
    }
}