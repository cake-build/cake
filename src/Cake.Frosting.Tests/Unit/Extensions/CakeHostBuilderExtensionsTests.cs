// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Frosting.Tests.Data;
using NSubstitute;
using Xunit;

namespace Cake.Frosting.Tests.Unit.Extensions
{
    public sealed class CakeHostBuilderExtensionsTests
    {
        public sealed class TheUseStartupExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Reference_Is_Null()
            {
                // Given
                ICakeHostBuilder builder = null;

                // When
                var result = Record.Exception(() => builder.UseStartup<DummyStartup>());

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Run_Startup()
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.UseStartup<DummyStartup>();

                // Then
                services.RegisterType(typeof(DummyStartup.DummyStartupSentinel));
            }
        }

        public sealed class TheWithArgumentsMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Reference_Is_Null()
            {
                // Given
                ICakeHostBuilder builder = null;

                // When
                var result = Record.Exception(() => builder.WithArguments(new string[] { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Theory]
            [InlineData("--v=quiet", Verbosity.Quiet)]
            [InlineData("--v=minimal", Verbosity.Minimal)]
            [InlineData("--v=normal", Verbosity.Normal)]
            [InlineData("--v=verbose", Verbosity.Verbose)]
            [InlineData("--v=diagnostic", Verbosity.Diagnostic)]
            [InlineData("--v=q", Verbosity.Quiet)]
            [InlineData("--v=m", Verbosity.Minimal)]
            [InlineData("--v=n", Verbosity.Normal)]
            [InlineData("--v=v", Verbosity.Verbose)]
            [InlineData("--v=d", Verbosity.Diagnostic)]
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
            public void Should_Parse_Verbosity(string args, Verbosity expected)
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.WithArguments(new[] { args });

                // Then
                services.Received(1).RegisterInstance(
                    Arg.Is<CakeHostOptions>(o => o.Verbosity == expected));
            }

            [Theory]
            [InlineData("--help")]
            [InlineData("--h")]
            [InlineData("-help")]
            [InlineData("-h")]
            public void Should_Parse_Show_Help(string args)
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.WithArguments(new[] { args });

                // Then
                services.Received(1).RegisterInstance(
                    Arg.Is<CakeHostOptions>(o => o.Command == CakeHostCommand.Help));
            }

            [Theory]
            [InlineData("--version")]
            [InlineData("-version")]
            public void Should_Parse_Show_Version(string args)
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.WithArguments(new[] { args });

                // Then
                services.Received(1).RegisterInstance(
                    Arg.Is<CakeHostOptions>(o => o.Command == CakeHostCommand.Version));
            }

            [Theory]
            [InlineData("--target=Test1", "Test1")]
            [InlineData("--t=Test2", "Test2")]
            [InlineData("-target=Test1", "Test1")]
            [InlineData("-t=Test2", "Test2")]
            public void Should_Parse_Target(string args, string expected)
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.WithArguments(new[] { args });

                // Then
                services.Received(1).RegisterInstance(
                    Arg.Is<CakeHostOptions>(o => o.Target == expected));
            }

            [Theory]
            [InlineData("--w=Test1", "Test1")]
            [InlineData("--working=Test2", "Test2")]
            [InlineData("-w=Test1", "Test1")]
            [InlineData("-working=Test2", "Test2")]
            public void Should_Parse_Working_Directory(string args, string expected)
            {
                // Given
                var builder = Substitute.For<ICakeHostBuilder>();
                var services = Substitute.For<ICakeServices>();
                builder.ConfigureServices(Arg.Invoke(services));

                // When
                builder.WithArguments(new[] { args });

                // Then
                services.Received(1).RegisterInstance(
                    Arg.Is<CakeHostOptions>(o => o.WorkingDirectory.FullPath == expected));
            }
        }
    }
}
