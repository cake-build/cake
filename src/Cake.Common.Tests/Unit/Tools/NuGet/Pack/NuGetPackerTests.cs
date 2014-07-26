using System;
using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Pack
{
    public sealed class NuGetPackerTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Nuspec_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack(null, new NuGetPackSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("nuspecFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetFixture(toolPath: expected);
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == "/Working/tools/NuGet.exe"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.Process.GetExitCode().Returns(1);
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Delete_Transformed_Nuspec()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings());

                // Then
                Assert.False(fixture.FileSystem.Exist((FilePath)"/Working/existing.temp.nuspec"));
            }

            [Fact]
            public void Should_Throw_If_Nuspec_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() =>
                    packer.Pack("./nonexisting.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find nuspec file '/Working/nonexisting.nuspec'.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Temporary_Nuspec_Already_Exist()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.FileSystem.GetCreatedFile("/Working/existing.temp.nuspec");
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() =>
                    packer.Pack("./existing.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not create the nuspec file '/Working/existing.temp.nuspec' since it already exist.", result.Message);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    Version = "1.0.0"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -Version \"1.0.0\" \"/Working/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_Base_Path_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    BasePath = "./build"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -BasePath \"/Working/build\" \"/Working/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    OutputDirectory = "./build/output"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -OutputDirectory \"/Working/build/output\" \"/Working/build/output/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_No_Package_Analysis_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    NoPackageAnalysis = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack \"/Working/existing.temp.nuspec\" -NoPackageAnalysis"));
            }

            [Fact]
            public void Should_Add_Symbols_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    Symbols = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack \"/Working/existing.temp.nuspec\" -Symbols"));
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    Id = "The ID",
                    Version = "The version",
                    Title = "The title",
                    Authors = new[] { "Author #1", "Author #2" },
                    Owners = new[] { "Owner #1", "Owner #2" },
                    Description = "The description",
                    Summary = "The summary",
                    LicenseUrl = new Uri("https://license.com"),
                    ProjectUrl = new Uri("https://project.com"),
                    IconUrl = new Uri("https://icon.com"),
                    RequireLicenseAcceptance = true,
                    Copyright = "The copyright",
                    ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" },
                    Tags = new[] { "Tag1", "Tag2", "Tag3" }
                });

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata.NormalizeLineEndings(),
                    fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
            }
        }
    }
}
