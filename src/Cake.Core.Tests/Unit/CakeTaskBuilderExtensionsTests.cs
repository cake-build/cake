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
                    var result = Record.Exception(() => builder.Does((Action)null));

                    // Then
                    Assert.IsArgumentNullException(result, "action");
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

        public sealed class TheOnErrorMethod
        {
            [Fact]
            public void Should_Set_The_Error_Handler()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.OnError(exception => { });

                // Then
                Assert.NotNull(builder.Task.ErrorHandler);
            }
        }

        public sealed class TheContinueOnErrorMethod
        {
            [Fact]
            public void Should_Set_The_Error_Handler()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.ContinueOnError();

                // Then
                Assert.NotNull(builder.Task.ErrorHandler);
            }
        }

        public sealed class TheFinallyMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.Finally<ActionTask>(null, () => { }));

                // Then
                Assert.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.Finally(builder, null));

                // Then
                Assert.IsArgumentNullException(result, "finallyHandler");
            }

            [Fact]
            public void Should_Set_The_Finally_Handler()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.Finally(() => { });

                // Then
                Assert.NotNull(builder.Task.FinallyHandler);
            }
        }
    }
}
