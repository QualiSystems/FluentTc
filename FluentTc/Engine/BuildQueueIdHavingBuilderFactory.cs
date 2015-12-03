using FluentTc.Locators;

namespace FluentTc.Engine
{
    public interface IBuildQueueIdHavingBuilderFactory
    {
        BuildQueueIdHavingBuilder CreateBuildQueueIdHavingBuilder();
    }

    internal class BuildQueueIdHavingBuilderFactory : IBuildQueueIdHavingBuilderFactory
    {
        public BuildQueueIdHavingBuilder CreateBuildQueueIdHavingBuilder()
        {
            return new BuildQueueIdHavingBuilder();
        }
    }
}