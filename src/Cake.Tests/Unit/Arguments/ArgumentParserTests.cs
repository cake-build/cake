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
            public void Should_Log_And_Return_Null_If_Parser_Encounters_Unknown_Switch()
            {
                // Given
                var log = new FakeLog();
                var parser = new ArgumentParser(log);

                // When
                var result = parser.Parse(new[] { "-unknown" });

                // Then
                Assert.Null(result);
                Assert.Equal("Unknown option: unknown", log.Messages[0]);
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
                var result = parser.Parse(new[] { input });

                // Then
                Assert.Equal(value, result.Verbosity);
            }

            [Theory]
            [InlineData("build.csx")]
            [InlineData("-verbosity=quiet build.csx")]
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
        }
    }
}
