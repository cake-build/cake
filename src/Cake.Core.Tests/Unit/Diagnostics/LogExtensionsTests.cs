// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics
{
    public class LogExtensionsTests
    {
        private sealed class TestLog : ICakeLog
        {
            public Verbosity Verbosity { get; set; }

            public LogLevel Level { get; private set; }

            public string Message { get; private set; }

            public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
            {
                Verbosity = verbosity;
                Level = level;
                Message = string.Format(format, args);
            }
        }

        public class TheDebugMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Debug("Hello World");

                // Then
                Assert.Equal(Verbosity.Diagnostic, log.Verbosity);
                Assert.Equal(LogLevel.Debug, log.Level);
                Assert.Equal("Hello World", log.Message);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Debug(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Debug, log.Level);
                Assert.Equal("Hello World", log.Message);
            }
        }

        public class TheVerboseMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Verbose("Hello World");

                // Then
                Assert.Equal(Verbosity.Verbose, log.Verbosity);
                Assert.Equal(LogLevel.Verbose, log.Level);
                Assert.Equal("Hello World", log.Message);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Verbose(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Verbose, log.Level);
                Assert.Equal("Hello World", log.Message);
            }
        }

        public class TheInformationMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Information("Hello World");

                // Then
                Assert.Equal(Verbosity.Normal, log.Verbosity);
                Assert.Equal(LogLevel.Information, log.Level);
                Assert.Equal("Hello World", log.Message);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Information(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Information, log.Level);
                Assert.Equal("Hello World", log.Message);
            }
        }

        public class TheWarningMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Warning("Hello World");

                // Then
                Assert.Equal(Verbosity.Minimal, log.Verbosity);
                Assert.Equal(LogLevel.Warning, log.Level);
                Assert.Equal("Hello World", log.Message);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Warning(Verbosity.Quiet, "Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Warning, log.Level);
                Assert.Equal("Hello World", log.Message);
            }
        }

        public class TheErrorMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, Verbosity.Normal, "Hello World"));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Error("Hello World");

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Error, log.Level);
                Assert.Equal("Hello World", log.Message);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Error(Verbosity.Diagnostic, "Hello World");

                // Then
                Assert.Equal(Verbosity.Diagnostic, log.Verbosity);
                Assert.Equal(LogLevel.Error, log.Level);
                Assert.Equal("Hello World", log.Message);
            }
        }
    }
}
