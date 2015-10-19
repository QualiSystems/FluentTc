using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildsRetriever
    {
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include);
        List<Build> GetBuildQueues(Action<IBuildProjectHavingBuilder> having);
    }

    internal class BuildsRetriever : IBuildsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly ICountBuilderFactory m_CountBuilderFactory;
        private readonly IBuildIncludeBuilderFactory m_BuildIncludeBuilderFactory;
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildsRetriever(ITeamCityCaller caller, IBuildHavingBuilderFactory buildHavingBuilderFactory, ICountBuilderFactory countBuilderFactory, IBuildIncludeBuilderFactory buildIncludeBuilderFactory, IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_CountBuilderFactory = countBuilderFactory;
            m_BuildIncludeBuilderFactory = buildIncludeBuilderFactory;
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
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

        public List<Build> GetBuildQueues(Action<IBuildProjectHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);

            var locator = buildProjectHavingBuilder.GetLocator();
            var buildWrapper = m_Caller.GetFormat<BuildWrapper>("/app/rest/buildQueue?locator=project:{0}",
                locator);
            if (int.Parse(buildWrapper.Count) > 0)
            {
                return buildWrapper.Build;
            }
            return new List<Build>();
        }
    }
}