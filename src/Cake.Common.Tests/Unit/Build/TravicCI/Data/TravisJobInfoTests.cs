using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TravicCI.Data
{
    public sealed class TravisJobInfoTests
    {
        public sealed class TheJobIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateJobInfo();

                // When
                var result = info.JobId;
                
                // Then
                Assert.Equal("934", result);
            }
        }

        public sealed class TheJobNumerProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateJobInfo();

                // When
                var result = info.JobNumber;

                // Then
                Assert.Equal("934.2", result);
            }
        }

        public sealed class TheOSNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateJobInfo();

                // When
                var result = info.OSName;

                // Then
                Assert.Equal("osx", result);
            }
        }

        public sealed class TheSecureEnvironmentVariablesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateJobInfo();

                // When
                var result = info.SecureEnvironmentVariables;

                // Then
                Assert.False(result);
            }
        }
    }
}
