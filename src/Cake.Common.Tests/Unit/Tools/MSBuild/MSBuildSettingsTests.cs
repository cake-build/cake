using System;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
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

            [Fact]
            public void Should_Set_Default_Tools_Version_To_VS2013()
            {
                // Given
                var path = new FilePath("./Project.sln");

                // When
                var settings = new MSBuildSettings(path);

                // Then
                Assert.Equal(MSBuildToolVersion.VS2013, settings.ToolVersion);
            }

            [Fact]
            public void Should_Set_Default_Platform_Target_To_MSIL()
            {
                // Given
                var path = new FilePath("./Project.sln");

                // When
                var settings = new MSBuildSettings(path);

                // Then
                Assert.Equal(PlatformTarget.MSIL, settings.PlatformTarget);
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

        public sealed class ThePropertiesProperty
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
