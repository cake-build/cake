using Cake.Common.Tests.Fixtures.Tools.NuGet.List;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.List
{
    public sealed class NuGetListTests
    {
        public sealed class TheListMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Id_Is_Null()
            {
                // Given
                var fixture = new NuGetListFixture
                {
                    PackageId = null
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/nuget/nuget.exe", "/bin/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetListFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetListFixture
                {
                    PackageId = "Cake"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_AllVersions_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.AllVersions = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -AllVersions -Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_IncludeDelisted_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.IncludeDelisted = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -IncludeDelisted -Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Prerelease_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.Prerelease = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -Prerelease -Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -Source \"A;B;C\" -Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetListFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"Cake\" -ConfigFile \"/Working/nuget.config\" " +
                             "-Verbosity Normal -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_NuGetListItems()
            {
                // Given
                var fixture = new NuGetListFixture
                {
                    PackageId = "Cake"
                };
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.Result,
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake");
                        Assert.Equal(item.Version, "0.22.2");
                    },
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake.Core");
                        Assert.Equal(item.Version, "0.22.2");
                    },
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake.CoreCLR");
                        Assert.Equal(item.Version, "0.22.2");
                    });
            }
        }
    }
}
