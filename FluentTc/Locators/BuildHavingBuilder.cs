using System;
using System.Collections.Generic;
using System.Web;

namespace FluentTc.Locators
{
    public interface IBuildHavingBuilder
    {
        IBuildHavingBuilder BuildConfiguration(Action<IBuildConfigurationHavingBuilder> havingBuildConfig);
        IBuildHavingBuilder Id(int buildId);
        IBuildHavingBuilder Id(long buildId);
        IBuildHavingBuilder Tags(params string[] tags);
        IBuildHavingBuilder Status(BuildStatus buildStatus);
        IBuildHavingBuilder TriggeredBy(Action<IUserHavingBuilder> buildStatus);
        IBuildHavingBuilder Personal();
        IBuildHavingBuilder NotPersonal();
        IBuildHavingBuilder Cancelled();
        IBuildHavingBuilder NotCancelled();
        IBuildHavingBuilder Running();
        IBuildHavingBuilder NotRunning();
        IBuildHavingBuilder Pinned();
        IBuildHavingBuilder NotPinned();
        IBuildHavingBuilder Branch(Action<IBranchHavingBuilder> branchHavingBuilder);
        IBuildHavingBuilder AgentName(string agentName);
        IBuildHavingBuilder SinceBuild(Action<IBuildHavingBuilder> buildHavingBuilder);
        IBuildHavingBuilder SinceDate(DateTime dateTime);
        IBuildHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilder);
    }

    internal class BuildHavingBuilder : IBuildHavingBuilder
    {
        private const string DateFormat = "yyyyMMddTHHmmss+0000";

        private readonly List<string> m_Having = new List<string>();
        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;
        private readonly IUserHavingBuilderFactory m_UserHavingBuilderFactory;
        private readonly IBranchHavingBuilderFactory m_BranchHavingBuilderFactory;
        private readonly ILocatorBuilder m_LocatorBuilder;

        public BuildHavingBuilder(IBuildHavingBuilderFactory buildHavingBuilderFactory, IUserHavingBuilderFactory userHavingBuilderFactory, IBranchHavingBuilderFactory branchHavingBuilderFactory, IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory, ILocatorBuilder locatorBuilder)
        {
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
            m_UserHavingBuilderFactory = userHavingBuilderFactory;
            m_BranchHavingBuilderFactory = branchHavingBuilderFactory;
            m_LocatorBuilder = locatorBuilder;
        }

        public IBuildHavingBuilder BuildConfiguration(Action<IBuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var locator = m_LocatorBuilder.GetBuildConfigurationLocator(havingBuildConfig);
            m_Having.Add("buildType:" + locator);
            return this;
        }

        public IBuildHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilder)
        {
            var locator = m_LocatorBuilder.GetProjectLocator(projectHavingBuilder);
            m_Having.Add("project:" + locator);
            return this;
        }

        public IBuildHavingBuilder Id(int buildId)
        {
            m_Having.Add("id:" + buildId);
            return this;
        }

        public IBuildHavingBuilder Id(long buildId)
        {
            m_Having.Add("id:" + buildId);
            return this;
        }

        public IBuildHavingBuilder Tags(params string[] tags)
        {
            m_Having.Add("tags:" + string.Join(",", tags));
            return this;
        }

        public IBuildHavingBuilder Status(BuildStatus buildStatus)
        {
            m_Having.Add("status:" + buildStatus.ToString().ToUpper());
            return this;
        }

        public IBuildHavingBuilder TriggeredBy(Action<IUserHavingBuilder> buildStatus)
        {
            var userHavingBuilder = m_UserHavingBuilderFactory.CreateUserHavingBuilder();
            buildStatus(userHavingBuilder);
            m_Having.Add("user:" + userHavingBuilder.GetLocator());
            return this;
        }

        public IBuildHavingBuilder Personal()
        {
            m_Having.Add("personal:" + bool.TrueString);
            return this;
        }

        public IBuildHavingBuilder NotPersonal()
        {
            m_Having.Add("personal:" + bool.FalseString);
            return this;
        }

        public IBuildHavingBuilder Cancelled()
        {
            m_Having.Add("canceled:" + bool.TrueString);
            return this;
        }

        public IBuildHavingBuilder NotCancelled()
        {
            m_Having.Add("canceled:" + bool.FalseString);
            return this;
        }

        public IBuildHavingBuilder Running()
        {
            m_Having.Add("running:" + bool.TrueString);
            return this;
        }

        public IBuildHavingBuilder NotRunning()
        {
            m_Having.Add("running:" + bool.FalseString);
            return this;
        }

        public IBuildHavingBuilder Pinned()
        {
            m_Having.Add("pinned:" + bool.TrueString);
            return this;
        }

        public IBuildHavingBuilder NotPinned()
        {
            m_Having.Add("pinned:" + bool.FalseString);
            return this;
        }

        public IBuildHavingBuilder Branch(Action<IBranchHavingBuilder> branchHavingBuilder)
        {
            var havingBuilder = m_BranchHavingBuilderFactory.CreateBranchHavingBuilder();
            branchHavingBuilder(havingBuilder);
            m_Having.Add("branch:" + havingBuilder.GetLocator());
            return this;
        }

        public IBuildHavingBuilder AgentName(string agentName)
        {
            m_Having.Add("agentName:" + agentName);
            return this;
        }

        public IBuildHavingBuilder SinceBuild(Action<IBuildHavingBuilder> buildHavingBuilder)
        {
            var havingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            buildHavingBuilder(havingBuilder);
            m_Having.Add("sinceBuild:" + havingBuilder.GetLocator());
            return this;
        }

        public IBuildHavingBuilder SinceDate(DateTime dateTime)
        {
            m_Having.Add("sinceDate:" + HttpUtility.UrlEncode(dateTime.ToUniversalTime().ToString(DateFormat)));
            return this;
        }

        public virtual string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}