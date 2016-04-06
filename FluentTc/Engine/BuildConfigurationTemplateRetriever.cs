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

        public BuildConfigurationTemplateRetriever(ITeamCityCaller teamCityCaller)
        {
            m_TeamCityCaller = teamCityCaller;
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