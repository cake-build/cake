using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Xunit;

namespace Cake.Core.Tests.Unit.IO.Arguments
{
    public sealed class SecretArgumentTests
    {
        private sealed class TestingArgument : ProcessArgument
        {
            public override string Render()
            {
                return "RENDER";
            }

            public override string RenderSafe()
            {
                return "RENDERSAFE";
            }
        }

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
                var argument = new SecretArgument(
                    new TextArgument(text));

                // When
                var result = argument.Render();

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Call_Child_Arguments_Render_Method()
            {
                // Given
                var argument = new SecretArgument(new TestingArgument());

                // When
                var result = argument.Render();

                // Then
                Assert.Equal("RENDER", result);
            }
        }

        public sealed class TheRenderSafeMethod
        {
            [Theory]
            [InlineData("Hello World", "[REDACTED]")]
            [InlineData("", "[REDACTED]")]
            [InlineData(" \t ", "[REDACTED]")]
            [InlineData(null, "[REDACTED]")]
            public void Should_Render_The_Provided_Text_As_Normal(string text, string expected)
            {
                // Given
                var argument = new SecretArgument(
                    new TextArgument(text));

                // When
                var result = argument.RenderSafe();

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Not_Call_Child_Arguments_RenderSafe_Method()
            {
                // Given
                var argument = new SecretArgument(new TestingArgument());

                // When
                var result = argument.RenderSafe();

                // Then
                Assert.Equal("[REDACTED]", result);
            }
        }
    }
}
