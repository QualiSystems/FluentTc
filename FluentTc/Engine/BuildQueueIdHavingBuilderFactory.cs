using FluentTc.Locators;

namespace FluentTc.Engine
{
    public interface IBuildQueueIdHavingBuilderFactory
    {
        IBuildQueueIdHavingBuilder CreateBuildQueueIdHavingBuilder();
    }

    internal class BuildQueueIdHavingBuilderFactory : IBuildQueueIdHavingBuilderFactory
    {
        public IBuildQueueIdHavingBuilder CreateBuildQueueIdHavingBuilder()
        {
            return new BuildQueueIdHavingBuilder();
        }
    }
}