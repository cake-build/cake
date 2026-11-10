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

        public class TheOverloadResolution
        {
            public static TheoryData<LogLevel, Verbosity, Action<ICakeLog, string>> InterpolatedStringCalls => new()
            {
                { LogLevel.Error, Verbosity.Quiet, (log, name) => log.Error($"Hello {name}") },
                { LogLevel.Warning, Verbosity.Minimal, (log, name) => log.Warning($"Hello {name}") },
                { LogLevel.Information, Verbosity.Normal, (log, name) => log.Information($"Hello {name}") },
                { LogLevel.Verbose, Verbosity.Verbose, (log, name) => log.Verbose($"Hello {name}") },
                { LogLevel.Debug, Verbosity.Diagnostic, (log, name) => log.Debug($"Hello {name}") },
                { LogLevel.Error, Verbosity.Diagnostic, (log, name) => log.Error(Verbosity.Diagnostic, $"Hello {name}") },
                { LogLevel.Warning, Verbosity.Diagnostic, (log, name) => log.Warning(Verbosity.Diagnostic, $"Hello {name}") },
                { LogLevel.Information, Verbosity.Diagnostic, (log, name) => log.Information(Verbosity.Diagnostic, $"Hello {name}") },
                { LogLevel.Verbose, Verbosity.Diagnostic, (log, name) => log.Verbose(Verbosity.Diagnostic, $"Hello {name}") },
                { LogLevel.Debug, Verbosity.Quiet, (log, name) => log.Debug(Verbosity.Quiet, $"Hello {name}") },
            };

            [Theory]
            [MemberData(nameof(InterpolatedStringCalls))]
            public void Should_Prefer_Formattable_String_Overload_For_Interpolated_String(LogLevel expectedLevel, Verbosity expectedVerbosity, Action<ICakeLog, string> logCall)
            {
                // Given
                var log = new TestLog();

                // When
                logCall(log, "{World}");

                // Then
                Assert.Equal(expectedLevel, log.Level);
                Assert.Equal(expectedVerbosity, log.Verbosity);
                Assert.Equal("Hello {0}", log.Format);
                Assert.Equal(["{World}"], log.Args);
                Assert.Equal("Hello {World}", log.Message);
            }

            [Fact]
            public void Should_Use_String_Overload_For_Composite_Format_String()
            {
                // Given
                var log = new TestLog();

                // When
                log.Information("Hello {0}", "World");

                // Then
                Assert.Equal("Hello {0}", log.Format);
                Assert.Equal(["World"], log.Args);
                Assert.Equal(ExpectedMessage, log.Message);
            }

            [Fact]
            public void Should_Use_String_Overload_For_String_Variable()
            {
                // Given
                var log = new TestLog();
                var message = $"Hello {"{World}"}";

                // When
                log.Information(message);

                // Then
                Assert.Equal("{0}", log.Format);
                Assert.Equal(["Hello {World}"], log.Args);
            }
        }
    }
}
