using System;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IInvestigationRetriever
    {
        Investigation GetInvestigation(Action<IBuildConfigurationHavingBuilder> having);
        Investigation GetTestInvestigationByTestNameId(string testNameId);
    }
    
    internal class InvestigationRetriever : IInvestigationRetriever
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;

        public InvestigationRetriever(ITeamCityCaller teamCityCaller,
            IBuildConfigurationRetriever buildConfigurationRetriever)
        {
            m_TeamCityCaller = teamCityCaller;
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
        }

        public Investigation GetInvestigation(Action<IBuildConfigurationHavingBuilder> having)
        {
            var buildConfiguration = m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
            var investigations = m_TeamCityCaller.GetFormat<InvestigationWrapper>("/app/rest/investigations?locator=buildType:(id:{0})", buildConfiguration.Id).Investigation;
            if(investigations != null)
                return investigations.First();

            return null;
        }

        public Investigation GetTestInvestigationByTestNameId(string testNameId)
        {
            var investigations = m_TeamCityCaller.GetFormat<InvestigationWrapper>("/app/rest/investigations?locator=test:(id:{0})", testNameId).Investigation;
            if (investigations != null)
                return investigations.First();

            return null;
        }
    }
}