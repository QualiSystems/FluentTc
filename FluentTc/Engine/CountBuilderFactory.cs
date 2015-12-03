using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface ICountBuilderFactory
    {
        CountBuilder CreateCountBuilder();
    }

    internal class CountBuilderFactory : ICountBuilderFactory
    {
        public CountBuilder CreateCountBuilder()
        {
            return new CountBuilder();
        }
    }
}