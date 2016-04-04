using System;
using EasyHttp.Http;
using FluentTc.Domain;

namespace FluentTc.Engine
{
    internal interface IProjectCreator
    {
        Project CreateProject(Action<INewProjectDetailsBuilder> newProjectDetailsBuilderAction);
    }

    internal class ProjectCreator : IProjectCreator
    {
        private readonly ITeamCityCaller m_TeamCityCaller;

        public ProjectCreator(ITeamCityCaller teamCityCaller)
        {
            m_TeamCityCaller = teamCityCaller;
        }

        public Project CreateProject(Action<INewProjectDetailsBuilder> newProjectDetailsBuilderAction)
        {
            var newProjectDetailsBuilder = new NewProjectDetailsBuilder();
            newProjectDetailsBuilderAction(newProjectDetailsBuilder);
            var dataXml = newProjectDetailsBuilder.GetDataXml();
            return m_TeamCityCaller.PostFormat<Project>(dataXml, HttpContentTypes.ApplicationXml,
                HttpContentTypes.ApplicationJson, "/app/rest/projects/");
        }
    }
}