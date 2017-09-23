// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Tests.Fixtures;
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

        public sealed class TheIsDependentOnMethodTaskBuilder
        {
            [Fact]
            public void Should_Add_Dependency_To_Task()
            {
                // Given
                var parentTask = new ActionTask("parent");
                var childTask = new ActionTask("child");
                var builder = new CakeTaskBuilder<ActionTask>(parentTask);
                var cakeTaskBuilder = new CakeTaskBuilder<ActionTask>(childTask);

                // When
                builder.IsDependentOn(cakeTaskBuilder);

                // Then
                Assert.Equal(1, parentTask.Dependencies.Count);
            }

            [Fact]
            public void Should_Add_Dependency_To_Task_With_Correct_Name()
            {
                // Given
                var parentTask = new ActionTask("parent");
                var childTask = new ActionTask("child");
                var builder = new CakeTaskBuilder<ActionTask>(parentTask);
                var childTaskBuilder = new CakeTaskBuilder<ActionTask>(childTask);

                // When
                builder.IsDependentOn(childTaskBuilder);

                // Then
                Assert.Equal(parentTask.Dependencies[0].TargetTaskName, childTaskBuilder.Task.Name);
            }

            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given
                var childTask = new ActionTask("child");
                CakeTaskBuilder<ActionTask> builder = null;
                var childTaskBuilder = new CakeTaskBuilder<ActionTask>(childTask);

                // When
                var result = Record.Exception(() => builder.IsDependentOn(childTaskBuilder));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_OtherBuilder_Is_Null()
            {
                // Given
                var parentTask = new ActionTask("parent");
                var builder = new CakeTaskBuilder<ActionTask>(parentTask);
                CakeTaskBuilder<ActionTask> childTaskBuilder = null;

                // When
                var result = Record.Exception(() => builder.IsDependentOn(childTaskBuilder));

                // Then
                AssertEx.IsArgumentNullException(result, "other");
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

            public sealed class ThatAcceptsBooleanLambda
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

            public sealed class ThatAcceptsCakeContextToBooleanLambda
            {
                [Fact]
                public void Should_Add_Criteria_To_Task()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    builder.WithCriteria(context => true);

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
                public void Should_Throw_If_Action_Is_Null()
                {
                    // Given
                    var task = new ActionTask("task");
                    var builder = new CakeTaskBuilder<ActionTask>(task);

                    // When
                    var result = Record.Exception(() => builder.Does((Action)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "action");
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
                    Assert.Single(task.Actions);
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
                    Assert.Single(task.Actions);
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
                AssertEx.IsArgumentNullException(result, "builder");
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
                AssertEx.IsArgumentNullException(result, "finallyHandler");
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

        public sealed class TheReportErrorMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.ReportError<ActionTask>(null, exception => { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.ReportError(builder, null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorReporter");
            }

            [Fact]
            public void Should_Set_The_Finally_Handler()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                builder.ReportError(exception => { });

                // Then
                Assert.NotNull(builder.Task.ErrorReporter);
            }
        }

        public sealed class TheDoesForEachMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.DoesForEach(null, new string[0], exception => { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Add_Actions_Foreach_Item()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { "a", "b", "c" }, item => { });

                // Then
                Assert.Equal(3, builder.Task.Actions.Count);
            }

            [Fact]
            public void Should_Support_Item_And_Context_Action()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // When
                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { "a", "b", "c" }, (item, context) => { });

                // Then
                Assert.Equal(3, builder.Task.Actions.Count);
            }

            [Fact]
            public void Should_Defer_Delegate_Items_Until_Execution()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, (item) => { });

                // Then
                Assert.Empty(builder.Task.Actions);
                Assert.Single(builder.Task.DelayedActions);

                // When
                builder.Task.Execute(context);

                // Then
                Assert.Empty(builder.Task.DelayedActions);
                Assert.Equal(3, builder.Task.Actions.Count);
            }

            [Fact]
            public void Should_Support_Defered_Item_And_Context_Action()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, (item, c) => { });

                // Then
                Assert.Empty(builder.Task.Actions);
                Assert.Single(builder.Task.DelayedActions);

                // When
                builder.Task.Execute(context);

                // Then
                Assert.Empty(builder.Task.DelayedActions);
                Assert.Equal(3, builder.Task.Actions.Count);
            }

            [Fact]
            public void Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, (item, c) => throw new NotImplementedException());
                var result = Record.Exception(() => builder.Task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }

        public sealed class TheDeferOnErrorMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.DeferOnError(null));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.Does(() => throw new NotSupportedException());
                builder.Does(() => throw new OutOfMemoryException());
                var result = Record.Exception(() => builder.Task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }

            [Fact]
            public void Should_Aggregate_Exceptions_From_Actions()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.Does(() => throw new NotSupportedException());
                builder.Does(() => throw new OutOfMemoryException());
                builder.DeferOnError();
                var result = Record.Exception(() => builder.Task.Execute(context));

                // Then
                Assert.IsType<AggregateException>(result);
                var ex = result as AggregateException;
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotImplementedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotSupportedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(OutOfMemoryException));
            }

            [Fact]
            public void Should_Only_Aggregate_Exceptions_When_There_Are_Many()
            {
                // Given
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.DeferOnError();
                var result = Record.Exception(() => builder.Task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }
    }
}