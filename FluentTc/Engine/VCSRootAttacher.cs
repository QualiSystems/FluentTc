using EasyHttp.Http;
using FluentTc.Domain;
using FluentTc.Locators;
using System;
using System.Security;

namespace FluentTc.Engine
{
    internal interface IVCSRootAttacher
    {
        void Attach(Action<IBuildConfigurationHavingBuilder> having, Action<IVCSRootEntryBuilder> vcsRootEntryHaving);
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

        public void Attach(Action<IBuildConfigurationHavingBuilder> having, Action<IVCSRootEntryBuilder> vcsRootEntryHaving)
        {
            var buildConfigurationHavingBuilder = m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            having(buildConfigurationHavingBuilder);

            var vcsRootEntryBuilder = new VCSRootEntryBuilder();
            vcsRootEntryHaving(vcsRootEntryBuilder);
            var vcsRootEntry = vcsRootEntryBuilder.GetVCSRootEntry();

            string xmlData = string.Format(
                @"<vcs-root-entry id=""{0}"">
                    <vcs-root id=""{0}""/>                    
                    <checkout-rules>{1}</checkout-rules>
                </vcs-root-entry>", 
                SecurityElement.Escape(vcsRootEntry.VcsRoot.Id),
                SecurityElement.Escape(vcsRootEntry.CheckoutRules));

            m_TeamCityCaller.PostFormat(
                xmlData, 
                HttpContentTypes.ApplicationXml, 
                "/app/rest/buildTypes/{0}/vcs-root-entries", 
                buildConfigurationHavingBuilder.GetLocator());
        }
    }
}
