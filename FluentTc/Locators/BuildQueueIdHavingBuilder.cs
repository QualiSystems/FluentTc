namespace FluentTc.Locators
{
    public interface IBuildQueueIdHavingBuilder : ILocator
    {
        IBuildQueueIdHavingBuilder Id(int buildId);
    }

    public class BuildQueueIdHavingBuilder : IBuildQueueIdHavingBuilder
    {
        private string m_Locator = string.Empty;

        public IBuildQueueIdHavingBuilder Id(int buildId)
        {
            m_Locator = string.Format("id:{0}", buildId);
            return this;
        }

        public string GetLocator()
        {
            return m_Locator;
        }
    }
}