using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Engine;
using JsonFx.IO;
using JsonFx.Model;
using JsonFx.Serialization;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class ModelFiltersTest
    {
        private static string GetTimezoneString()
        {
            return "+"
                   + TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString().PadLeft(2, '0')
                   + TimeZoneInfo.Local.BaseUtcOffset.Minutes.ToString().PadLeft(2, '0');
        }

        [Test]
        public void TeamCityDateFilter_TryRead()
        {
            // Arrange
            var stream = A.Fake<IStream<Token<ModelTokenType>>>();
            A.CallTo(() => stream.Peek()).Returns(new Token<ModelTokenType>(ModelTokenType.Primitive, "20160815T233118" + GetTimezoneString()));

            // Act
            DateTime dateTimeValue;
            new TeamCityDateFilter().TryRead(new DataReaderSettings(), stream, out dateTimeValue);

            // Assert
            dateTimeValue.ShouldBeEquivalentTo(
                new DateTime(
                    year: 2016, month: 08, day: 15,
                    hour: 23, minute: 31, second: 18,
                    kind: DateTimeKind.Utc)
                );
        }

        [Test]
        public void TeamCityDateFilter_TryWrite()
        {
            // Arrange
            var datetime = new DateTime(
                year: 2016, month: 08, day: 15,
                hour: 23, minute: 31, second: 18,
                kind: DateTimeKind.Utc);

            // Act
            IEnumerable<Token<ModelTokenType>> tokens;
            new TeamCityDateFilter().TryWrite(new DataWriterSettings(), datetime, out tokens);

            // Assert
            tokens.Single().Value.ShouldBeEquivalentTo("20160815T233118" + GetTimezoneString());
        }
    }
}
