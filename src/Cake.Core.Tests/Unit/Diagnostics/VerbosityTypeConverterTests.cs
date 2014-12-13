using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics
{
    public sealed class VerboseTypeConverterTests
    {
        [Theory]
        [InlineData("q", Verbosity.Quiet)]
        [InlineData("quiet", Verbosity.Quiet)]
        [InlineData("m", Verbosity.Minimal)]
        [InlineData("minimal", Verbosity.Minimal)]
        [InlineData("n", Verbosity.Normal)]
        [InlineData("normal", Verbosity.Normal)]
        [InlineData("v", Verbosity.Verbose)]
        [InlineData("verbose", Verbosity.Verbose)]
        [InlineData("d", Verbosity.Diagnostic)]
        [InlineData("diagnostic", Verbosity.Diagnostic)]
        [InlineData("Q", Verbosity.Quiet)]
        [InlineData("Quiet", Verbosity.Quiet)]
        [InlineData("M", Verbosity.Minimal)]
        [InlineData("Minimal", Verbosity.Minimal)]
        [InlineData("N", Verbosity.Normal)]
        [InlineData("Normal", Verbosity.Normal)]
        [InlineData("V", Verbosity.Verbose)]
        [InlineData("Verbose", Verbosity.Verbose)]
        [InlineData("D", Verbosity.Diagnostic)]
        [InlineData("Diagnostic", Verbosity.Diagnostic)]
        public void Can_Convert_From_String(string input, Verbosity expected)
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof(Verbosity));

            // When
            var result = converter.ConvertFromString(input);

            // Then
            Assert.NotNull(result);
            Assert.IsType<Verbosity>(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Indicate_That_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof(Verbosity));

            // When
            var result = converter.CanConvertFrom(typeof(string));

            // Then
            Assert.True(result);
        }

        [Fact]
        public void Should_Not_Indicate_That_Non_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof(Verbosity));

            // When
            var result = converter.CanConvertFrom(typeof(int));

            // Then
            Assert.False(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof(Verbosity));

            // When
            var result = Record.Exception(() => converter.ConvertFrom(new List<string>()));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_Known_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof(Verbosity));

            // When
            var result = Record.Exception(() => converter.ConvertFromString("Hello"));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }
    }
}
