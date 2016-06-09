// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Diagnostics;
using Cake.Common.Tests.Fixtures.Diagnostics;
using Cake.Core;
using Cake.Core.Diagnostics;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Diagnostics
{
    public sealed class LoggingAliasesTests
    {
        public sealed class TheErrorMethod
        {
            [Fact]
            public void Should_Write_Error_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Error(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Quiet, LogLevel.Error, format, args);
            }

            [Fact]
            public void Should_Evaluate_And_Write_Error_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture();

                // When
                fixture.Context.Error(fixture.Log);

                // Then
                fixture.Context.Log.Received(1).Write(Verbosity.Quiet, LogLevel.Error, fixture.Format, fixture.Args);
                Assert.True(fixture.Evaluated);
            }
        }

        public sealed class TheWarningMethod
        {
            [Fact]
            public void Should_Write_Warning_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Warning(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Minimal, LogLevel.Warning, format, args);
            }

            [Fact]
            public void Should_Evaluate_And_Write_Warning_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Minimal);

                // When
                fixture.Context.Warning(fixture.Log);

                // Then
                fixture.Context.Log.Received(1).Write(Verbosity.Minimal, LogLevel.Warning, fixture.Format, fixture.Args);
                Assert.True(fixture.Evaluated);
            }

            [Fact]
            public void Should_Not_Evaluate_And_Write_Warning_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture();

                // When
                fixture.Context.Warning(fixture.Log);

                // Then
                fixture.Context.Log.DidNotReceive().Write(Verbosity.Minimal, LogLevel.Warning, fixture.Format, fixture.Args);
                Assert.False(fixture.Evaluated);
            }
        }

        public sealed class TheInformationMethod
        {
            [Fact]
            public void Should_Write_Informational_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Information(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Normal, LogLevel.Information, format, args);
            }

            [Fact]
            public void Should_Evaluate_And_Write_Information_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Normal);

                // When
                fixture.Context.Information(fixture.Log);

                // Then
                fixture.Context.Log.Received(1).Write(Verbosity.Normal, LogLevel.Information, fixture.Format, fixture.Args);
                Assert.True(fixture.Evaluated);
            }

            [Fact]
            public void Should_Not_Evaluate_And_Write_Information_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Minimal);

                // When
                fixture.Context.Information(fixture.Log);

                // Then
                fixture.Context.Log.DidNotReceive().Write(Verbosity.Normal, LogLevel.Information, fixture.Format, fixture.Args);
                Assert.False(fixture.Evaluated);
            }
        }

        public sealed class TheVerboseMethod
        {
            [Fact]
            public void Should_Write_Verbose_Message_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Verbose(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Verbose, LogLevel.Verbose, format, args);
            }

            [Fact]
            public void Should_Evaluate_And_Write_Verbose_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Verbose);

                // When
                fixture.Context.Verbose(fixture.Log);

                // Then
                fixture.Context.Log.Received(1).Write(Verbosity.Verbose, LogLevel.Verbose, fixture.Format, fixture.Args);
                Assert.True(fixture.Evaluated);
            }

            [Fact]
            public void Should_Not_Evaluate_And_Write_Verbose_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Normal);

                // When
                fixture.Context.Verbose(fixture.Log);

                // Then
                fixture.Context.Log.DidNotReceive().Write(Verbosity.Verbose, LogLevel.Verbose, fixture.Format, fixture.Args);
                Assert.False(fixture.Evaluated);
            }
        }

        public sealed class TheDebugMethod
        {
            [Fact]
            public void Should_Write_Debug_Message_To_Log()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(Substitute.For<ICakeLog>());
                const string format = "Hello {0}";
                var args = new object[] { 1 };

                // When
                context.Debug(format, args);

                // Then
                context.Log.Received(1).Write(Verbosity.Diagnostic, LogLevel.Debug, format, args);
            }

            [Fact]
            public void Should_Evaluate_And_Write_Debug_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Diagnostic);

                // When
                fixture.Context.Debug(fixture.Log);

                // Then
                fixture.Context.Log.Received(1).Write(Verbosity.Diagnostic, LogLevel.Debug, fixture.Format, fixture.Args);
                Assert.True(fixture.Evaluated);
            }

            [Fact]
            public void Should_Not_Evaluate_And_Write_Debug_Message_To_Log()
            {
                // Given
                var fixture = new LogActionFixture(verbosity:Verbosity.Normal);

                // When
                fixture.Context.Debug(fixture.Log);

                // Then
                fixture.Context.Log.DidNotReceive().Write(Verbosity.Diagnostic, LogLevel.Debug, fixture.Format, fixture.Args);
                Assert.False(fixture.Evaluated);
            }
        }
    }
}
