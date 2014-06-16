using System;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class ActionTaskTests
    {
        public sealed class TheAddActionMethod
        {
            [Fact]
            private void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new ActionTask("task");

                // When
                var exception = Record.Exception(() => task.AddAction(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("action", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Add_Action_To_Task()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.AddAction(c => { });

                // Then
                Assert.Equal(1, task.Actions.Count);
            }
        }
    }
}
