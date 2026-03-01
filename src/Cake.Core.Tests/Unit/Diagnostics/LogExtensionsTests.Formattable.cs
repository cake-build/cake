using System;
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics;

public partial class LogExtensionsTests
{
    public class Formattable
    {
        private const string ExpectedMessage = "Hello World";
        private static readonly FormattableString HelloWorld = $"{"Hello"} {"World"}";

        public class TheDebugMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Debug(null, Verbosity.Normal, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Debug(HelloWorld);

                // Then
                Assert.Equal(Verbosity.Diagnostic, log.Verbosity);
                Assert.Equal(LogLevel.Debug, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Can_Write_Debug_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Debug(Verbosity.Quiet, HelloWorld);

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Debug, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }
        }

        public class TheVerboseMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Verbose(null, Verbosity.Normal, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Verbose(HelloWorld);

                // Then
                Assert.Equal(Verbosity.Verbose, log.Verbosity);
                Assert.Equal(LogLevel.Verbose, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Can_Write_Verbose_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Verbose(Verbosity.Quiet, HelloWorld);

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Verbose, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }
        }

        public class TheInformationMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Information(null, Verbosity.Normal, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Information(HelloWorld);

                // Then
                Assert.Equal(Verbosity.Normal, log.Verbosity);
                Assert.Equal(LogLevel.Information, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Can_Write_Informative_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Information(Verbosity.Quiet, HelloWorld);

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Information, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }
        }

        public class TheWarningMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Warning(null, Verbosity.Normal, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Warning(HelloWorld);

                // Then
                Assert.Equal(Verbosity.Minimal, log.Verbosity);
                Assert.Equal(LogLevel.Warning, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Can_Write_Warning_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Warning(Verbosity.Quiet, HelloWorld);

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Warning, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }
        }

        public class TheErrorMethod
        {
            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Default_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Not_Throw_If_Log_Is_Null_When_Logging_With_Custom_Verbosity()
            {
                // Given, When
                var result = Record.Exception(() => LogExtensions.Error(null, Verbosity.Normal, HelloWorld));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Default_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Error(HelloWorld);

                // Then
                Assert.Equal(Verbosity.Quiet, log.Verbosity);
                Assert.Equal(LogLevel.Error, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Can_Write_Error_Message_With_Custom_Verbosity()
            {
                // Given
                var log = new TestLog();

                // When
                log.Error(Verbosity.Diagnostic, HelloWorld);

                // Then
                Assert.Equal(Verbosity.Diagnostic, log.Verbosity);
                Assert.Equal(LogLevel.Error, log.Level);
                Assert.Equal(ExpectedMessage, log.Message);
            }
        }
    }
}
