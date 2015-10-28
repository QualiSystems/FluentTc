using System;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildQueueRemover
    {
        void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having);
    }

    internal class BuildQueueRemover : IBuildQueueRemover
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IQueueHavingBuilderFactory m_QueueHavingBuilderFactory;

        public BuildQueueRemover(ITeamCityCaller teamCityCaller, IQueueHavingBuilderFactory queueHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_QueueHavingBuilderFactory = queueHavingBuilderFactory;
        }

        public void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_QueueHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            var locator = ((ILocator) buildProjectHavingBuilder).GetLocator();
            m_TeamCityCaller.DeleteFormat(@"/app/rest/buildQueue/?locator={0}", locator);
        }
    }
}