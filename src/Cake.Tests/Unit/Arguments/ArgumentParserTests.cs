// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Arguments;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Arguments
{
    public class ArgumentParserTests
    {
        public class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Arguments_Are_Null()
            {
                // Given
                var fixture = new ArgumentParserFixture();
                var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                // When
                var result = Record.Exception(() => parser.Parse(null));

                // Then
                AssertEx.IsArgumentNullException(result, "args");
            }

            [Fact]
            public void Can_Parse_Empty_Parameters()
            {
                // Given
                var fixture = new ArgumentParserFixture();
                var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                // When
                var result = parser.Parse(new string[] { });

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Throw_If_Multiple_Build_Configurations_Are_Provided()
            {
                // Given
                var fixture = new ArgumentParserFixture { Log = new FakeLog() };
                var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);
                var arguments = new[] { "build1.config", "build2.config" };

                // When
                parser.Parse(arguments);

                // Then
                Assert.Equal("More than one build script specified.", fixture.Log.Entries[0].Message);
            }

            [Theory]
            [InlineData("/home/test/build.cake")]
            [InlineData("\"/home/test/build.cake\"")]
            public void Can_Parse_Script_With_Unix_Path(string input)
            {
                // Given
                var fixture = new ArgumentParserFixture();
                var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);
                var arguments = input.Split(new[] { ' ' }, StringSplitOptions.None);

                // When
                var result = parser.Parse(arguments);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.Script);
                Assert.Equal("/home/test/build.cake", result.Script.FullPath);
            }

            [Theory]
            [InlineData(".cakefile")]
            [InlineData("build.cake")]
            public void Can_Find_Default_Scripts(string scriptName)
            {
                // Given
                var fixture = new ArgumentParserFixture();
                var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);
                var file = Substitute.For<IFile>();
                file.Exists.Returns(true);

                fixture.FileSystem.GetFile(Arg.Is<FilePath>(fp => fp.FullPath == scriptName))
                    .Returns(file);

                // When
                var result = parser.Parse(new string[] { });

                // Then
                Assert.NotNull(result.Script);
            }

            public sealed class WithSingleDashLongArguments
            {
                [Fact]
                public void Should_Add_Unknown_Arguments_To_Argument_List()
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", "-unknown" });

                    // Then
                    Assert.True(result.Arguments.ContainsKey("unknown"));
                }

                [Fact]
                public void Should_Add_Unknown_Arguments_To_Argument_List_Without_Script()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fakeFileSystem = new FakeFileSystem(environment);
                    fakeFileSystem.CreateFile(new FilePath("build.cake"));
                    var fixture = new ArgumentParserFixture { FileSystem = fakeFileSystem };
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "-unknown" });

                    // Then
                    Assert.True(result.Arguments.ContainsKey("unknown"));
                }

                [Fact]
                public void Should_Return_Error_If_Multiple_Arguments_With_The_Same_Name_Exist()
                {
                    // Given
                    var fixture = new ArgumentParserFixture { Log = new FakeLog() };
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", "-unknown", "-unknown" });

                    // Then
                    Assert.NotNull(result);
                    Assert.True(result.HasError);
                    Assert.True(fixture.Log.Entries.Any(x => x.Message == "Multiple arguments with the same name (unknown)."));
                }

                [Theory]
                [InlineData("-verbosity=quiet", Verbosity.Quiet)]
                [InlineData("-verbosity=minimal", Verbosity.Minimal)]
                [InlineData("-verbosity=normal", Verbosity.Normal)]
                [InlineData("-verbosity=verbose", Verbosity.Verbose)]
                [InlineData("-verbosity=diagnostic", Verbosity.Diagnostic)]
                [InlineData("-verbosity=q", Verbosity.Quiet)]
                [InlineData("-verbosity=m", Verbosity.Minimal)]
                [InlineData("-verbosity=n", Verbosity.Normal)]
                [InlineData("-verbosity=v", Verbosity.Verbose)]
                [InlineData("-verbosity=d", Verbosity.Diagnostic)]
                [InlineData("-v=quiet", Verbosity.Quiet)]
                [InlineData("-v=minimal", Verbosity.Minimal)]
                [InlineData("-v=normal", Verbosity.Normal)]
                [InlineData("-v=verbose", Verbosity.Verbose)]
                [InlineData("-v=diagnostic", Verbosity.Diagnostic)]
                [InlineData("-v=q", Verbosity.Quiet)]
                [InlineData("-v=m", Verbosity.Minimal)]
                [InlineData("-v=n", Verbosity.Normal)]
                [InlineData("-v=v", Verbosity.Verbose)]
                [InlineData("-v=d", Verbosity.Diagnostic)]
                public void Can_Parse_Verbosity(string input, Verbosity value)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(value, result.Verbosity);
                }

                [Theory]
                [InlineData("-verbosity=lol", "The value 'lol' is not a valid verbosity.")]
                [InlineData("-verbosity=", "The value '' is not a valid verbosity.")]
                [InlineData("-v=lol", "The value 'lol' is not a valid verbosity.")]
                [InlineData("-v=", "The value '' is not a valid verbosity.")]
                public void Should_Throw_If_Parsing_Invalid_Verbosity(string verbosity, string expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", verbosity });

                    // Then
                    Assert.True(result.HasError);
                    Assert.True(fixture.Log.Entries.Any(e => e.Message == expected));
                }

                [Theory]
                [InlineData("build.cake")]
                [InlineData("build.cake -verbosity=quiet")]
                public void Can_Parse_Script(string input)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);
                    var arguments = input.Split(new[] { ' ' }, StringSplitOptions.None);

                    // When
                    var result = parser.Parse(arguments);

                    // Then
                    Assert.NotNull(result.Script);
                    Assert.Equal("build.cake", result.Script.FullPath);
                }

                [Theory]
                [InlineData("-showdescription", true)]
                [InlineData("-showdescription=true", true)]
                [InlineData("-showdescription=false", false)]
                [InlineData("-s", true)]
                [InlineData("-s=true", true)]
                [InlineData("-s=false", false)]
                public void Can_Parse_ShowDescription(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowDescription);
                }

                [Theory]
                [InlineData("-dryrun", true)]
                [InlineData("-dryrun=true", true)]
                [InlineData("-dryrun=false", false)]
                [InlineData("-noop", true)]
                [InlineData("-noop=true", true)]
                [InlineData("-noop=false", false)]
                [InlineData("-whatif", true)]
                [InlineData("-whatif=true", true)]
                [InlineData("-whatif=false", false)]
                public void Can_Parse_DryRun(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.PerformDryRun);
                }

                [Theory]
                [InlineData("-help", true)]
                [InlineData("-help=true", true)]
                [InlineData("-help=false", false)]
                [InlineData("-?", true)]
                public void Can_Parse_Help(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowHelp);
                }

                [Theory]
                [InlineData("-version", true)]
                [InlineData("-version=true", true)]
                [InlineData("-version=false", false)]
                [InlineData("-ver", true)]
                [InlineData("-ver=true", true)]
                [InlineData("-ver=false", false)]
                public void Can_Parse_Version(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowVersion);
                }

                [Theory]
                [InlineData("-debug", true)]
                [InlineData("-debug=true", true)]
                [InlineData("-debug=false", false)]
                [InlineData("-d", true)]
                [InlineData("-d=true", true)]
                [InlineData("-d=false", false)]
                public void Can_Parse_Debug(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.PerformDebug);
                }

                [Theory]
                [InlineData("-mono", true)]
                [InlineData("-mono=true", true)]
                [InlineData("-mono=false", false)]
                public void Can_Parse_Mono(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.Mono);
                }

                [Theory]
                [InlineData("-experimental", true)]
                [InlineData("-experimental=true", true)]
                [InlineData("-experimental=false", false)]
                public void Can_Parse_Experimental(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.Experimental);
                }
            }

            public sealed class WithTwoDashLongArguments
            {
                [Fact]
                public void Should_Add_Unknown_Arguments_To_Argument_List()
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", "--unknown" });

                    // Then
                    Assert.True(result.Arguments.ContainsKey("unknown"));
                }

                [Fact]
                public void Should_Add_Unknown_Arguments_To_Argument_List_Without_Script()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fakeFileSystem = new FakeFileSystem(environment);
                    fakeFileSystem.CreateFile(new FilePath("build.cake"));
                    var fixture = new ArgumentParserFixture { FileSystem = fakeFileSystem };
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "--unknown" });

                    // Then
                    Assert.True(result.Arguments.ContainsKey("unknown"));
                }

                [Fact]
                public void Should_Return_Error_If_Multiple_Arguments_With_The_Same_Name_Exist()
                {
                    // Given
                    var fixture = new ArgumentParserFixture { Log = new FakeLog() };
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", "--unknown", "--unknown" });

                    // Then
                    Assert.NotNull(result);
                    Assert.True(result.HasError);
                    Assert.True(fixture.Log.Entries.Any(x => x.Message == "Multiple arguments with the same name (unknown)."));
                }

                [Theory]
                [InlineData("--verbosity=quiet", Verbosity.Quiet)]
                [InlineData("--verbosity=minimal", Verbosity.Minimal)]
                [InlineData("--verbosity=normal", Verbosity.Normal)]
                [InlineData("--verbosity=verbose", Verbosity.Verbose)]
                [InlineData("--verbosity=diagnostic", Verbosity.Diagnostic)]
                [InlineData("--verbosity=q", Verbosity.Quiet)]
                [InlineData("--verbosity=m", Verbosity.Minimal)]
                [InlineData("--verbosity=n", Verbosity.Normal)]
                [InlineData("--verbosity=v", Verbosity.Verbose)]
                [InlineData("--verbosity=d", Verbosity.Diagnostic)]
                [InlineData("--v=quiet", Verbosity.Quiet)]
                [InlineData("--v=minimal", Verbosity.Minimal)]
                [InlineData("--v=normal", Verbosity.Normal)]
                [InlineData("--v=verbose", Verbosity.Verbose)]
                [InlineData("--v=diagnostic", Verbosity.Diagnostic)]
                public void Can_Parse_Verbosity(string input, Verbosity value)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(value, result.Verbosity);
                }

                [Theory]
                [InlineData("--verbosity=lol", "The value 'lol' is not a valid verbosity.")]
                [InlineData("--verbosity=", "The value '' is not a valid verbosity.")]
                public void Should_Throw_If_Parsing_Invalid_Verbosity(string verbosity, string expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", verbosity });

                    // Then
                    Assert.True(result.HasError);
                    Assert.True(fixture.Log.Entries.Any(e => e.Message == expected));
                }

                [Theory]
                [InlineData("build.cake")]
                [InlineData("build.cake --verbosity=quiet")]
                public void Can_Parse_Script(string input)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);
                    var arguments = input.Split(new[] { ' ' }, StringSplitOptions.None);

                    // When
                    var result = parser.Parse(arguments);

                    // Then
                    Assert.NotNull(result.Script);
                    Assert.Equal("build.cake", result.Script.FullPath);
                }

                [Theory]
                [InlineData("--showdescription", true)]
                [InlineData("--showdescription=true", true)]
                [InlineData("--showdescription=false", false)]
                public void Can_Parse_ShowDescription(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowDescription);
                }

                [Theory]
                [InlineData("--dryrun", true)]
                [InlineData("--dryrun=true", true)]
                [InlineData("--dryrun=false", false)]
                [InlineData("--noop", true)]
                [InlineData("--noop=true", true)]
                [InlineData("--noop=false", false)]
                [InlineData("--whatif", true)]
                [InlineData("--whatif=true", true)]
                [InlineData("--whatif=false", false)]
                public void Can_Parse_DryRun(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.PerformDryRun);
                }

                [Theory]
                [InlineData("--help", true)]
                [InlineData("--help=true", true)]
                [InlineData("--help=false", false)]
                public void Can_Parse_Help(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowHelp);
                }

                [Theory]
                [InlineData("--version", true)]
                [InlineData("--version=true", true)]
                [InlineData("--version=false", false)]
                [InlineData("--ver", true)]
                [InlineData("--ver=true", true)]
                [InlineData("--ver=false", false)]
                public void Can_Parse_Version(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.ShowVersion);
                }

                [Theory]
                [InlineData("--debug", true)]
                [InlineData("--debug=true", true)]
                [InlineData("--debug=false", false)]
                public void Can_Parse_Debug(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.PerformDebug);
                }

                [Theory]
                [InlineData("--mono", true)]
                [InlineData("--mono=true", true)]
                [InlineData("--mono=false", false)]
                public void Can_Parse_Mono(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.Mono);
                }

                [Theory]
                [InlineData("--experimental", true)]
                [InlineData("--experimental=true", true)]
                [InlineData("--experimental=false", false)]
                public void Can_Parse_Experimental(string input, bool expected)
                {
                    // Given
                    var fixture = new ArgumentParserFixture();
                    var parser = new ArgumentParser(fixture.Log, fixture.VerbosityParser);

                    // When
                    var result = parser.Parse(new[] { "build.cake", input });

                    // Then
                    Assert.Equal(expected, result.Experimental);
                }
            }
        }
    }
}