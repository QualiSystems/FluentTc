using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildIncludeBuilderFactory
    {
        IBuildIncludeBuilder CreateBuildIncludeBuilder();
    }

    internal class BuildIncludeBuilderFactory : IBuildIncludeBuilderFactory
    {
        public IBuildIncludeBuilder CreateBuildIncludeBuilder()
        {
            return new BuildIncludeBuilder();
        }
    }
}