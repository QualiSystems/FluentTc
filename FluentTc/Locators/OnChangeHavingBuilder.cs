namespace FluentTc.Locators
{
    public interface IOnChangeHavingBuilder
    {
        IOnChangeHavingBuilder Id(long changeId);
    }

    internal class OnChangeHavingBuilder : IOnChangeHavingBuilder
    {
        private long m_ChangeId;

        public IOnChangeHavingBuilder Id(long changeId)
        {
            m_ChangeId = changeId;
            return this;
        }

        public long GetChangeId()
        {
            return m_ChangeId;
        }
    }
}
