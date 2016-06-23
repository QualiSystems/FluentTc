using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildConfigurationRunner
    {
        void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent = null, Action<IBuildParameterValueBuilder> parameters = null,
            Action<IMoreOptionsHavingBuilder> moreOptions = null);
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

        public void Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent = null, Action<IBuildParameterValueBuilder> parameters = null,
            Action<IMoreOptionsHavingBuilder> moreOptionsAction = null)
        {
            var agentId = GetAgentId(onAgent);
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var moreOptions = GetMoreOptions(moreOptionsAction);
            var body = CreateTriggerBody(buildConfiguration.Id, agentId, GetProperties(parameters), moreOptions);
            m_TeamCityCaller.PostFormat(body, HttpContentTypes.ApplicationXml, "/app/rest/buildQueue");
        }

        private static MoreOptionsHavingBuilder GetMoreOptions(Action<IMoreOptionsHavingBuilder> moreOptionsAction)
        {
            var moreOptions = new MoreOptionsHavingBuilder();
            if (moreOptionsAction == null)
            {
                return moreOptions;    
            }
            moreOptionsAction(moreOptions);
            return moreOptions;
        }

        private int? GetAgentId(Action<IAgentHavingBuilder> onAgent)
        {
            if (onAgent == null) return null;
            return m_AgentsRetriever.GetAgent(onAgent).Id;
        }

        private static List<Property> GetProperties(Action<IBuildParameterValueBuilder> parameters)
        {
            if ( parameters == null ) return new List<Property>();

            var buildParameterValueBuilder = new BuildParameterValueBuilder();
            parameters(buildParameterValueBuilder);
            return buildParameterValueBuilder.GetParameters();
        }

        private static string CreateTriggerBody(string buildConfigId, int? agentId, List<Property> properties = null, MoreOptionsHavingBuilder moreOptions = null)
        {
            var bodyBuilder = new StringBuilder();

            if (moreOptions != null &&
                moreOptions.TriggeringOptions.Personal == true)
            {
                bodyBuilder.Append(@"<build personal=""true"">").AppendLine();
            }
            else
            {
                bodyBuilder.Append(@"<build>").AppendLine();
            }
            
            bodyBuilder.AppendFormat(@"<buildType id=""{0}""/>", buildConfigId).AppendLine();

            if (agentId.HasValue)
            {
                bodyBuilder.AppendFormat(@"<agent id=""{0}""/>", agentId).AppendLine();
            }

            if (moreOptions != null &&
                moreOptions.GetComment() != null)
            {
                bodyBuilder.AppendFormat(@"<comment><text>{0}</text></comment>", moreOptions.GetComment()).AppendLine();
            }

            if (moreOptions != null &&
                (moreOptions.TriggeringOptions.CleanSources == true || 
                 moreOptions.TriggeringOptions.QueueAtTop == true || 
                 moreOptions.TriggeringOptions.RebuildAllDependencies == true))
            {
                bodyBuilder.Append(@"<triggeringOptions ");
                if (moreOptions.TriggeringOptions.CleanSources == true)
                    bodyBuilder.Append(@"cleanSources=""true"" ");
                if (moreOptions.TriggeringOptions.RebuildAllDependencies == true)
                    bodyBuilder.Append(@"rebuildAllDependencies=""true"" ");
                if (moreOptions.TriggeringOptions.QueueAtTop == true)
                    bodyBuilder.Append(@"queueAtTop=""true"" ");
                bodyBuilder.Append(@"/>").AppendLine();
            }

            if (properties != null && properties.Any())
            {
                bodyBuilder.Append(@"<properties>").AppendLine();

                foreach (var property in properties)
                {
                    bodyBuilder.AppendFormat(@"<property name=""{0}"" value=""{1}""", property.Name, property.Value);
                    if (property.Type != null && !string.IsNullOrEmpty(property.Type.RawValue))
                    {
                        bodyBuilder.Append(">").AppendLine();
                        bodyBuilder.AppendFormat(@"<type rawValue=""{0}""/>", property.Type.RawValue).AppendLine();
                        bodyBuilder.Append("</property>").AppendLine();
                    }
                    else
                    {
                        bodyBuilder.Append("/>").AppendLine();
                    }
                }

                bodyBuilder.Append(@"</properties>").AppendLine();
            }

            bodyBuilder.Append("</build>").AppendLine();

            return bodyBuilder.ToString();
        }

    }
}