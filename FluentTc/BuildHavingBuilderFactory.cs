using FluentTc.Locators;

namespace FluentTc
{
    internal interface IBuildHavingBuilderFactory
    {
        BuildHavingBuilder CreateBuildHavingBuilder();
    }

    internal class BuildHavingBuilderFactory : IBuildHavingBuilderFactory
    {
        public BuildHavingBuilder CreateBuildHavingBuilder()
        {
            return new BuildHavingBuilder();
        }
    }
}