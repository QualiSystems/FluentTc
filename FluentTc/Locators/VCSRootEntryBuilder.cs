using FluentTc.Domain;

namespace FluentTc.Locators
{
    public interface IVCSRootEntryBuilder
    {
        IVCSRootEntryBuilder Id(string value);

        IVCSRootEntryBuilder CheckoutRules(string value);
    }

    internal class VCSRootEntryBuilder: IVCSRootEntryBuilder
    {
        private VcsRootEntry m_VCSRootEntry = new VcsRootEntry();

        public VCSRootEntryBuilder()
        {
            m_VCSRootEntry.VcsRoot = new VcsRoot();
        }

        public IVCSRootEntryBuilder Id(string value)
        {
            m_VCSRootEntry.VcsRoot.Id = value;
            return this;
        }

        public IVCSRootEntryBuilder CheckoutRules(string value)
        {
            m_VCSRootEntry.CheckoutRules = value;
            return this;
        }

        internal VcsRootEntry GetVCSRootEntry()
        {
            return m_VCSRootEntry;
        }
    }
}
