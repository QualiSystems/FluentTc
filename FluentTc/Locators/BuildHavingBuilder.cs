using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildHavingBuilder
    {
        BuildHavingBuilder BuildConfiguration(
            Action<BuildConfigurationHavingBuilder> havingBuildConfig);

        BuildHavingBuilder Id(int buildId);
        BuildHavingBuilder Tags(params string [] tags);
        BuildHavingBuilder Status(BuildStatus buildStatus);
        BuildHavingBuilder TriggeredBy(UserHavingBuilder buildStatus);
        BuildHavingBuilder Personal();
        BuildHavingBuilder NotPersonal();
        BuildHavingBuilder Cancelled();
        BuildHavingBuilder NotCancelled();
        BuildHavingBuilder Running();
        BuildHavingBuilder NotRunning();
        BuildHavingBuilder Pinned();
        BuildHavingBuilder NotPinned();
        BuildHavingBuilder Branch(Action<BranchHavingBuilder> branchHavingBuilder);
        BuildHavingBuilder AgentName(string agentName);
        BuildHavingBuilder SinceBuild(Action<IBuildHavingBuilder> buildHavingBuilder);
        BuildHavingBuilder SinceDate(DateTime dateTime);
        BuildHavingBuilder Project(Action<BuildProjectHavingBuilder> projectHavingBuilder);
        string GetLocator();
    }

    public class BuildHavingBuilder : IBuildHavingBuilder
    {
        private const string DateFormat = "yyyyMMddTHHmmsszz00";

        private readonly List<string> m_Having = new List<string>();
        private readonly IBuildConfigurationHavingBuilderFactory m_BuildConfigurationHavingBuilderFactory;
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;

        public BuildHavingBuilder(IBuildConfigurationHavingBuilderFactory buildConfigurationHavingBuilderFactory, IBuildHavingBuilderFactory buildHavingBuilderFactory)
        {
            m_BuildConfigurationHavingBuilderFactory = buildConfigurationHavingBuilderFactory;
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
        }

        public BuildHavingBuilder BuildConfiguration(
            Action<BuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var buildConfigurationHavingBuilder = m_BuildConfigurationHavingBuilderFactory.CreateBuildConfigurationHavingBuilder();
            havingBuildConfig.Invoke(buildConfigurationHavingBuilder);
            m_Having.AddRange(buildConfigurationHavingBuilder.Get());
            return this;
        }

        public BuildHavingBuilder Id(int buildId)
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

        public BuildHavingBuilder Personal()
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

        public BuildHavingBuilder SinceBuild(Action<IBuildHavingBuilder> buildHavingBuilder)
        {
            var havingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
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

        string IBuildHavingBuilder.GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}