using System;
using System.Collections.Generic;
using FluentTc.Engine;

namespace FluentTc.Locators
{
    internal interface IChangesHavingBuilder
    {
        IChangesHavingBuilder Build(Action<IBuildHavingBuilder> build);
    }

    internal class ChangesHavingBuilder : IChangesHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        private readonly IBuildHavingBuilderFactory m_BuildHavingBuilderFactory;

        public ChangesHavingBuilder(IBuildHavingBuilderFactory buildHavingBuilderFactory)
        {
            m_BuildHavingBuilderFactory = buildHavingBuilderFactory;
        }

        public IChangesHavingBuilder Build(Action<IBuildHavingBuilder> build)
        {
            var buildHavingBuilder = m_BuildHavingBuilderFactory.CreateBuildHavingBuilder();
            build(buildHavingBuilder);
            m_Having.Add("build:" + buildHavingBuilder.GetLocator());
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}