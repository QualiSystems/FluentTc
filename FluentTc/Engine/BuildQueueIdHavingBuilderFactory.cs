using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildQueueIdHavingBuilderFactory
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