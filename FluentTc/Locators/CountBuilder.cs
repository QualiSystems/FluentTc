using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface ICountBuilder
    {
        /// <summary>
        /// Returns the default number of results 
        /// </summary>
        /// <returns></returns>
        ICountBuilder DefaultCount();

        /// <summary>
        /// Return specified number of results
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        ICountBuilder Count(int count);

        /// <summary>
        /// Retuns results starting from the position specified (zero-based)
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        ICountBuilder Start(int start);

        /// <summary>
        /// Limits processing to the latest N builds only. If none of the latest N builds match the other specified criteria of the build locator, 404 response is returned.
        /// </summary>
        /// <param name="lookupLimit"></param>
        /// <returns></returns>
        ICountBuilder LookupLimit(int lookupLimit);

        string GetCount();
    }

    public class CountBuilder : ICountBuilder
    {
        private const int AllRecords = -1;
        readonly List<string> m_Counts = new List<string>();

        public ICountBuilder DefaultCount()
        {
            return this;
        }

        public ICountBuilder Count(int count)
        {
            m_Counts.Add("count:" + count);
            return this;
        }

        public ICountBuilder Start(int start)
        {
            m_Counts.Add("start:" + start);
            return this;
        }

        public ICountBuilder LookupLimit(int lookupLimit)
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