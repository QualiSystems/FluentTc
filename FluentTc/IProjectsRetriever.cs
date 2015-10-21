using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IProjectsRetriever
    {
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having);
    }

    internal class ProjectsRetriever : IProjectsRetriever
    {
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public ProjectsRetriever(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory,
            ITeamCityCaller teamCityCaller)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            var projects = m_TeamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", buildProjectHavingBuilder.GetLocator());
            return projects.Project;
        }
    }
}