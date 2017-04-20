namespace FluentTc.Locators
{
    using System;

    public interface IMoreOptionsHavingBuilder
    {
        IMoreOptionsHavingBuilder WithCleanSources();
        IMoreOptionsHavingBuilder RebuildAllDependencies();
        IMoreOptionsHavingBuilder QueueAtTop();
        IMoreOptionsHavingBuilder AsPersonal();
        IMoreOptionsHavingBuilder WithComment(string comment);
        IMoreOptionsHavingBuilder OnBranch(string branchName);
        IMoreOptionsHavingBuilder OnChange(Action<IOnChangeHavingBuilder> onChangeHavingBuilderAction);
    }

    internal class MoreOptionsHavingBuilder : IMoreOptionsHavingBuilder
    {
        private string m_Comment;
        private string m_BranchName;
        private readonly TriggeringOptions m_TriggeringOptions;
        private readonly OnChangeHavingBuilder m_OnChangeHavingBuilder;

        public MoreOptionsHavingBuilder()
        {
            m_TriggeringOptions = new TriggeringOptions();
            m_OnChangeHavingBuilder = new OnChangeHavingBuilder();
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

        public IMoreOptionsHavingBuilder OnBranch(string branchName)
        {
            m_BranchName = branchName;
            return this;
        }

        public IMoreOptionsHavingBuilder OnChange(Action<IOnChangeHavingBuilder> onChangeHavingBuilderAction)
        {
            onChangeHavingBuilderAction(this.m_OnChangeHavingBuilder);
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

        public bool HasBranch()
        {
            return !string.IsNullOrEmpty(m_BranchName);
        }

        public TriggeringOptions TriggeringOptions
        {
            get { return m_TriggeringOptions; }
        }

        public bool HasChangeId()
        {
            return this.m_OnChangeHavingBuilder.GetChangeId() != 0;
        }

        public long GetChangeId()
        {
            return this.m_OnChangeHavingBuilder.GetChangeId();
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