using System;
using System.Collections.Generic;
using System.Linq;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildConfigurationRetriever
    {
        IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having,
            Action<BuildConfigurationPropertyBuilder> include);
        void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        BuildConfiguration GetSingleBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
    }

    internal class BuildConfigurationRetriever : IBuildConfigurationRetriever
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public BuildConfigurationRetriever(
            IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory,
            ITeamCityCaller teamCityCaller)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having,
            Action<BuildConfigurationPropertyBuilder> include)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            var buildWrapper = m_TeamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes/{0}",
                buildConfigurationHavingBuilder.GetLocator());

            if (buildWrapper == null || buildWrapper.BuildType == null) return new List<BuildConfiguration>();

            return buildWrapper.BuildType;
        }

        public BuildConfiguration GetSingleBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfigurations = RetrieveBuildConfigurations(having, i=>i.IncludeDefaults());
            if (!buildConfigurations.Any()) throw new BuildConfigurationNotFoundException();
            if (buildConfigurations.Count() > 1) throw new MoreThanOneBuildConfigurationFoundException();
            return buildConfigurations.Single();
        }

        public void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            IBuildParameterValueBuilder buildParameterValueBuilder = new BuildParameterValueBuilder();
            parameters(buildParameterValueBuilder);
            buildParameterValueBuilder.GetParameters().ToList()
                .ForEach(
                    p =>
                        m_TeamCityCaller.PutFormat(p.Value, HttpContentTypes.TextPlain,
                            "/app/rest/buildTypes/{0}/parameters/{1}", buildConfigurationHavingBuilder.GetLocator(),
                            p.Name));
        }
    }
}