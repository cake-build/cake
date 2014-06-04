using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeOptionsTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Should_Set_Verbosity_To_Normal()
            {
                // Given, When
                var result = new CakeOptions();

                // Then
                Assert.Equal(Verbosity.Normal, result.Verbosity);
            }
        }
    }
}
