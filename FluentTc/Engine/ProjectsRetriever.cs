using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Locators;
using EasyHttp.Http;
using FluentTc.Exceptions;

namespace FluentTc.Engine
{
    internal interface IProjectsRetriever
    {
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having = null);
        Project GetProject(string projectId);
        Project GetProject(Action<IBuildProjectHavingBuilder> having = null);
        void SetFields(Action<IBuildProjectHavingBuilder> having, Action<IBuildProjectFieldValueBuilder> fields);
    }

    internal class ProjectsRetriever : IProjectsRetriever
    {
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;
        private const string TeamCityProjectPrefix = "/app/rest/projects";

        public ProjectsRetriever(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory,
            ITeamCityCaller teamCityCaller)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having = null)
        {
            var locator = having == null ? string.Empty : GetLocator(having);
            var projects = m_TeamCityCaller.GetFormat<ProjectWrapper>(GetApiCall("/{0}"), locator).Project;
            return projects ?? new List<Project>();
        }

        public Project GetProject(string projectId)
        {
            return m_TeamCityCaller.GetFormat<Project>(GetApiCall("/id:{0}"), projectId);
        }

        public Project GetProject(Action<IBuildProjectHavingBuilder> having)
        {
            var projects = GetProjects(having);
            if (!projects.Any()) throw new ProjectNotFoundException();
            if (projects.Count > 1) throw new MoreThanOneProjectFoundException();
            return projects.Single();
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
                            GetApiCall("/{0}/{1}"), projectConfigurationHavingBuilder.GetLocator(), f.Name));
        }

        private string GetLocator(Action<IBuildProjectHavingBuilder> having)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            having(buildProjectHavingBuilder);
            return buildProjectHavingBuilder.GetLocator();
        }

        private string GetApiCall(string appendix)
        {
            var result = TeamCityProjectPrefix + appendix;
            return result;
        }
    }
}