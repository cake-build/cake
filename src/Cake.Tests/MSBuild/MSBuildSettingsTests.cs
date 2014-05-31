using System;
using Cake.Core.IO;
using Cake.Core.MSBuild;
using Xunit;

namespace Cake.Tests.MSBuild
{
    public sealed class MSBuildSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Solution_Path_Is_Null()
            {
                // Given, When
                var exception = Record.Exception(() => new MSBuildSettings(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("solution", ((ArgumentNullException)exception).ParamName);
            }
        }

        public sealed class TheSolutionProperty
        {
            [Fact]
            public void Should_Return_The_Solution_File_Path_Provided_To_The_Constructor()
            {
                // Given, When
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // Then
                Assert.Equal(solution, configuration.Solution);
            }
        }

        public sealed class TheTargetsProperty
        {
            [Fact]
            public void Should_Return_A_Set_That_Is_Case_Insensitive()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);

                // When
                configuration.Targets.Add("TARGET");

                // Then
                Assert.True(configuration.Targets.Contains("target"));
            }
        }

        public sealed class TheParametersProperty
        {
            [Fact]
            public void Should_Return_A_Dictionary_That_Is_Case_Insensitive()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");
                var configuration = new MSBuildSettings(solution);
                
                // When
                configuration.Properties.Add("THEKEY", "THEVALUE");

                // Then
                Assert.True(configuration.Properties.ContainsKey("thekey"));
            }
        }

        public sealed class TheConfigurationProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var solution = new FilePath("/src/Solution.sln");                

                // When
                var configuration = new MSBuildSettings(solution);

                // Then
                Assert.Equal(string.Empty, configuration.Configuration);
            }
        }
    }
}
