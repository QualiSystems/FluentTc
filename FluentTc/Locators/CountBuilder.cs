using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface ICountBuilder
    {
        CountBuilder All();
        CountBuilder Count(int count);
        CountBuilder Start(int start);
        CountBuilder LookupLimit(int lookupLimit);
        string GetCount();
    }

    public class CountBuilder : ICountBuilder
    {
        private const int AllRecords = -1;
        readonly List<string> m_Counts = new List<string>(); 

        public CountBuilder All()
        {
            Count(AllRecords);
            return this;
        }

        public CountBuilder Count(int count)
        {
            m_Counts.Add("count:" + count);
            return this;
        }

        public CountBuilder Start(int start)
        {
            m_Counts.Add("start:" + start);
            return this;
        }

        public CountBuilder LookupLimit(int lookupLimit)
        {
            m_Counts.Add("lookupLimit:" + lookupLimit);
            return this;
        }

        public string GetCount()
        {
            return string.Join(",", m_Counts);
        }
    }
}