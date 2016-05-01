using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseDirectoryInfoTests
    {
        public sealed class TheSourceDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateDirectoryInfo();

                // When
                var result = info.SourceDirectory;

                //Then
                Assert.Equal("/Users/vagrant/git", result);
            }

        }

        public sealed class TheDeployDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateDirectoryInfo();

                // When
                var result = info.DeployDirectory;

                //Then
                Assert.Equal("/Users/vagrant/deploy", result);
            }

        }
    }
}
