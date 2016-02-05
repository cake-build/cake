using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Run;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Run
{
    public sealed class DotNetCoreRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();
                fixture.Project = "./src/*";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DotNetCore: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DotNetCore: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run", result.Args);
            }

            [Fact]
            public void Should_Add_Path_Arguments()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();
                fixture.Project = "./tools/tool/";
                fixture.Arguments = "--args";
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run --project \"./tools/tool/\" -- \"--args\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Settings()
            {
                // Given
                var fixture = new DotNetCoreRunnerFixture();
                fixture.Settings.Framework = "dnxcore50";
                fixture.Settings.Configuration = "Release";
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run --framework dnxcore50 --configuration Release", result.Args);
            }
        }
    }
}
