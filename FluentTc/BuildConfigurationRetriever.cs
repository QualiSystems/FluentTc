using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildConfigurationRetriever
    {
        IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);
    }

    internal class BuildConfigurationRetriever : IBuildConfigurationRetriever
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public BuildConfigurationRetriever(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, ITeamCityCaller teamCityCaller)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            var buildWrapper = m_TeamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes/{0}",
                buildConfigurationHavingBuilder.GetLocator());

            if (buildWrapper == null || buildWrapper.BuildType == null) return new List<BuildConfiguration>();

            return buildWrapper.BuildType;
        }
    }
}