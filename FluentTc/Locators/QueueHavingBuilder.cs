using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IQueueHavingBuilder
    {
        IQueueHavingBuilder BuildConfiguration(Action<IBuildConfigurationHavingBuilder> havingBuildConfig);
        IQueueHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilder);
    }

    public class QueueHavingBuilder : IQueueHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();
        private readonly ILocatorBuilder m_LocatorBuilder;

        public QueueHavingBuilder(ILocatorBuilder locatorBuilder)
        {
            m_LocatorBuilder = locatorBuilder;
        }

        public IQueueHavingBuilder BuildConfiguration(Action<IBuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var locator = m_LocatorBuilder.GetBuildConfigurationLocator(havingBuildConfig);
            m_Having.Add("buildType:" + locator);
            return this;
        }

        public IQueueHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilder)
        {
            var locator = m_LocatorBuilder.GetProjectLocator(projectHavingBuilder);
            m_Having.Add("project:" + locator);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}