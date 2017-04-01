using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;
using JsonFx.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentTc.Engine
{
    internal interface IVCSRootCreator
    {
        VcsRoot Create(Project project, Action<IGitVCSRootBuilder> having);
    }

    internal class VCSRootCreator: IVCSRootCreator
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IBuildProjectHavingBuilder m_BuildProjectHavingBuilder;

        public VCSRootCreator(ITeamCityCaller teamCityCaller, IBuildProjectHavingBuilder buildProjectHavingBuilder)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildProjectHavingBuilder = buildProjectHavingBuilder;
        }

        public VcsRoot Create(Project project, Action<IGitVCSRootBuilder> having)
        {
            var gitVCSBuilder = new GitVCSRootBuilder();

            gitVCSBuilder.Project(project);
            having(gitVCSBuilder);

            var vcs = gitVCSBuilder.GetVCSRoot();

            string xmlData = $@"<vcs-root id=""{vcs.Id}"" name=""{vcs.Name}"" vcsName=""{vcs.vcsName}"">
                                    <project id=""{vcs.Project.Id}"" name=""{vcs.Project.Name}"" href=""{vcs.Project.Href}""/>
                                    <properties count =""{vcs.Properties.Property.Count}"">";

            foreach (var property in vcs.Properties.Property)
            {
                xmlData += @"<property name=""";
                xmlData += property.Name + @"""";
                xmlData += @" value=""" + property.Value + @"""/>";
            }
            xmlData += @"</properties>";

            xmlData += @"</vcs-root>";
            return m_TeamCityCaller.PostFormat<VcsRoot>(
                xmlData, 
                HttpContentTypes.ApplicationXml, 
                HttpContentTypes.ApplicationJson,
                "/app/rest/vcs-roots");
        }
    }
}
