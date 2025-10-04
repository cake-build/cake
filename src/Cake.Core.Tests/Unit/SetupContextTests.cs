using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class SetupContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Returns_Empty_Tasks_When_Input_Is_Null()
            {
                var cakeContextMock = Substitute.For<ICakeContext>();
                var cakeTaskInfoMock = Substitute.For<ICakeTaskInfo>();

                // Given, When
                var result = new SetupContext(cakeContextMock, cakeTaskInfoMock, null);

                // Then
                Assert.NotNull(result.TasksToExecute);
                Assert.Empty(result.TasksToExecute);
            }

            [Fact]
            public void Returns_Injected_Tasks_When_Input_Is_Not_Null()
            {
                var cakeContextMock = Substitute.For<ICakeContext>();
                var cakeTaskInfoMock = Substitute.For<ICakeTaskInfo>();

                // Given, When
                var result = new SetupContext(cakeContextMock, cakeTaskInfoMock, new[]
                {
                    cakeTaskInfoMock
                });

                // Then
                Assert.NotNull(result.TasksToExecute);
                Assert.Single(result.TasksToExecute);
            }
        }
    }
}
