using System;
using Cake.Core.Diagnostics.Formatting;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics.Formatting;

public sealed class PropertyTokenTests
{
    public sealed class TheRenderMethod
    {
        [Fact]
        public void Should_Throw_FormatException_When_Index_And_Args_Are_Mismatched()
        {
            // Given
            var token = new PropertyToken(1, null);

            // When
            var ex = Record.Exception(() => token.Render(["test"]));

            // Then
            Assert.IsType<FormatException>(ex);
            Assert.Equal("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.", ex.Message);
        }

        [Fact]
        public void Should_Format_Argument_According_To_Formatting_Rules()
        {
            // Given
            var token = new PropertyToken(0, "B");

            // When
            var result = token.Render([new Guid("d6ed7358ef9645bf9245864025de28fa")]);

            // Then
            Assert.Equal("{d6ed7358-ef96-45bf-9245-864025de28fa}", result);
        }

        [Fact]
        public void Should_Format_Argument_As_String_When_No_Formatting_Rules_Specified()
        {
            // Given
            var token = new PropertyToken(0, null);

            // When
            var result = token.Render([new Guid("{d6ed7358-ef96-45bf-9245-864025de28fa}")]);

            // Then
            Assert.Equal("d6ed7358-ef96-45bf-9245-864025de28fa", result);
        }

        [Theory]
        [InlineData(5, null, "   42")]
        [InlineData(-5, null, "42   ")]
        [InlineData(1, null, "42")]
        [InlineData(8, "N2", "   42.00")]
        [InlineData(-8, "N2", "42.00   ")]
        public void Should_Align_Argument_According_To_Alignment(int alignment, string format, string expected)
        {
            // Given
            var token = new PropertyToken(0, format, alignment);

            // When
            var result = token.Render([42]);

            // Then
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Align_Null_Argument()
        {
            // Given
            var token = new PropertyToken(0, null, 8);

            // When
            var result = token.Render([null]);

            // Then
            Assert.Equal("  [NULL]", result);
        }
    }
}
