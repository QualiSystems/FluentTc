using FluentTc.Locators;

namespace FluentTc
{
    internal interface ICountBuilderFactory
    {
        ICountBuilder CreateCountBuilder();
    }

    internal class CountBuilderFactory : ICountBuilderFactory
    {
        public ICountBuilder CreateCountBuilder()
        {
            return new CountBuilder();
        }
    }
}