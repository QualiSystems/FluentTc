using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IQueueHavingBuilderFactory
    {
        IQueueHavingBuilder CreateBuildProjectHavingBuilder();
    }

    internal class QueueHavingBuilderFactory : IQueueHavingBuilderFactory
    {
        private readonly ILocatorBuilder m_LocatorBuilder;

        public QueueHavingBuilderFactory(ILocatorBuilder locatorBuilder)
        {
            m_LocatorBuilder = locatorBuilder;
        }

        public IQueueHavingBuilder CreateBuildProjectHavingBuilder()
        {
            return new QueueHavingBuilder(m_LocatorBuilder);
        }
    }
}