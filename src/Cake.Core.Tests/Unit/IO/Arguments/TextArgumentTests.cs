using Cake.Core.IO.Arguments;
using Xunit;
using Xunit.Extensions;

namespace Cake.Core.Tests.Unit.IO.Arguments
{
    public sealed class TextArgumentTests
    {
        public sealed class TheRenderMethod
        {
            [Theory]
            [InlineData("Hello World", "Hello World")]
            [InlineData("", "")]
            [InlineData(" \t ", " \t ")]
            [InlineData(null, "")]
            public void Should_Render_The_Provided_Text(string text, string expected)
            {
                // Given
                var argument = new TextArgument(text);

                // When
                var result = argument.Render();

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheRenderSafeMethod
        {
            [Theory]
            [InlineData("Hello World", "Hello World")]
            [InlineData("", "")]
            [InlineData(" \t ", " \t ")]
            [InlineData(null, "")]
            public void Should_Render_The_Provided_Text_As_Normal(string text, string expected)
            {
                // Given
                var argument = new TextArgument(text);

                // When
                var result = argument.RenderSafe();

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
