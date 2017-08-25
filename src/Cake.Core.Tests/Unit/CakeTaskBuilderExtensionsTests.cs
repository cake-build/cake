// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                Assert.Equal(parentTask.Dependencies[0], childTaskBuilder.Task.Name);
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
    }
}