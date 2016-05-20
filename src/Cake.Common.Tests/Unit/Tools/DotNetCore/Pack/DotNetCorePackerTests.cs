using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Pack;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Pack
{
    public sealed class DotNetCorePackerTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
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
                var fixture = new DotNetCorePackFixture();
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
                var fixture = new DotNetCorePackFixture();
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
                var fixture = new DotNetCorePackFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack", result.Args);
            }

            [Fact]
            public void Should_Add_Project()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack ./src/*", result.Args);
            }

            [Fact]
            public void Should_Add_Settings()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Settings.BuildBasePath = "./temp/";
                fixture.Settings.NoBuild = true;
                fixture.Settings.Configuration = "Release";
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.VersionSuffix = "rc1";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --output \"/Working/artifacts\" --build-base-path \"/Working/temp\" --no-build --configuration Release --version-suffix rc1", result.Args);
            }
        }
    }
}
