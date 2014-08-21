using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class EnvironmentExtensionsTests
    {
        public sealed class EnvMethods
        {
            private const string TestVariableName = "Test";
            private const string TestVariableValue = "Value";

            [Fact]
            public void Does_Call_Context_Check1186147061()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable(TestVariableName)
                    .Returns(TestVariableValue);

                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(environment);

                // When
                EnvironmentExtensions.HasEnvironmentVariable(context, TestVariableName);

                // Then
                environment.Received().GetEnvironmentVariable(TestVariableName);
            }

            public sealed class HasEnv
            {
                [Fact]
                public void Does_Check_For_Value()
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
                    Assert.Equal(result, true);
                }

                [Fact]
                public void Returns_False_On_Empty_Variable()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.GetEnvironmentVariable(TestVariableName)
                        .Returns("");

                    var context = Substitute.For<ICakeContext>();
                    context.Environment.Returns(environment);

                    // When
                    var result = EnvironmentExtensions.HasEnvironmentVariable(context, TestVariableName);

                    // Then
                    Assert.Equal(result, false);
                }

                [Fact]
                public void Returns_False_On_Null_Variable()
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
                    Assert.Equal(result, false);
                }
            }
            public sealed class GetEnv
            {
                [Fact]
                public void Retrieves_Value()
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
                public void Returns_Empty_Value()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.GetEnvironmentVariable(TestVariableName)
                        .Returns("");

                    var context = Substitute.For<ICakeContext>();
                    context.Environment.Returns(environment);

                    // When
                    var result = EnvironmentExtensions.EnvironmentVariable(context, TestVariableName);

                    // Then
                    Assert.Equal(result, "");
                }
            }
        }
    }
}
