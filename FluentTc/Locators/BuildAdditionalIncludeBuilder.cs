using System;

namespace FluentTc.Locators
{
    public interface IBuildAdditionalIncludeBuilder
    {
        IBuildAdditionalIncludeBuilder IncludeChanges(Action<IChangesIncludeBuilder> include);
    }

    public class BuildAdditionalIncludeBuilder : IBuildAdditionalIncludeBuilder
    {
        internal bool ShouldIncludeChanges { get; set; }

        internal Action<IChangesIncludeBuilder> ChangesInclude { get; set; }

        public BuildAdditionalIncludeBuilder()
        {
            ChangesInclude = delegate {  };
        }

        public IBuildAdditionalIncludeBuilder IncludeChanges(Action<IChangesIncludeBuilder> include)
        {
            ShouldIncludeChanges = true;
            ChangesInclude = include;

            return this;
        }
    }
}