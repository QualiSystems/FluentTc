using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildStatisticsRetriever
    {
        IList<IBuildStatistic> GetBuildStatistics(Action<IBuildHavingBuilder> having);
    }
    internal class BuildStatisticsRetriever : IBuildStatisticsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly IBuildStatisticConverter m_BuildStatisticConverter;

        public BuildStatisticsRetriever(ITeamCityCaller caller, IBuildHavingBuilderFactory buildHavingBuilderFactory, IBuildStatisticConverter buildStatisticConverter)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_BuildStatisticConverter = buildStatisticConverter;
        }

        public IList<IBuildStatistic> GetBuildStatistics(Action<IBuildHavingBuilder> having)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            having(buildHavingBuilder);
            var locator = buildHavingBuilder.GetLocator();
            var buildStatisticsModel = m_Caller.GetFormat<BuildStatisticsModel>("/app/rest/builds/{0}/statistics", locator);

            return m_BuildStatisticConverter.Convert(buildStatisticsModel);
        }
    }
}
