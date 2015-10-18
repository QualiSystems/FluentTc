using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public class BuildHavingBuilder
    {
        private const string DateFormat = "yyyyMMddTHHmmsszz00";

        private readonly List<string> m_Having = new List<string>();

        public BuildHavingBuilder BuildConfiguration(
            Action<BuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var buildConfigurationHavingBuilder = new BuildConfigurationHavingBuilder();
            havingBuildConfig.Invoke(buildConfigurationHavingBuilder);
            m_Having.AddRange(buildConfigurationHavingBuilder.Get());
            return this;
        }

        public BuildHavingBuilder HavingId(int buildId)
        {
            m_Having.Add("id:" + buildId);
            return this;
        }

        public BuildHavingBuilder Tags(params string [] tags)
        {
            m_Having.Add("tags:" + string.Join(",", tags));
            return this;
        }

        public BuildHavingBuilder Status(BuildStatus buildStatus)
        {
            m_Having.Add("status:" + buildStatus.ToString().ToUpper());
            return this;
        }

        public BuildHavingBuilder TriggeredBy(UserHavingBuilder buildStatus)
        {
            m_Having.Add("user:" + buildStatus.GetLocator());
            return this;
        }

        public BuildHavingBuilder OnlyPersonal()
        {
            m_Having.Add("personal:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder NotPersonal()
        {
            m_Having.Add("personal:" + bool.FalseString);
            return this;
        }

        public BuildHavingBuilder Cancelled()
        {
            m_Having.Add("cancelled:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder NotCancelled()
        {
            m_Having.Add("cancelled:" + bool.FalseString);
            return this;
        }

        public BuildHavingBuilder Running()
        {
            m_Having.Add("running:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder NotRunning()
        {
            m_Having.Add("running:" + bool.FalseString);
            return this;
        }

        public BuildHavingBuilder Pinned()
        {
            m_Having.Add("pinned:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder NotPinned()
        {
            m_Having.Add("pinned:" + bool.FalseString);
            return this;
        }

        public BuildHavingBuilder Branch(Action<BranchHavingBuilder> branchHavingBuilder)
        {
            var havingBuilder = new BranchHavingBuilder();
            branchHavingBuilder(havingBuilder);
            m_Having.Add("branch:" + havingBuilder.GetLocator());
            return this;
        }

        public BuildHavingBuilder AgentName(string agentName)
        {
            m_Having.Add("agentName:" + agentName);
            return this;
        }

        public BuildHavingBuilder SinceBuild(Action<BuildHavingBuilder> buildHavingBuilder)
        {
            var havingBuilder = new BuildHavingBuilder();
            buildHavingBuilder(havingBuilder);
            m_Having.Add("sinceBuild:" + havingBuilder.GetLocator());
            return this;
        }

        public BuildHavingBuilder SinceDate(DateTime dateTime)
        {
            m_Having.Add("sinceDate:" + dateTime.ToString(DateFormat));
            return this;
        }

        public BuildHavingBuilder Project(Action<BuildProjectHavingBuilder> projectHavingBuilder)
        {
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();
            projectHavingBuilder(buildProjectHavingBuilder);
            m_Having.Add("project:" + buildProjectHavingBuilder.GetLocator());
            return this;
        }

        internal string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}