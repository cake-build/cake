// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Pack
{
    public sealed class ChocolateyPackerTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Nuspec_File_Path_Is_Null()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.NuSpecFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "nuspecFilePath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/chocolatey/choco.exe", "/bin/chocolatey/choco.exe")]
            [InlineData("./chocolatey/choco.exe", "/Working/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Delete_Transformed_Nuspec()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();

                // When
                fixture.Run();

                // Then
                Assert.False(fixture.FileSystem.Exist((FilePath)"/Working/existing.temp.nuspec"));
            }

            [Fact]
            public void Should_Throw_If_Nuspec_Do_Not_Exist()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.NuSpecFilePath = "./nonexisting.nuspec";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Could not find nuspec file '/Working/nonexisting.nuspec'.");
            }

            [Fact]
            public void Should_Throw_If_Temporary_Nuspec_Already_Exist()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.GivenTemporaryNuSpecAlreadyExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Could not create the nuspec file '/Working/existing.temp.nuspec' since it already exist.");
            }

            [Theory]
            [InlineData(true, "pack -d -y \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack -v -y \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack -y -f \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack -y --noop \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack -y -r \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "pack -y --execution-timeout \"5\" \"/Working/existing.temp.nuspec\"")]
            [InlineData(0, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "pack -y -c \"c:\\temp\" \"/Working/existing.temp.nuspec\"")]
            [InlineData("", "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack -y --allowunofficial \"/Working/existing.temp.nuspec\"")]
            [InlineData(false, "pack -y \"/Working/existing.temp.nuspec\"")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack -y --version \"1.0.0\" " +
                             "\"/Working/existing.temp.nuspec\"", result.Args);
            }

            [Fact]
            public void Should_Add_Metadata_Element_To_Nuspec_If_Missing()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataElement);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_Without_Namespaces()
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }
        }

        [Fact]
        public void Should_Replace_Template_Tokens_In_Nuspec_With_Files()
        {
            // Given
            var fixture = new ChocolateyPackerWithNuSpecFixture();

            fixture.Settings.Id = "The ID";
            fixture.Settings.Title = "The title";
            fixture.Settings.Version = "The version";
            fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
            fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
            fixture.Settings.Summary = "The summary";
            fixture.Settings.Description = "The description";
            fixture.Settings.ProjectUrl = new Uri("https://project.com");
            fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
            fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
            fixture.Settings.DocsUrl = new Uri("https://docs.com");
            fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
            fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
            fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
            fixture.Settings.Copyright = "The copyright";
            fixture.Settings.LicenseUrl = new Uri("https://license.com");
            fixture.Settings.RequireLicenseAcceptance = true;
            fixture.Settings.IconUrl = new Uri("https://icon.com");
            fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
            fixture.Settings.Files = new[]
            {
                new ChocolateyNuSpecContent { Source = @"tools\**", Target = "tools" },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                result.NuspecContent.NormalizeLineEndings());
        }

        [Fact]
        public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_Without_Namespaces()
        {
            // Given
            var fixture = new ChocolateyPackerWithNuSpecFixture();
            fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues_WithoutNamespaces);

            fixture.Settings.Id = "The ID";
            fixture.Settings.Title = "The title";
            fixture.Settings.Version = "The version";
            fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
            fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
            fixture.Settings.Summary = "The summary";
            fixture.Settings.Description = "The description";
            fixture.Settings.ProjectUrl = new Uri("https://project.com");
            fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
            fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
            fixture.Settings.DocsUrl = new Uri("https://docs.com");
            fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
            fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
            fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
            fixture.Settings.Copyright = "The copyright";
            fixture.Settings.LicenseUrl = new Uri("https://license.com");
            fixture.Settings.RequireLicenseAcceptance = true;
            fixture.Settings.IconUrl = new Uri("https://icon.com");
            fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
            fixture.Settings.Files = new[]
            {
                new ChocolateyNuSpecContent { Source = @"tools\**", Target = "tools" },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                Resources.ChocolateyNuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                result.NuspecContent.NormalizeLineEndings());
        }

        public sealed class TheSettingsPackMethod
        {
            [Fact]
            public void Should_Pack_If_Sufficient_Settings_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();
                fixture.Settings.Id = "nonexisting";
                fixture.Settings.Version = "1.0.0";
                fixture.Settings.Description = "The description";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Files = new[] { new ChocolateyNuSpecContent { Source = @"tools\**" } };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack -y --version \"1.0.0\" " +
                             "\"/Working/nonexisting.temp.nuspec\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Id_Setting_Not_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Required setting Id not specified.");
            }

            [Fact]
            public void Should_Throw_If_Version_Setting_Not_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();
                fixture.Settings.Id = "nonexisting";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Required setting Version not specified.");
            }

            [Fact]
            public void Should_Throw_If_Authors_Setting_Not_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();
                fixture.Settings.Id = "nonexisting";
                fixture.Settings.Version = "1.0.0";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Required setting Authors not specified.");
            }

            [Fact]
            public void Should_Throw_If_Description_Setting_Not_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();
                fixture.Settings.Id = "nonexisting";
                fixture.Settings.Version = "1.0.0";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Required setting Description not specified.");
            }

            [Fact]
            public void Should_Throw_If_Files_Setting_Not_Specified()
            {
                // Given
                var fixture = new ChocolateyPackerWithoutNuSpecFixture();
                fixture.Settings.Id = "nonexisting";
                fixture.Settings.Version = "1.0.0";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Description = "The description";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Required setting Files not specified.");
            }
        }
    }
}