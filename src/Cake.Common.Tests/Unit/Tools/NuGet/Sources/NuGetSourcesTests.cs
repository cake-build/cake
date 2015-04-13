using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tests.Fixtures.Tools.NuGet;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Sources;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Sources
{
    public sealed class NuGetSourcesTests
    {
        public sealed class TheAddSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Name = null;

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsArgumentNullException(result, "name");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Name = string.Empty;

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.Equal("name", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source name cannot be empty.{0}Parameter name: name", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Adding_Source_That_Has_Already_Been_Added()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = "existingsource";
                fixture.GivenSourceAlreadyHasBeenAdded();

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'existingsource' already exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.AddSources());

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
                var fixture = new NuGetSourcesFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.AddSources();

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.AddSources());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetSourcesFixture();

                // When
                fixture.AddSources();

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetSourcesFixture();

                // When
                fixture.AddSources();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_UserName_And_Password_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings.UserName = "username";
                fixture.Settings.Password = "password";

                // When
                fixture.AddSources();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -NonInteractive -UserName \"username\" -Password \"password\""));
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings.Verbosity = NuGetVerbosity.Detailed;

                // When
                fixture.AddSources();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -Verbosity detailed -NonInteractive"));
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings.IsSensitiveSource = true;

                // When
                fixture.AddSources();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.RenderSafe() == "sources Add -Name \"name\" -Source \"[REDACTED]\" -NonInteractive"));
            }
        }
        
        public sealed class TheRemoveSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Name = null;

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsArgumentNullException(result, "name");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Name = string.Empty;

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.Equal("name", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source name cannot be empty.{0}Parameter name: name", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Removing_Source_That_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetSourcesFixture();

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'source' does not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

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
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.RemoveSource();

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.RemoveSource());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();

                // When
                fixture.RemoveSource();

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();

                // When
                fixture.RemoveSource();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Remove -Name \"name\" -Source \"source\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.Settings.Verbosity = NuGetVerbosity.Detailed;

                // When
                fixture.RemoveSource();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Remove -Name \"name\" -Source \"source\" -Verbosity detailed -NonInteractive"));
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.GivenExistingSource();
                fixture.Settings.IsSensitiveSource = true;

                // When
                fixture.RemoveSource();
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.RenderSafe() == "sources Remove -Name \"name\" -Source \"[REDACTED]\" -NonInteractive"));
            }
        }

        public sealed class TheHasSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.HasSource());

                // Then
                Assert.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.HasSource());

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetSourcesFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.HasSource());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }
        }
    }
}
