using System;
using System.Linq;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Sources;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;


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
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                fixture.Globber.Match("./tools/**/NuGet.exe").Returns(Enumerable.Empty<FilePath>());
                var sources = fixture.CreateSources();

                // When
                var result = Record.Exception(() => sources.AddSource("name", "source", new NuGetSourcesSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result.Message);
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
        }
    }
}
