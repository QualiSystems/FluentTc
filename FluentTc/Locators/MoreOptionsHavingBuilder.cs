using System;

namespace FluentTc.Locators
{
    public interface IMoreOptionsHavingBuilder
    {
        IMoreOptionsHavingBuilder WithCleanSources();
        IMoreOptionsHavingBuilder RebuildAllDependencies();
        IMoreOptionsHavingBuilder QueueAtTop();
        IMoreOptionsHavingBuilder AsPersonal();
        IMoreOptionsHavingBuilder WithComment(string comment);
        IMoreOptionsHavingBuilder BranchName(string branchName);
    }

    internal class MoreOptionsHavingBuilder : IMoreOptionsHavingBuilder
    {
        private string m_Comment;
        private string m_BranchName;
        private readonly TriggeringOptions m_TriggeringOptions;

        public MoreOptionsHavingBuilder()
        {
            m_TriggeringOptions = new TriggeringOptions();
        }

        public IMoreOptionsHavingBuilder WithCleanSources()
        {
            // <triggeringOptions cleanSources="true"/>
            m_TriggeringOptions.CleanSources = true;   
            return this;
        }

        public IMoreOptionsHavingBuilder RebuildAllDependencies()
        {
            // <triggeringOptions rebuildAllDependencies="true"/>
            m_TriggeringOptions.RebuildAllDependencies = true;
            return this;
        }

        public IMoreOptionsHavingBuilder QueueAtTop()
        {
            // <triggeringOptions queueAtTop="true"/>
            m_TriggeringOptions.QueueAtTop = true;
            return this;
        }

        public IMoreOptionsHavingBuilder AsPersonal()
        {
            m_TriggeringOptions.Personal = true;
            return this;
        }

        public IMoreOptionsHavingBuilder WithComment(string comment)
        {
            m_Comment = comment;
            return this;
        }

        public IMoreOptionsHavingBuilder BranchName(string branchName)
        {
            m_BranchName = branchName;
            return this;
        }

        public string GetComment()
        {
            return m_Comment;
        }

        public string GetBranchName()
        {
            return m_BranchName;
        }

        public TriggeringOptions TriggeringOptions
        {
            get { return m_TriggeringOptions; }
        }
    }

    public class TriggeringOptions
    {
        public bool? QueueAtTop { get; set; }
        public bool? RebuildAllDependencies { get; set; }
        public bool? CleanSources { get; set; }
        public bool? Personal { get; set; }
    }
}