// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Diagnostics;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics
{
    public sealed class CakeBuildLogTests
    {
        public sealed class TheWriteMethod
        {
            public sealed class UsingAnsiEscapeCodes
            {
                [Theory]
                [InlineData(Verbosity.Quiet, Verbosity.Minimal)]
                [InlineData(Verbosity.Minimal, Verbosity.Normal)]
                [InlineData(Verbosity.Normal, Verbosity.Verbose)]
                [InlineData(Verbosity.Verbose, Verbosity.Diagnostic)]
                public void Should_Drop_Log_Messages_Written_With_A_Lower_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, logVerbosity);

                    // When
                    log.Write(messageVerbosity, LogLevel.Information, "Hello World");

                    // Then
                    Assert.Empty(console.Messages);
                }

                [Theory]
                [InlineData(Verbosity.Minimal, Verbosity.Quiet)]
                [InlineData(Verbosity.Normal, Verbosity.Minimal)]
                [InlineData(Verbosity.Verbose, Verbosity.Normal)]
                [InlineData(Verbosity.Diagnostic, Verbosity.Verbose)]
                public void Should_Write_Log_Messages_Written_With_A_Higher_Verbosity_Than_Allowed(Verbosity logVerbosity, Verbosity messageVerbosity)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, logVerbosity);

                    // When
                    log.Write(messageVerbosity, LogLevel.Information, "Hello World");

                    // Then
                    Assert.Single(console.Messages);
                }

                [Theory]
                [InlineData(LogLevel.Warning)]
                [InlineData(LogLevel.Information)]
                [InlineData(LogLevel.Verbose)]
                [InlineData(LogLevel.Debug)]
                public void Should_Write_Standard_Log_Messages_Written_With_A_Higher_Log_Level_Than_Error(LogLevel logLevel)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, logLevel, "Hello World");

                    // Then
                    Assert.Single(console.Messages);
                }

                [Theory]
                [InlineData(LogLevel.Fatal)]
                [InlineData(LogLevel.Error)]
                public void Should_Write_Error_Log_Messages_Written_With_A_Lower_Log_Level_Than_Warning(LogLevel logLevel)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, logLevel, "Hello World");

                    // Then
                    Assert.Single(console.ErrorMessages);
                }

                [Fact]
                public void Should_Not_Colorize_A_Log_Message_Containg_A_Single_Token()
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, LogLevel.Information, "{0}", "Hello World");

                    // Then
                    Assert.Single(console.Messages);
                    Assert.Equal("Hello World", console.Messages[0]);
                }

                [Theory]
                [InlineData(LogLevel.Warning, "\u001b[33;1mHello, \u001b[0m\u001b[33;1mWorld\u001b[0m")]
                [InlineData(LogLevel.Information, "\u001b[37;1mHello, \u001b[0m\u001b[44m\u001b[37;1mWorld\u001b[0m")]
                [InlineData(LogLevel.Verbose, "\u001b[37mHello, \u001b[0m\u001b[37;1mWorld\u001b[0m")]
                [InlineData(LogLevel.Debug, "\u001b[30;1mHello, \u001b[0m\u001b[37mWorld\u001b[0m")]
                public void Should_Colorize_Tokens_Correctly(LogLevel level, string expected)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, level, "Hello, {0}", "World");

                    // Then
                    Assert.Single(console.Messages);
                    Assert.Equal(expected, console.Messages[0]);
                }

                [Theory]
                [InlineData(LogLevel.Fatal, "\u001b[45;1m\u001b[37;1mHello, \u001b[0m\u001b[45m\u001b[37;1mWorld\u001b[0m")]
                [InlineData(LogLevel.Error, "\u001b[41m\u001b[37;1mHello, \u001b[0m\u001b[41;1m\u001b[37;1mWorld\u001b[0m")]
                public void Should_Colorize_Error_Tokens_Correctly(LogLevel level, string expected)
                {
                    // Given
                    var console = FakeConsole.CreateAnsiConsole();
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, level, "Hello, {0}", "World");

                    // Then
                    Assert.Single(console.ErrorMessages);
                    Assert.Equal(expected, console.ErrorMessages[0]);
                }
            }

            public sealed class UsingSystemConsole
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
                    Assert.Empty(console.Messages);
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
                    Assert.Single(console.Messages);
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
                    Assert.Single(console.Messages);
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
                    Assert.Single(console.ErrorMessages);
                }

                [Fact]
                public void Should_Not_Colorize_A_Log_Message_Containg_A_Single_Token()
                {
                    // Given
                    var console = new FakeConsole();
                    console.OutputConsoleColor = true;
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, LogLevel.Information, "{0}", "Hello World");

                    // Then
                    Assert.Single(console.Messages);
                    Assert.Equal("#[Black|White]Hello World[/]", console.Messages[0]);
                }

                [Theory]
                [InlineData(LogLevel.Warning, "#[Black|Yellow]Hello, [/]#[Black|Yellow]World[/]")]
                [InlineData(LogLevel.Information, "#[Black|White]Hello, [/]#[DarkBlue|White]World[/]")]
                [InlineData(LogLevel.Verbose, "#[Black|Gray]Hello, [/]#[Black|White]World[/]")]
                [InlineData(LogLevel.Debug, "#[Black|DarkGray]Hello, [/]#[Black|Gray]World[/]")]
                public void Should_Colorize_Tokens_Correctly(LogLevel level, string expected)
                {
                    // Given
                    var console = new FakeConsole();
                    console.OutputConsoleColor = true;
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, level, "Hello, {0}", "World");

                    // Then
                    Assert.Single(console.Messages);
                    Assert.Equal(expected, console.Messages[0]);
                }

                [Theory]
                [InlineData(LogLevel.Fatal, "#[Magenta|White]Hello, [/]#[DarkMagenta|White]World[/]")]
                [InlineData(LogLevel.Error, "#[DarkRed|White]Hello, [/]#[Red|White]World[/]")]
                public void Should_Colorize_Error_Tokens_Correctly(LogLevel level, string expected)
                {
                    // Given
                    var console = new FakeConsole();
                    console.OutputConsoleColor = true;
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Write(Verbosity.Diagnostic, level, "Hello, {0}", "World");

                    // Then
                    Assert.Single(console.ErrorMessages);
                    Assert.Equal(expected, console.ErrorMessages[0]);
                }

                [Theory]
                [InlineData(false, "#[Black|DarkGray]Executing: if ($LASTEXITCODE -gt 0) { throw \"script failed with exit code $LASTEXITCODE\" }[/]")]
                [InlineData(true, "\u001b[30;1mExecuting: if ($LASTEXITCODE -gt 0) { throw \"script failed with exit code $LASTEXITCODE\" }\u001b[0m")]
                public void Should_Output_Escaped_Tokens_Correctly(bool ansi, string expected)
                {
                    // Given
                    var message = "Executing: if ($LASTEXITCODE -gt 0) {{ throw \"script failed with exit code $LASTEXITCODE\" }}";
                    var console = ansi
                        ? FakeConsole.CreateAnsiConsole()
                        : new FakeConsole()
                        {
                            OutputConsoleColor = true
                        };
                    var log = new CakeBuildLog(console, Verbosity.Diagnostic);

                    // When
                    log.Debug(Verbosity.Normal, message);

                    // Then
                    Assert.Single(console.Messages);
                    Assert.Equal(expected, console.Messages[0]);
                }
            }
        }
    }
}