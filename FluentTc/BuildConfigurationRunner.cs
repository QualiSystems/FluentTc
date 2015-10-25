using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IBuildConfigurationRunner
    {
        void Run(Action<IBuildConfigurationHavingBuilder> having);
    }

    internal class BuildConfigurationRunner : IBuildConfigurationRunner
    {
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public BuildConfigurationRunner(IBuildConfigurationRetriever buildConfigurationRetriever, ITeamCityCaller teamCityCaller)
        {
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
            m_TeamCityCaller = teamCityCaller;
        }

        public void Run(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var body = CreateTriggerBody(buildConfiguration.Id, null, new Property[0]);
            m_TeamCityCaller.PostFormat(body, HttpContentTypes.ApplicationXml, "/app/rest/buildQueue");
        }

        private static string CreateTriggerBody(string buildConfigId, int? agentId, Property[] properties)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append(@"<build>").AppendLine()
                .AppendFormat(@"<buildType id=""{0}""/>", buildConfigId).AppendLine();

            if (agentId.HasValue)
            {
                bodyBuilder.AppendFormat(@"<agent id=""{0}""/>", agentId).AppendLine();
            }

            if (properties.Any())
            {
                bodyBuilder.Append(@"<properties>").AppendLine();

                foreach (var property in properties)
                {
                    bodyBuilder.AppendFormat(@"<property name=""{0}"" value=""{1}""/>", property.Name, property.Value).AppendLine();
                }

                bodyBuilder.Append(@"</properties>").AppendLine();
            }

            bodyBuilder.Append("</build>").AppendLine();

            return bodyBuilder.ToString();
        }

    }
}