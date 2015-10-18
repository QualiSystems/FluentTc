namespace FluentTc.Locators
{
    public interface IBuildProjectHavingBuilderFactory
    {
        BuildProjectHavingBuilder CreateBuildProjectHavingBuilder();
    }

    public class BuildProjectHavingBuilderFactory : IBuildProjectHavingBuilderFactory
    {
        public BuildProjectHavingBuilder CreateBuildProjectHavingBuilder()
        {
            return new BuildProjectHavingBuilder();
        }
    }
}