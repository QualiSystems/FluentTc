namespace FluentTc.Domain
{
    public enum FileChangeStatus
    {
        Changed,
        Added,
        Removed,
        Not_Changed,
        Directory_Changed,
        Directory_Added,
        Directory_Removed
    }

    public interface IChangedFile
    {
        string RelativeFilePath { get; }
        FileChangeStatus ChangeStatus { get; }
        string ChangesetNumber { get; }
    }

    public class ChangedFile : IChangedFile
    {
        private readonly string m_RelativeFilePath;
        private readonly FileChangeStatus m_ChangeStatus;
        private readonly string m_ChangesetNumber;

        public ChangedFile(string relativeFilePath, FileChangeStatus changeStatus, string changesetNumber)
        {
            m_RelativeFilePath = relativeFilePath;
            m_ChangeStatus = changeStatus;
            m_ChangesetNumber = changesetNumber;
        }

        public string RelativeFilePath { get { return m_RelativeFilePath; } }
        public FileChangeStatus ChangeStatus { get { return m_ChangeStatus; } }
        public string ChangesetNumber { get { return m_ChangesetNumber; } }
    }

}