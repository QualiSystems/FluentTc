using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildsRetriever
    {
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include);
        List<Build> GetBuildQueues(Action<IQueueHavingBuilder> having);
    }

    internal class BuildsRetriever : IBuildsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly ICountBuilderFactory m_CountBuilderFactory;
        private readonly IBuildIncludeBuilderFactory m_BuildIncludeBuilderFactory;
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly IQueueHavingBuilderFactory m_QueueHavingBuilderFactory;

        public BuildsRetriever(ITeamCityCaller caller,
            IBuildHavingBuilderFactory buildHavingBuilderFactory,
            ICountBuilderFactory countBuilderFactory,
            IBuildIncludeBuilderFactory buildIncludeBuilderFactory,
            IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory,
            IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, IQueueHavingBuilderFactory queueHavingBuilderFactory)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_CountBuilderFactory = countBuilderFactory;
            m_BuildIncludeBuilderFactory = buildIncludeBuilderFactory;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_QueueHavingBuilderFactory = queueHavingBuilderFactory;
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            having(buildHavingBuilder);
            var countBuilder = m_CountBuilderFactory.CreateCountBuilder();
            count(countBuilder);
            var buildIncludeBuilder = m_BuildIncludeBuilderFactory.CreateBuildIncludeBuilder();
            include(buildIncludeBuilder);

            var locator = buildHavingBuilder.GetLocator();
            var parts = countBuilder.GetCount();
            var columns = buildIncludeBuilder.GetColumns();
            var buildWrapper = m_Caller.GetFormat<BuildWrapper>("/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                locator, parts, columns);
            if (int.Parse(buildWrapper.Count) > 0)
            {
                return buildWrapper.Build;
            }
            return new List<Build>();
        }

        public List<Build> GetBuildQueues(Action<IQueueHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_QueueHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);

            var locator = ((ILocator)buildProjectHavingBuilder).GetLocator();
            var buildWrapper = m_Caller.GetFormat<BuildWrapper>("/app/rest/buildQueue?locator={0}",
                locator);
            if (int.Parse(buildWrapper.Count) > 0)
            {
                return buildWrapper.Build;
            }
            return new List<Build>();
        }

        public List<Build> GetBuildQueues(Action<IBuildConfigurationHavingBuilder> having)
        {
            var configurationHavingBuilder = m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(configurationHavingBuilder);

            var locator = configurationHavingBuilder.GetLocator();
            var buildWrapper = m_Caller.GetFormat<BuildWrapper>("/app/rest/buildQueue?locator=buildType:{0}", locator);
            if (int.Parse(buildWrapper.Count) > 0)
            {
                return buildWrapper.Build;
            }
            return new List<Build>();
        }
    }
}