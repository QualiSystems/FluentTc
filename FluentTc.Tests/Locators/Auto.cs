using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;

namespace FluentTc.Tests.Locators
{
    public class Auto
    {
        public static IFixture Fixture()
        {
            return new Fixture().Customize(new AutoFakeItEasyCustomization());
        }
    }
}