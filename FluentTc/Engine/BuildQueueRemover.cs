using System;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildQueueRemover
    {
        void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having);
        void RemoveBuildFromQueue(Action<IBuildQueueIdHavingBuilder> having);
    }

    internal class BuildQueueRemover : IBuildQueueRemover
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IQueueHavingBuilderFactory m_QueueHavingBuilderFactory;
        private readonly IBuildQueueIdHavingBuilderFactory m_BuildQueueIdHavingBuilderFactory;

        public BuildQueueRemover(ITeamCityCaller teamCityCaller, IQueueHavingBuilderFactory queueHavingBuilderFactory, IBuildQueueIdHavingBuilderFactory buildQueueIdHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_QueueHavingBuilderFactory = queueHavingBuilderFactory;
            m_BuildQueueIdHavingBuilderFactory = buildQueueIdHavingBuilderFactory;
        }

        public void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_QueueHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            var locator = "?locator=" + buildProjectHavingBuilder.GetLocator();
            RemoveFromQueuebyLocator(locator);
        }

        public void RemoveBuildFromQueue(Action<IBuildQueueIdHavingBuilder> having)
        {
            var buildQueueIdHavingBuilder = m_BuildQueueIdHavingBuilderFactory.CreateBuildQueueIdHavingBuilder();
            having(buildQueueIdHavingBuilder);
            RemoveFromQueuebyLocator(buildQueueIdHavingBuilder.GetLocator());
        }

        private void RemoveFromQueuebyLocator(string locator)
        {
            m_TeamCityCaller.DeleteFormat(@"/app/rest/buildQueue/{0}", locator);
        }
    }
}