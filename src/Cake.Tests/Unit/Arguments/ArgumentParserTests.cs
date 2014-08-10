using System;
using Cake.Arguments;
using Cake.Core.Diagnostics;
using Cake.Tests.Fakes;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

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
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = Record.Exception(() => parser.Parse(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("args", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Parse_Empty_Parameters()
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new string[] { });

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Add_Unknown_Arguments_To_Argument_List()
            {
                // Given
                var log = new FakeLog();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", "-unknown" });

                // Then
                Assert.True(result.Arguments.ContainsKey("unknown"));
            }

            [Fact]
            public void Should_Return_Error_If_Multiple_Arguments_With_The_Same_Name_Exist()
            {
                // Given
                var log = new FakeLog();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", "-unknown", "-unknown" });

                // Then
                Assert.Null(result);
                Assert.True(log.Messages.Contains("Multiple arguments with the same name (unknown)."));
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
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", input });

                // Then
                Assert.Equal(value, result.Verbosity);
            }

            [Theory]
            [InlineData("build.csx")]
            [InlineData("build.csx -verbosity=quiet")]
            public void Can_Parse_Script(string input)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);
                var arguments = input.Split(new[] { ' ' }, StringSplitOptions.None);

                // When
                var result = parser.Parse(arguments);

                // Then
                Assert.NotNull(result.Script);
                Assert.Equal("build.csx", result.Script.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Multiple_Build_Configurations_Are_Provided()
            {
                // Given
                var log = new FakeLog();
                var parser = new ArgumentParser(log);
                var arguments = new[] { "build1.config", "build2.config" };

                // When
                parser.Parse(arguments);

                // Then
                Assert.Equal("More than one build script specified.", log.Messages[0]);
            }

            [Theory]
            [InlineData("/home/test/build.csx")]
            [InlineData("\"/home/test/build.csx\"")]
            public void Can_Parse_Script_With_Unix_Path(string input)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);
                var arguments = input.Split(new[] { ' ' }, StringSplitOptions.None);

                // When
                var result = parser.Parse(arguments);

                // Then
                Assert.NotNull(result);
                Assert.NotNull(result.Script);
                Assert.Equal("/home/test/build.csx", result.Script.FullPath);
            }

            [Theory]
            [InlineData("-showdescription")]
            [InlineData("-s")]
            public void Can_Parse_ShowDescription(string input)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", input });

                // Then
                Assert.Equal(true, result.ShowDescription);
            }

            [Theory]
            [InlineData("-help")]
            [InlineData("-?")]
            public void Can_Parse_Help(string input)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", input });

                // Then
                Assert.Equal(true, result.ShowHelp);
            }

            [Theory]
            [InlineData("-version")]
            [InlineData("-ver")]
            public void Can_Parse_Version(string input)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "build.csx", input });

                // Then
                Assert.Equal(true, result.ShowVersion);
            }
        }
    }
}
