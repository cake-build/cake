using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessArgumentBuilderTests
    {
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
                    var builder = (ProcessArgumentBuilder)value;

                    // Render 
                    Assert.Equal(expected, builder.Render());
                }
            }
        }
    }
}
