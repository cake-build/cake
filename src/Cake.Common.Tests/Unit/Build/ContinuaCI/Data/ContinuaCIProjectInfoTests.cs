using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIProjectInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("the project from hell", result);
            }
        }
    }
}
