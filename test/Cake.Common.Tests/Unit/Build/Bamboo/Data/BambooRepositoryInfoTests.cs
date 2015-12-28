using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo.Data
{
    public sealed class BambooRepositoryInfoTests
    {

        public sealed class TheScmProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Scm;

                // Then
                Assert.Equal("git", result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("Cake/Develop", result);
            }
        }

        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Branch;

                // Then
                Assert.Equal("develop", result);
            }
        }
    }
}