using System;
using Cake.Common.Tests.Fixtures.Tools.NuGet;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

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
                var fixture = new NuGetPackerFixture();
                fixture.NuSpecFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsArgumentNullException(result, "nuspecFilePath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenCustomToolPathExist(expected);

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetPackerFixture();

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsCakeException(result, "NuGet: Process returned an error.");
            }

            [Fact]
            public void Should_Delete_Transformed_Nuspec()
            {
                // Given
                var fixture = new NuGetPackerFixture();

                // When
                fixture.Pack();

                // Then
                Assert.False(fixture.FileSystem.Exist((FilePath)"/Working/existing.temp.nuspec"));
            }

            [Fact]
            public void Should_Throw_If_Nuspec_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.NuSpecFilePath = "./nonexisting.nuspec";

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsCakeException(result, "Could not find nuspec file '/Working/nonexisting.nuspec'.");
            }

            [Fact]
            public void Should_Throw_If_Temporary_Nuspec_Already_Exist()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.GivenTemporaryNuSpecAlreadyExist();

                // When
                var result = Record.Exception(() => fixture.Pack());

                // Then
                Assert.IsCakeException(result, "Could not create the nuspec file '/Working/existing.temp.nuspec' since it already exist.");
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pack -Version \"1.0.0\" \"/Working/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_Base_Path_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.BasePath = "./build";

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pack -BasePath \"/Working/build\" \"/Working/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.OutputDirectory = "./build/output";

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pack -OutputDirectory \"/Working/build/output\" \"/Working/existing.temp.nuspec\""));
            }

            [Fact]
            public void Should_Add_No_Package_Analysis_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.NoPackageAnalysis = true;

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pack \"/Working/existing.temp.nuspec\" -NoPackageAnalysis"));
            }

            [Fact]
            public void Should_Add_Symbols_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.Settings.Symbols = true;

                // When
                fixture.Pack();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pack \"/Working/existing.temp.nuspec\" -Symbols"));
            }

            [Fact]
            public void Should_Add_Metadata_Element_To_Nuspec_If_Missing()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataElement);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };

                // When
                fixture.Pack();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata.NormalizeLineEndings(),
                    fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec()
            {
                // Given
                var fixture = new NuGetPackerFixture();

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };

                // When
                fixture.Pack();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata.NormalizeLineEndings(),
                    fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_Without_Namespaces()
            {
                // Given
                var fixture = new NuGetPackerFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };

                // When
                fixture.Pack();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                    fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
            }
        }

        [Fact]
        public void Should_Replace_Template_Tokens_In_Nuspec_With_Files()
        {
            // Given
            var fixture = new NuGetPackerFixture();

            fixture.Settings.Id = "The ID";
            fixture.Settings.Version = "The version";
            fixture.Settings.Title = "The title";
            fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
            fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
            fixture.Settings.Description = "The description";
            fixture.Settings.Summary = "The summary";
            fixture.Settings.LicenseUrl = new Uri("https://license.com");
            fixture.Settings.ProjectUrl = new Uri("https://project.com");
            fixture.Settings.IconUrl = new Uri("https://icon.com");
            fixture.Settings.RequireLicenseAcceptance = true;
            fixture.Settings.Copyright = "The copyright";
            fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
            fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
            fixture.Settings.Files = new[]
            {
                new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                new NuSpecContent { Source = "LICENSE" }
            };

            // When
            fixture.Pack();

            // Then
            Assert.Equal(
                Resources.Nuspec_Metadata.NormalizeLineEndings(),
                fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
        }

        [Fact]
        public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_Without_Namespaces()
        {
            // Given
            var fixture = new NuGetPackerFixture();
            fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

            fixture.Settings.Id = "The ID";
            fixture.Settings.Version = "The version";
            fixture.Settings.Title = "The title";
            fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
            fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
            fixture.Settings.Description = "The description";
            fixture.Settings.Summary = "The summary";
            fixture.Settings.LicenseUrl = new Uri("https://license.com");
            fixture.Settings.ProjectUrl = new Uri("https://project.com");
            fixture.Settings.IconUrl = new Uri("https://icon.com");
            fixture.Settings.RequireLicenseAcceptance = true;
            fixture.Settings.Copyright = "The copyright";
            fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
            fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
            fixture.Settings.Files = new[]
            {
                new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                new NuSpecContent { Source = "LICENSE" }
            };

            // When
            fixture.Pack();

            // Then
            Assert.Equal(
                Resources.Nuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                fixture.FileSystem.GetTextContent("/Working/existing.temp.nuspec").NormalizeLineEndings());
        }
    }
}
