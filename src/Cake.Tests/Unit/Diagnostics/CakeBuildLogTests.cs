// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using Cake.Testing;
using Xunit;

namespace Cake.Tests.Unit.Diagnostics
{
    public sealed class CakeBuildLogTests
    {
        public sealed class TheWriteMethod
        {
            [Theory]
            [InlineData(Verbosity.Quiet, Verbosity.Minimal)]
            [InlineData(Verbosity.Minimal, Verbosity.Normal)]
            [InlineData(Verbosity.Normal, Verbosity.Verbose)]
            [InlineData(Verbosity.Verbose, Verbosity.Diagnostic)]
            public void Should_Drop_Log_Messages_Written_With_A_Lower_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
            {
                // Given
                var console = new FakeConsole();
                var log = new CakeBuildLog(console, logVerbosity);

                // When
                log.Write(messageVerbosity, LogLevel.Information, "Hello World");

                // Then
                Assert.Equal(0, console.Messages.Count);
            }

            [Theory]
            [InlineData(Verbosity.Minimal, Verbosity.Quiet)]
            [InlineData(Verbosity.Normal, Verbosity.Minimal)]
            [InlineData(Verbosity.Verbose, Verbosity.Normal)]
            [InlineData(Verbosity.Diagnostic, Verbosity.Verbose)]
            public void Should_Write_Log_Messages_Written_With_A_Higher_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
            {
                // Given
                var console = new FakeConsole();
                var log = new CakeBuildLog(console, logVerbosity);

                // When
                log.Write(messageVerbosity, LogLevel.Information, "Hello World");

                // Then
                Assert.Equal(1, console.Messages.Count);
            }

            [Theory]
            [InlineData(LogLevel.Warning)]
            [InlineData(LogLevel.Information)]
            [InlineData(LogLevel.Verbose)]
            [InlineData(LogLevel.Debug)]
            public void Should_Write_Standard_Log_Messages_Written_With_A_Higher_Log_Level_Than_Error(LogLevel logLevel)
            {
                // Given
                var console = new FakeConsole();
                var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                // When
                log.Write(Verbosity.Diagnostic, logLevel, "Hello World");

                // Then
                Assert.Equal(1, console.Messages.Count);
            }

            [Theory]
            [InlineData(LogLevel.Fatal)]
            [InlineData(LogLevel.Error)]
            public void Should_Write_Error_Log_Messages_Written_With_A_Lower_Log_Level_Than_Warning(LogLevel logLevel)
            {
                // Given
                var console = new FakeConsole();
                var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                // When
                log.Write(Verbosity.Diagnostic, logLevel, "Hello World");

                // Then
                Assert.Equal(1, console.ErrorMessages.Count);
            }
        }
    }
}
