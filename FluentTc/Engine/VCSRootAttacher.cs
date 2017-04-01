using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;
using System;

namespace FluentTc.Engine
{
    internal interface IVCSRootAttacher
    {
        void Attach(Action<IBuildConfigurationHavingBuilder> having, VcsRoot vcsRoot);
    }

    internal class VCSRootAttacher: IVCSRootAttacher
    {
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public VCSRootAttacher(ITeamCityCaller teamCityCaller, IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
        }

        public void Attach(Action<IBuildConfigurationHavingBuilder> having, VcsRoot vcsRoot)
        {
            var buildConfigurationHavingBuilder = m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            string xmlData = string.Format(
                @"<vcs-root-entry id=""{0}"">
                    <vcs-root id=""{0}"" 
                        vcsName=""{1}""
                        href=""{2}""/>                    
                    <checkout-rules/>
                </vcs-root-entry>", vcsRoot.Id, vcsRoot.Id, vcsRoot.vcsName, vcsRoot.Href);

            m_TeamCityCaller.PostFormat(xmlData, HttpContentTypes.ApplicationXml, "/app/rest/buildTypes/{0}/vcs-root-entries", buildConfigurationHavingBuilder.GetLocator());
        }
    }
}
