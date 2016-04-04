using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildsRetriever
    {
        IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count,
            Action<IBuildIncludeBuilder> include);

        IList<IBuild> GetBuildsQueue(Action<IQueueHavingBuilder> having = null);
        IBuild GetBuild(long buildId);
    }

    internal class BuildsRetriever : IBuildsRetriever
    {
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly IBuildIncludeBuilderFactory m_BuildIncludeBuilderFactory;
        private readonly IBuildModelToBuildConverter m_BuildModelToBuildConverter;
        private readonly ITeamCityCaller m_Caller;
        private readonly ICountBuilderFactory m_CountBuilderFactory;
        private readonly IQueueHavingBuilderFactory m_QueueHavingBuilderFactory;

        public BuildsRetriever(ITeamCityCaller caller,
            IBuildHavingBuilderFactory buildHavingBuilderFactory,
            ICountBuilderFactory countBuilderFactory,
            IBuildIncludeBuilderFactory buildIncludeBuilderFactory, IQueueHavingBuilderFactory queueHavingBuilderFactory,
            IBuildModelToBuildConverter buildModelToBuildConverter)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_CountBuilderFactory = countBuilderFactory;
            m_BuildIncludeBuilderFactory = buildIncludeBuilderFactory;
            m_QueueHavingBuilderFactory = queueHavingBuilderFactory;
            m_BuildModelToBuildConverter = buildModelToBuildConverter;
        }

        public IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count,
            Action<IBuildIncludeBuilder> include)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            having(buildHavingBuilder);
            var countBuilder = m_CountBuilderFactory.CreateCountBuilder();
            count(countBuilder);
            var buildIncludeBuilder = m_BuildIncludeBuilderFactory.CreateBuildIncludeBuilder();
            include(buildIncludeBuilder);

            var buildWrapper = GetBuildWrapper(buildHavingBuilder, countBuilder, buildIncludeBuilder);
            return m_BuildModelToBuildConverter.ConvertToBuilds(buildWrapper);
        }

        public IList<IBuild> GetBuildsQueue(Action<IQueueHavingBuilder> having = null)
        {
            var locator = having == null ? string.Empty : GetLocator(having);

            var buildWrapper = m_Caller.GetFormat<BuildWrapper>("/app/rest/buildQueue{0}",
                locator);
            return m_BuildModelToBuildConverter.ConvertToBuilds(buildWrapper);
        }

        public IBuild GetBuild(long buildId)
        {
            var buildModel = m_Caller.GetFormat<BuildModel>("/app/rest/builds/id:{0}", buildId);
            return m_BuildModelToBuildConverter.ConvertToBuild(buildModel);
        }

        private BuildWrapper GetBuildWrapper(BuildHavingBuilder buildHavingBuilder, CountBuilder countBuilder,
            BuildIncludeBuilder buildIncludeBuilder)
        {
            var locator = buildHavingBuilder.GetLocator();
            var count = countBuilder.GetCount();
            var columns = buildIncludeBuilder.GetColumns();

            if (string.IsNullOrEmpty(count))
            {
                return m_Caller.GetFormat<BuildWrapper>("/app/rest/builds?locator={0},&fields=count,build({1})", locator,
                    columns);
            }

            return m_Caller.GetFormat<BuildWrapper>("/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                locator, count, columns);
        }

        private string GetLocator(Action<IQueueHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_QueueHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            return "?locator=" + buildProjectHavingBuilder.GetLocator();
        }
    }
}