using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessArgumentBuilderTests
    {
        public sealed class TheClearMethod
        {
            [Fact]
            public void Should_Remove_All_Arguments()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("Hello World"));

                // When
                builder.Clear();

                // Then
                Assert.Empty(builder.Render());
            }
        }

        public sealed class ImplicitConversion
        {
            public sealed class FromString
            {
                [Theory]
                [InlineData("Hello World", "Hello World")]
                [InlineData("", "")]
                [InlineData(" \t ", " \t ")]
                [InlineData(null, "")]
                public void Should_Return_Builder_With_Correct_Content(string value, string expected)
                {
                    // Given, When
                    var builder = (ProcessArgumentBuilder)value;

                    // Then
                    Assert.Equal(expected, builder.Render());
                }
            }
        }
    }
}