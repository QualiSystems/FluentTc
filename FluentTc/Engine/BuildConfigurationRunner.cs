using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildConfigurationRunner
    {
        IBuild Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent = null, Action<IBuildParameterValueBuilder> parameters = null,
            Action<IMoreOptionsHavingBuilder> moreOptions = null);
    }

    internal class BuildConfigurationRunner : IBuildConfigurationRunner
    {
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IAgentsRetriever m_AgentsRetriever;
        private readonly IBuildModelToBuildConverter m_BuildModelToBuildConverter;

        public BuildConfigurationRunner(IBuildConfigurationRetriever buildConfigurationRetriever,
            ITeamCityCaller teamCityCaller,
            IAgentsRetriever agentsRetriever,
            IBuildModelToBuildConverter buildModelToBuildConverter
            )
        {
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
            m_TeamCityCaller = teamCityCaller;
            m_AgentsRetriever = agentsRetriever;
            m_BuildModelToBuildConverter = buildModelToBuildConverter;
        }

        public IBuild Run(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent = null, Action<IBuildParameterValueBuilder> parameters = null,
            Action<IMoreOptionsHavingBuilder> moreOptionsAction = null)
        {
            var agentId = GetAgentId(onAgent);
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var moreOptions = GetMoreOptions(moreOptionsAction);
            var body = CreateTriggerBody(buildConfiguration.Id, agentId, GetProperties(parameters), moreOptions);
            var buildModel = m_TeamCityCaller.PostFormat<BuildModel>(body, HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue");
            var build = m_BuildModelToBuildConverter.ConvertToBuild(buildModel);
            return build;
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

            bodyBuilder.Append(@"<build");

            if (moreOptions != null)
            {
                if (moreOptions.TriggeringOptions.Personal == true)
                {
                    bodyBuilder.Append(@" personal=""true""");
                }

                if (moreOptions.HasBranch())
                {
                    var encodedName = SecurityElement.Escape(moreOptions.GetBranchName());
                    bodyBuilder.AppendFormat(@" branchName=""{0}""", encodedName);
                }
            }

            bodyBuilder.Append(@">").AppendLine();

            bodyBuilder.AppendFormat(@"<buildType id=""{0}""/>", buildConfigId).AppendLine();

            if (agentId.HasValue)
            {
                bodyBuilder.AppendFormat(@"<agent id=""{0}""/>", agentId).AppendLine();
            }

            if (moreOptions != null &&
                moreOptions.GetComment() != null)
            {
                var encodedName = SecurityElement.Escape(moreOptions.GetComment());
                bodyBuilder.AppendFormat(@"<comment><text>{0}</text></comment>", encodedName).AppendLine();
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