using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;
using JsonFx.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

            StringBuilder xmlData = new StringBuilder();
            xmlData.AppendFormat(
                @"<vcs-root id=""{0}"" name=""{1}"" vcsName=""{2}""> <project id=""{3}"" name=""{4}"" href=""{5}""/> <properties count =""{6}"">",
                SecurityElement.Escape(vcs.Id),
                SecurityElement.Escape(vcs.Name),
                SecurityElement.Escape(vcs.vcsName),
                SecurityElement.Escape(vcs.Project.Id),
                SecurityElement.Escape(vcs.Project.Name),
                SecurityElement.Escape(vcs.Project.Href), 
                vcs.Properties.Property.Count);

            foreach (var property in vcs.Properties.Property)
            {
                xmlData.Append(@"<property name=""");
                xmlData.AppendFormat(@"{0}""", SecurityElement.Escape(property.Name));
                xmlData.AppendFormat(@" value=""{0}""/>", SecurityElement.Escape(property.Value));
            }
            xmlData.Append(@"</properties>");

            xmlData.Append(@"</vcs-root>");
            m_TeamCityCaller.Post(
                xmlData.ToString(),
                HttpContentTypes.ApplicationXml,
                "/app/rest/vcs-roots",
                HttpContentTypes.ApplicationJson);
            return vcs;
        }
    }
}
