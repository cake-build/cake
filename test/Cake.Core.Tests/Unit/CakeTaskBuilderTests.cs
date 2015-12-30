using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeTaskBuilderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_Is_Provided_Task_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeTaskBuilder<ActionTask>(null));

                // Then
                Assert.IsArgumentNullException(result, "task");
            }
        }

        public sealed class TheTaskProperty
        {
            [Fact]
            public void Should_Return_The_Task_Provided_To_The_Constructor()
            {
                // Given, When
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // Then
                Assert.Equal(task, builder.Task);
            }
        }
    }
}