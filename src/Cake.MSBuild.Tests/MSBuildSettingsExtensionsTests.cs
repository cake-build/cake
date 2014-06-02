using Cake.Core.IO;
using Xunit;

namespace Cake.MSBuild.Tests
{
    public sealed class MSBuildSettingsExtensionsTests
    {
        public sealed class TheWithTargetMethod
        {
            [Fact]
            public void Should_Add_Target_To_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                configuration.WithTarget("Target");

                // Then
                Assert.True(configuration.Targets.Contains("Target"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                var result = configuration.WithTarget("Target");          

                // Then
                Assert.Equal(configuration, result);
            }
        }

        public sealed class TheWithPropertyMethod
        {
            [Fact]
            public void Should_Add_Property_To_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                configuration.WithProperty("PropertyName", "Value");

                // Then
                Assert.True(configuration.Properties.ContainsKey("PropertyName"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                var result = configuration.WithProperty("PropertyName", "Value");

                // Then
                Assert.Equal(configuration, result);
            }
        }

        public sealed class TheSetConfigurationMethod
        {
            [Fact]
            public void Should_Set_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                configuration.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal("TheConfiguration", configuration.Configuration);  
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                var result = configuration.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal(configuration, result);
            }
        }
    }
}
