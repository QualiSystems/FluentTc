using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;
using System;
using System.Security;
using System.Text;

namespace FluentTc.Engine
{
    internal interface IVCSRootCreator
    {
        VcsRoot Create(Action<IGitVCSRootBuilder> having);
    }

    internal class VCSRootCreator: IVCSRootCreator
    {
        private readonly ITeamCityCaller m_TeamCityCaller;

        public VCSRootCreator(ITeamCityCaller teamCityCaller)
        {
            m_TeamCityCaller = teamCityCaller;
        }

        public VcsRoot Create(Action<IGitVCSRootBuilder> having)
        {
            var gitVCSBuilder = new GitVCSRootBuilder();

            having(gitVCSBuilder);

            var vcs = gitVCSBuilder.GetVCSRoot();

            StringBuilder xmlData = new StringBuilder();
            xmlData.AppendFormat(
                @"<vcs-root id=""{0}"" name=""{1}"" vcsName=""{2}""> <project id=""{3}""/> <properties count =""{4}"">",
                SecurityElement.Escape(vcs.Id),
                SecurityElement.Escape(vcs.Name),
                SecurityElement.Escape(vcs.vcsName),
                SecurityElement.Escape(vcs.Project.Id),
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
