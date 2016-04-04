using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using FluentTc.Domain;
using FluentTc.Exceptions;
using FluentTc.Locators;
using JsonFx.Json;

namespace FluentTc.Engine
{
    internal interface IBuildConfigurationRetriever
    {
        IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);
        void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        BuildConfiguration GetSingleBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        void DeleteBuildConfigurationParameter(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterHavingBuilder> parameterName);
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

        public IList<BuildConfiguration> RetrieveBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            try
            {
                var buildWrapper = m_TeamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator={0}",
                    buildConfigurationHavingBuilder.GetLocator());
                if (buildWrapper == null || buildWrapper.BuildType == null) return new List<BuildConfiguration>();
                return buildWrapper.BuildType;
            }
            catch (HttpException httpException)
            {
                if (httpException.StatusCode == HttpStatusCode.NotFound) return new List<BuildConfiguration>();
                throw;
            }
        }

        public BuildConfiguration GetSingleBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfigurations = RetrieveBuildConfigurations(having);
            if (!buildConfigurations.Any()) throw new BuildConfigurationNotFoundException();
            if (buildConfigurations.Count() > 1) throw new MoreThanOneBuildConfigurationFoundException();
            return GetSingleBuildConfiguration(buildConfigurations.Single().Id);
        }

        public void DeleteBuildConfigurationParameter(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterHavingBuilder> parameterName)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            var buildParameterHavingBuilder = new BuildParameterHavingBuilder();
            parameterName(buildParameterHavingBuilder);

            m_TeamCityCaller.DeleteFormat("/app/rest/buildTypes/{0}/parameters/{1}", buildConfigurationHavingBuilder.GetLocator(), buildParameterHavingBuilder.GetLocator());
        }

        private BuildConfiguration GetSingleBuildConfiguration(string buildTypeId)
        {
            return m_TeamCityCaller.GetFormat<BuildConfiguration>("/app/rest/buildTypes/id:{0}", buildTypeId);
        }

        public void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            var buildConfigurationHavingBuilder =
                m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            var writer = new JsonWriter();

            BuildParameterValueBuilder buildParameterValueBuilder = new BuildParameterValueBuilder();
            parameters(buildParameterValueBuilder);
            buildParameterValueBuilder.GetParameters()
                .ForEach(
                    p =>
                        m_TeamCityCaller.PutFormat(writer.Write(p), HttpContentTypes.ApplicationJson,
                            "/app/rest/buildTypes/{0}/parameters/{1}", buildConfigurationHavingBuilder.GetLocator(), p.Name));
        }
    }
}