using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildStatisticsRetriever
    {
        BuildStatistics GetStatistics(Action<IBuildHavingBuilder> having);
    }
    internal class BuildStatisticsRetriever : IBuildStatisticsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;

        public BuildStatisticsRetriever(ITeamCityCaller caller, IBuildHavingBuilderFactory buildHavingBuilderFactory)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
        }

        public BuildStatistics GetStatistics(Action<IBuildHavingBuilder> having)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            having(buildHavingBuilder);
            var locator = buildHavingBuilder.GetLocator();
            return m_Caller.GetFormat<BuildStatistics>("/app/rest/builds/{0}/statistics", locator);
        }
    }
}
