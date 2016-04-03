using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IChangesRetriever
    {
        List<Change> GetChanges(Action<IChangesHavingBuilder> having, Action<IChangesIncludeBuilder> include);
    }

    internal class ChangesRetriever : IChangesRetriever
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;

        public ChangesRetriever(ITeamCityCaller teamCityCaller, IBuildHavingBuilderFactory buildHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
        }

        public List<Change> GetChanges(Action<IChangesHavingBuilder> having, Action<IChangesIncludeBuilder> include)
        {
            var changesHavingBuilder = new ChangesHavingBuilder(m_BuildHavingBuilderFactory);
            having(changesHavingBuilder);
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            include(changesIncludeBuilder);
            var columns = changesIncludeBuilder.GetColumns();
            var locator = changesHavingBuilder.GetLocator();

            return m_TeamCityCaller.GetFormat<ChangesList>(@"/app/rest/changes?locator={0}&fields=change({1})", locator, columns).Change;
        }
    }
}