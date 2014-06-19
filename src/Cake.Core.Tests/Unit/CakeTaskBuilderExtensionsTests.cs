using System;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeTaskBuilderExtensionsTests
    {
        public sealed class TheIsDependentOnMethod
        {
            [Fact]
            public void Should_Add_Dependency_To_Task()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.IsDependentOn("other");

                // Then
                Assert.Equal(1, task.Dependencies.Count);
            }
        }

        public sealed class TheWithCriteriaMethod
        {
            public sealed class ThatAcceptsBoolean
            {
                [Fact]
                public void Should_Add_Criteria_To_Task()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    builder.WithCriteria(false);

                    // Then
                    Assert.Equal(1, task.Criterias.Count);
                }
            }

            public sealed class ThatAcceptsLambda
            {
                [Fact]
                public void Should_Add_Criteria_To_Task()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    builder.WithCriteria(() => true);

                    // Then
                    Assert.Equal(1, task.Criterias.Count);
                }
            }
        }

        public sealed class TheDoesMethod
        {
            public class WithoutContext
            {
                [Fact]
                private void Should_Throw_If_Action_Is_Null()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);


                    // When
                    var exception = Record.Exception(() => builder.Does((Action)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(exception);
                    Assert.Equal("action", ((ArgumentNullException)exception).ParamName);
                }

                [Fact]
                public void Should_Add_Action_To_Task()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    builder.Does(() => { });

                    // Then
                    Assert.Equal(1, task.Actions.Count);
                }
            }

            public class WithContext
            {
                [Fact]
                public void Should_Add_Action_To_Task()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    builder.Does(c => { });

                    // Then
                    Assert.Equal(1, task.Actions.Count);
                }
            }
        }

        public sealed class TheContinueOnErrorMethod
        {
            [Fact]
            public void Should_Set_The_Property()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.ContinueOnError();

                // Then
                Assert.True(builder.Task.ContinueOnError);
            }
        }
    }
}
