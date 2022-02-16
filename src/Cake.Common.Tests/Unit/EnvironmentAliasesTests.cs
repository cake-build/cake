// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class EnvironmentAliasesTests
    {
        private const string TestVariableName = "Test";
        private const string TestVariableValue = "Value";

        public sealed class TheHasEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Return_True_If_Variable_Exist()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns(TestVariableValue);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.HasEnvironmentVariable(context, TestVariableName);

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_If_Value_Is_Empty()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns(string.Empty);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.HasEnvironmentVariable(context, TestVariableName);

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Variable_Was_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns((string)null);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = context.HasEnvironmentVariable(TestVariableName);

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Return_Value()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns(TestVariableValue);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.EnvironmentVariable(context, TestVariableName);

                // Then
                Assert.Equal(TestVariableValue, result);
            }

            [Fact]
            public void Should_Return_Null_If_Value_Does_Not_Exist()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns((string)null);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.EnvironmentVariable(context, TestVariableName);

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Typed_Value()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns("123");

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.EnvironmentVariable(context, TestVariableName, 456);

                // Then
                Assert.Equal(123, result);
            }

            [Fact]
            public void Should_Return_Typed_DefaultValue_If_Value_Does_Not_Exist()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns((string)null);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentAliases.EnvironmentVariable(context, TestVariableName, 456);

                // Then
                Assert.Equal(456, result);
            }

            [Fact]
            public void Should_Throw_If_Value_Does_Not_Convert()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns("abc");

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var ex = Record.Exception(() => EnvironmentAliases.EnvironmentVariable(context, TestVariableName, 456));

                // Then
                Assert.NotNull(ex);
                Assert.NotNull(ex.InnerException);
                Assert.IsType<FormatException>(ex.InnerException);
            }
        }

        public sealed class TheIsRunningOnWindowsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => EnvironmentAliases.IsRunningOnWindows(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Theory]
            [InlineData(PlatformFamily.Linux, false)]
            [InlineData(PlatformFamily.OSX, false)]
            [InlineData(PlatformFamily.Windows, true)]
            public void Should_Return_Correct_Value(PlatformFamily family, bool expected)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(new FakeEnvironment(family));

                // When
                var result = EnvironmentAliases.IsRunningOnWindows(context);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheIsRunningOnUnixMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => EnvironmentAliases.IsRunningOnUnix(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Theory]
            [InlineData(PlatformFamily.Linux, true)]
            [InlineData(PlatformFamily.OSX, true)]
            [InlineData(PlatformFamily.Windows, false)]
            public void Should_Return_Correct_Value(PlatformFamily family, bool expected)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(new FakeEnvironment(family));

                // When
                var result = EnvironmentAliases.IsRunningOnUnix(context);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheIsRunningOnLinuxMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => EnvironmentAliases.IsRunningOnLinux(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Theory]
            [InlineData(PlatformFamily.Linux, true)]
            [InlineData(PlatformFamily.OSX, false)]
            [InlineData(PlatformFamily.Windows, false)]
            public void Should_Return_Correct_Value(PlatformFamily family, bool expected)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(new FakeEnvironment(family));

                // When
                var result = EnvironmentAliases.IsRunningOnLinux(context);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheIsRunningOnMacMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => EnvironmentAliases.IsRunningOnMacOs(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Theory]
            [InlineData(PlatformFamily.Linux, false)]
            [InlineData(PlatformFamily.OSX, true)]
            [InlineData(PlatformFamily.Windows, false)]
            public void Should_Return_Correct_Value(PlatformFamily family, bool expected)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(new FakeEnvironment(family));

                // When
                var result = EnvironmentAliases.IsRunningOnMacOs(context);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
