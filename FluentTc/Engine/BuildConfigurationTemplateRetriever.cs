using System;
using System.Collections.Generic;
using System.Net;
using EasyHttp.Infrastructure;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildConfigurationTemplateRetriever
    {
        List<BuildConfiguration> GetAllBuildConfigurationTemplates();
        BuildConfiguration GetBuildConfigurationTemplate(Action<IBuildConfigurationTemplateHavingBuilder> having);
    }

    internal class BuildConfigurationTemplateRetriever : IBuildConfigurationTemplateRetriever
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;

        public BuildConfigurationTemplateRetriever(ITeamCityCaller teamCityCaller, IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
        }

        public List<BuildConfiguration> GetAllBuildConfigurationTemplates()
        {
            try
            {
                var buildWrapper = m_TeamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true");
                if (buildWrapper == null || buildWrapper.BuildType == null) return new List<BuildConfiguration>();
                return buildWrapper.BuildType;
            }
            catch (HttpException httpException)
            {
                if (httpException.StatusCode == HttpStatusCode.NotFound) return new List<BuildConfiguration>();
                throw;
            }
        }

        public BuildConfiguration GetBuildConfigurationTemplate(Action<IBuildConfigurationTemplateHavingBuilder> having)
        {
            var buildConfigurationHavingBuilder = new BuildConfigurationTemplateHavingBuilder();
            having(buildConfigurationHavingBuilder);
            var locator = buildConfigurationHavingBuilder.GetLocator();

            return m_TeamCityCaller.GetFormat<BuildConfiguration>("/app/rest/buildTypes/{0}", locator);
        }
    }
}