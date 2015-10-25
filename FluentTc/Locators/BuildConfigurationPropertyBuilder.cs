namespace FluentTc.Locators
{
    public interface IBuildConfigurationPropertyBuilder
    {
    }

    public class BuildConfigurationPropertyBuilder : IBuildConfigurationPropertyBuilder
    {
        public BuildConfigurationPropertyBuilder IncludeDefaults()
        {
            return this;
        }
    }
}