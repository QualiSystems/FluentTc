using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;
using EasyHttp.Http;

namespace FluentTc.Engine
{
    internal interface IProjectsRetriever
    {
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having = null);
        Project GetProject(string projectId);

        void SetFields(Action<IBuildProjectHavingBuilder> having, Action<IBuildProjectFieldValueBuilder> fields);

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

        public IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having = null)
        {
            var locator = having == null ? string.Empty : GetLocator(having);
            return m_TeamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", locator).Project;
        }

        private string GetLocator(Action<IBuildProjectHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            return buildProjectHavingBuilder.GetLocator();
        }

        public Project GetProject(string projectId)
        {
            return m_TeamCityCaller.GetFormat<Project>("/app/rest/projects/id:{0}", projectId);
        }

        public void SetFields(Action<IBuildProjectHavingBuilder> having, Action<IBuildProjectFieldValueBuilder> fields)
        {
            var projectConfigurationHavingBuilder =
                m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(projectConfigurationHavingBuilder);

            BuildProjectFieldValueBuilder fieldValueBuilder = new BuildProjectFieldValueBuilder();
            fields(fieldValueBuilder);
            fieldValueBuilder.GetFields()
                .ForEach(
                    f =>
                        m_TeamCityCaller.PutFormat(f.Value, HttpContentTypes.TextPlain,
                            "/app/rest/projects/{0}/{1}", projectConfigurationHavingBuilder.GetLocator(), f.Name));

        }
    }
}