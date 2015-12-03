using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildIncludeBuilderFactory
    {
        BuildIncludeBuilder CreateBuildIncludeBuilder();
    }

    internal class BuildIncludeBuilderFactory : IBuildIncludeBuilderFactory
    {
        public BuildIncludeBuilder CreateBuildIncludeBuilder()
        {
            return new BuildIncludeBuilder();
        }
    }
}