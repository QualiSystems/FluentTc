using System;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IProjectsRetriever
    {
        Project GetProject(Action<IBuildProjectHavingBuilder> having);
    }

    internal class ProjectsRetriever : IProjectsRetriever
    {
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        internal ProjectsRetriever(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory,
            ITeamCityCaller teamCityCaller)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public Project GetProject(Action<IBuildProjectHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            return m_TeamCityCaller.GetFormat<Project>("/app/rest/projects/{0}", buildProjectHavingBuilder.GetLocator());
        }
    }
}