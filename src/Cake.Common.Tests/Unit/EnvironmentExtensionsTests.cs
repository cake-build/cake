using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class EnvironmentExtensionsTests
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
                var result = EnvironmentExtensions.HasEnvironmentVariable(context, TestVariableName);

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
                var result = EnvironmentExtensions.HasEnvironmentVariable(context, TestVariableName);

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
                var result = EnvironmentExtensions.HasEnvironmentVariable(context, TestVariableName);

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheGetEnvironmentVariableVariable
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
                var result = EnvironmentExtensions.EnvironmentVariable(context, TestVariableName);

                // Then
                Assert.Equal(result, TestVariableValue);
            }

            [Fact]
            public void Should_Return_Null_If_Value_Do_Not_Exist()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns((string)null);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                var result = EnvironmentExtensions.EnvironmentVariable(context, TestVariableName);

                // Then
                Assert.Null(result);
            }
        }
    }
}
