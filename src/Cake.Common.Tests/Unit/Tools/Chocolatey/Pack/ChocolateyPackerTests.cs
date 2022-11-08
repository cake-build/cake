// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey;
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --debug --confirm")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --verbose --confirm")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --force")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --trace --confirm")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --no-color --confirm")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --accept-license --confirm")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --what-if")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --limit-output")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(5, "pack \"/Working/existing.temp.nuspec\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(@"c:\temp", "pack \"/Working/existing.temp.nuspec\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "pack \"/Working/existing.temp.nuspec\" --confirm")]
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
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --allow-unofficial")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
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

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --fail-on-error-output")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --use-system-powershell")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --no-progress")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "pack \"/Working/existing.temp.nuspec\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "pack \"/Working/existing.temp.nuspec\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "pack \"/Working/existing.temp.nuspec\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "pack \"/Working/existing.temp.nuspec\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "pack \"/Working/existing.temp.nuspec\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pack \"/Working/existing.temp.nuspec\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.SkipCompatibilityChecks = skipCompatibiity;

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
                Assert.Equal("pack \"/Working/existing.temp.nuspec\" --confirm --version=\"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData("./output", "pack \"/Working/existing.temp.nuspec\" --confirm --output-directory=\"/Working/output\"")]
            [InlineData(null, "pack \"/Working/existing.temp.nuspec\" --confirm")]
            public void Should_Add_OutputDirectory_Flag_To_Arguments_If_Set(string outputDirectory, string expected)
            {
                // Given
                var fixture = new ChocolateyPackerWithNuSpecFixture();
                fixture.Settings.OutputDirectory = outputDirectory;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
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
                fixture.Settings.Dependencies = new[]
                {
                    new ChocolateyNuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new ChocolateyNuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

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
                fixture.Settings.Dependencies = new[]
                {
                    new ChocolateyNuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new ChocolateyNuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

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
                fixture.Settings.Dependencies = new[]
                {
                    new ChocolateyNuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new ChocolateyNuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

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
            fixture.Settings.Dependencies = new[]
            {
                new ChocolateyNuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                new ChocolateyNuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
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
            fixture.Settings.Dependencies = new[]
            {
                new ChocolateyNuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                new ChocolateyNuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
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
                Assert.Equal("pack \"/Working/nonexisting.temp.nuspec\" --confirm --version=\"1.0.0\"", result.Args);
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