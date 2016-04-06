using FluentAssertions;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class UniversalTypeConverterTests
    {
        [Test]
        public void StringToType_123_Int()
        {
            var value = UniversalTypeConverter.StringToType<int>("123");

            value.Should().Be(123);
        }

        [Test]
        public void StringToType_true_True()
        {
            var value = UniversalTypeConverter.StringToType<bool>("true");

            value.Should().BeTrue();
        }
    }
}