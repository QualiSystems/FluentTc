using System;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IBuildConfigurationRunner
    {
        void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters = null);
        void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent);
    }

    internal class BuildConfigurationRunner : IBuildConfigurationRunner
    {
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IAgentsRetriever m_AgentsRetriever;

        public BuildConfigurationRunner(IBuildConfigurationRetriever buildConfigurationRetriever, ITeamCityCaller teamCityCaller, IAgentsRetriever agentsRetriever)
        {
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
            m_TeamCityCaller = teamCityCaller;
            m_AgentsRetriever = agentsRetriever;
        }

        public void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters = null)
        {
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var properties = GetProperties(parameters);
            var body = CreateTriggerBody(buildConfiguration.Id, null, properties);
            m_TeamCityCaller.PostFormat(body, HttpContentTypes.ApplicationXml, "/app/rest/buildQueue");
        }

        public void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent)
        {
            var agent = m_AgentsRetriever.GetAgent(onAgent);
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var body = CreateTriggerBody(buildConfiguration.Id, agent.Id);
            m_TeamCityCaller.PostFormat(body, HttpContentTypes.ApplicationXml, "/app/rest/buildQueue");
        }

        private static Property[] GetProperties(Action<IBuildParameterValueBuilder> parameters)
        {
            if ( parameters == null ) return new Property[0];

            var buildParameterValueBuilder = new BuildParameterValueBuilder();
            parameters(buildParameterValueBuilder);
            return buildParameterValueBuilder.GetParameters();
        }

        private static string CreateTriggerBody(string buildConfigId, int? agentId, Property[] properties = null)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append(@"<build>").AppendLine()
                .AppendFormat(@"<buildType id=""{0}""/>", buildConfigId).AppendLine();

            if (agentId.HasValue)
            {
                bodyBuilder.AppendFormat(@"<agent id=""{0}""/>", agentId).AppendLine();
            }

            if (properties != null && properties.Any())
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