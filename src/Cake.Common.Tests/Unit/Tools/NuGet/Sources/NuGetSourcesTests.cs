using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures;
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
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource(null, "source", new NuGetSourcesSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("name", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource(name, "source", new NuGetSourcesSettings()));

                // Then
                Assert.Equal("name", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source name cannot be empty.{0}Parameter name: name", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", null, new NuGetSourcesSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("source", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", source, new NuGetSourcesSettings()));

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", "source", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Adding_Source_That_Has_Already_Been_Added()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();
                fixture.Process.GetStandardOutput().Returns(new[] {"existingsource"});

                // When
                var result = Record.Exception(() => sources.AddSource("name", "existingsource", new NuGetSourcesSettings()));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'existingsource' already exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", "source", new NuGetSourcesSettings()));

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
                var fixture = new NuGetFixture(expected);
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", new NuGetSourcesSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", "source", NuGetSourcesSettings.Default));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.Process.GetExitCode().Returns(2);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", "source", NuGetSourcesSettings.Default));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", NuGetSourcesSettings.Default);

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", NuGetSourcesSettings.Default);
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_UserName_And_Password_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", new NuGetSourcesSettings
                {
                    UserName = "username",
                    Password = "password"
                });
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -NonInteractive -UserName \"username\" -Password \"password\""));
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", new NuGetSourcesSettings
                {
                    Verbosity = NuGetVerbosity.Detailed
                });
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Add -Name \"name\" -Source \"source\" -Verbosity detailed -NonInteractive"));
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                sources.AddSource("name", "source", new NuGetSourcesSettings
                {
                    IsSensitiveSource = true
                });
                
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
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(null, "source", new NuGetSourcesSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("name", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(name, "source", new NuGetSourcesSettings()));

                // Then
                Assert.Equal("name", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source name cannot be empty.{0}Parameter name: name", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource("name", null, new NuGetSourcesSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("source", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource("name", source, new NuGetSourcesSettings()));

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(defaultToolExist: false, sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(source.Key, source.Value, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Removing_Source_That_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetFixture();
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource("name", "nonexistingsource", new NuGetSourcesSettings()));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'nonexistingsource' does not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(defaultToolExist: false, sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(source.Key, source.Value, new NuGetSourcesSettings()));

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
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(expected, sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                sources.RemoveSource(source.Key, source.Value, new NuGetSourcesSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(source.Key, source.Value, NuGetSourcesSettings.Default));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                fixture.Process.GetExitCode().Returns(2);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.RemoveSource(source.Key, source.Value, NuGetSourcesSettings.Default));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                sources.RemoveSource(source.Key, source.Value, NuGetSourcesSettings.Default);

                // Then
                fixture.ProcessRunner.Received(2).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                sources.RemoveSource(source.Key, source.Value, NuGetSourcesSettings.Default);
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Remove -Name \"name\" -Source \"source\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                sources.RemoveSource(source.Key, source.Value, new NuGetSourcesSettings
                {
                    Verbosity = NuGetVerbosity.Detailed
                });
                
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "sources Remove -Name \"name\" -Source \"source\" -Verbosity detailed -NonInteractive"));
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var source = new KeyValuePair<string, string>("name", "source");
                var fixture = new NuGetFixture(sourceExists:source);
                var sources = fixture.CreateSources();

                // When
                sources.RemoveSource(source.Key, source.Value, new NuGetSourcesSettings
                {
                    IsSensitiveSource = true
                });
                
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
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.HasSource(null, new NuGetSourcesSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("source", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.HasSource(source, new NuGetSourcesSettings()));

                // Then
                Assert.Equal("source", ((ArgumentException)result).ParamName);
                Assert.Equal(string.Format("Source cannot be empty.{0}Parameter name: source", Environment.NewLine), result.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.HasSource("source", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }
        }
    }
}
