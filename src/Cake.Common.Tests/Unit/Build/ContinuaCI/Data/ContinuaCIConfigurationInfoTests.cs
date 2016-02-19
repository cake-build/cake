using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIConfigurationInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateConfigurationInfo();

                // When
                var result = info.Name;
                
                // Then
                Assert.Equal("The configuration from the end of the universe", result);
            }
        }
    }
}