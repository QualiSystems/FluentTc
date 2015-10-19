namespace FluentTc.Locators
{
    public interface IBuildProjectHavingBuilderFactory
    {
        IBuildProjectHavingBuilder CreateBuildProjectHavingBuilder();
    }

    public class BuildProjectHavingBuilderFactory : IBuildProjectHavingBuilderFactory
    {
        public IBuildProjectHavingBuilder CreateBuildProjectHavingBuilder()
        {
            return new BuildProjectHavingBuilder();
        }
    }
}