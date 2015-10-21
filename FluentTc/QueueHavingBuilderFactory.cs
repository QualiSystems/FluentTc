using FluentTc.Locators;

namespace FluentTc
{
    internal interface IQueueHavingBuilderFactory
    {
        QueueHavingBuilder CreateBuildProjectHavingBuilder();
    }

    internal class QueueHavingBuilderFactory : IQueueHavingBuilderFactory
    {
        private readonly ILocatorBuilder m_LocatorBuilder;

        public QueueHavingBuilderFactory(ILocatorBuilder locatorBuilder)
        {
            m_LocatorBuilder = locatorBuilder;
        }

        public QueueHavingBuilder CreateBuildProjectHavingBuilder()
        {
            return new QueueHavingBuilder(m_LocatorBuilder);
        }
    }
}