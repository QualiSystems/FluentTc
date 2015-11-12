using System;
using EasyHttp.Http;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    public interface IBuildTemplateAttacher
    {
        void Attach(Action<IBuildConfigurationHavingBuilder> having, string buildTemplateId);
    }

    internal class BuildTemplateAttacher : IBuildTemplateAttacher
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public BuildTemplateAttacher(ITeamCityCaller teamCityCaller, IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
        }

        public void Attach(Action<IBuildConfigurationHavingBuilder> having, string buildTemplateId)
        {
            var buildConfigurationHavingBuilder = m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);
            m_TeamCityCaller.PutFormat(buildTemplateId, HttpContentTypes.TextPlain, "/app/rest/buildTypes/{0}/template", buildConfigurationHavingBuilder.GetLocator());
        }
    }
}