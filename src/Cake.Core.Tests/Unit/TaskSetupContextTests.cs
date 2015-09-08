using System;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class TaskSetupContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Task_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TaskTeardownContext(null,TimeSpan.Zero, false));

                // Then
                Assert.IsArgumentNullException(result, "task");
            }
        }
    }
}