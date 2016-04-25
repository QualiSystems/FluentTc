using System;
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

        [Test]
        public void StringToType_BooleanStringEmpty_FormatExceptionThrown()
        {
            Action action = () => UniversalTypeConverter.StringToType<bool>("");

            action.ShouldThrow<FormatException>().WithMessage("String was not recognized as a valid Boolean.");
        }

        [Test]
        public void StringToType_NullableBooleanStringEmpty_HasNoValue()
        {
            var value = UniversalTypeConverter.StringToType<bool?>("");

            value.HasValue.Should().BeFalse();
        }

        [Test]
        public void StringToType_NullableBooleanTrue_True()
        {
            var value = UniversalTypeConverter.StringToType<bool?>("true");

            value.HasValue.Should().BeTrue();
            value.Value.Should().BeTrue();
        }
    }
}