using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildsRetriever
    {
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<BuildIncludeBuilder> include);
    }

    internal class BuildsRetriever : IBuildsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly ICountBuilderFactory m_CountBuilderFactory;

        public BuildsRetriever(ITeamCityCaller caller, IBuildHavingBuilderFactory buildHavingBuilderFactory, ICountBuilderFactory countBuilderFactory)
        {
            m_Caller = caller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_CountBuilderFactory = countBuilderFactory;
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<BuildIncludeBuilder> include)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            having(buildHavingBuilder);
            var countBuilder = m_CountBuilderFactory.CreateCountBuilder();
            count(countBuilder);
            var buildIncludeBuilder = new BuildIncludeBuilder();
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
    }
}