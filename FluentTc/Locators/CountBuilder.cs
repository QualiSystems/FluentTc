namespace FluentTc.Locators
{
    public class CountBuilder
    {
        private const int AllRecords = -1;
        private int m_Count = AllRecords;

        public CountBuilder All()
        {
            m_Count = AllRecords;
            return this;
        }

        public CountBuilder Count(int count)
        {
            m_Count = count;
            return this;
        }

        public CountBuilder Start(int start)
        {
            m_Count = start;
            return this;
        }

        public CountBuilder LookupLimit(int lookupLimit)
        {
            m_Count = lookupLimit;
            return this;
        }

        public int GetCount()
        {
            return m_Count;
        }
    }
}