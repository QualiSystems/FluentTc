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

            string xmlData = $@"<vcs-root-entry id=""{vcsRoot.Id}"">
                                    <vcs-root id=""{vcsRoot.Id}"" 
                                        vcsName=""{vcsRoot.vcsName}""
                                        href=""{vcsRoot.Href}""/>                    
                                    <checkout-rules/>
                                </vcs-root-entry>";

            m_TeamCityCaller.PostFormat(xmlData, HttpContentTypes.ApplicationXml, "/app/rest/buildTypes/{0}/vcs-root-entries", buildConfigurationHavingBuilder.GetLocator());
        }
    }
}
