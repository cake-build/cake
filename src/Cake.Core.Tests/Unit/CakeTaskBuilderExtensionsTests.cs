// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                builder.IsDependentOn("other");

                // Then
                Assert.Single(task.Dependencies);
            }

            public sealed class OnMethodTaskBuilder
            {
                [Fact]
                public void Should_Add_Dependency_To_Task()
                {
                    // Given
                    var parentTask = new CakeTask("parent");
                    var childTask = new CakeTask("child");
                    var builder = new CakeTaskBuilder(parentTask);
                    var cakeTaskBuilder = new CakeTaskBuilder(childTask);

                    // When
                    builder.IsDependentOn(cakeTaskBuilder);

                    // Then
                    Assert.Single(parentTask.Dependencies);
                }

                [Fact]
                public void Should_Add_Dependency_To_Task_With_Correct_Name()
                {
                    // Given
                    var parentTask = new CakeTask("parent");
                    var childTask = new CakeTask("child");
                    var builder = new CakeTaskBuilder(parentTask);
                    var childTaskBuilder = new CakeTaskBuilder(childTask);

                    // When
                    builder.IsDependentOn(childTaskBuilder);

                    // Then
                    Assert.Equal(parentTask.Dependencies[0].Name, childTaskBuilder.Target.Name);
                }

                [Fact]
                public void Should_Throw_If_Builder_Is_Null()
                {
                    // Given
                    var childTask = new CakeTask("child");
                    CakeTaskBuilder builder = null;
                    var childTaskBuilder = new CakeTaskBuilder(childTask);

                    // When
                    var result = Record.Exception(() => builder.IsDependentOn(childTaskBuilder));

                    // Then
                    AssertEx.IsArgumentNullException(result, "builder");
                }

                [Fact]
                public void Should_Throw_If_OtherBuilder_Is_Null()
                {
                    // Given
                    var parentTask = new CakeTask("parent");
                    var builder = new CakeTaskBuilder(parentTask);
                    CakeTaskBuilder childTaskBuilder = null;

                    // When
                    var result = Record.Exception(() => builder.IsDependentOn(childTaskBuilder));

                    // Then
                    AssertEx.IsArgumentNullException(result, "other");
                }
            }
        }

        public sealed class TheIsDependeeOfMethod
        {
            [Fact]
            public void Should_Add_Dependee_To_Task()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                builder.IsDependeeOf("other");

                // Then
                Assert.Single(task.Dependees);
                Assert.Equal("other", task.Dependees[0].Name);
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
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(false);

                    // Then
                    Assert.Single(task.Criterias);
                }

                [Fact]
                public void Should_Add_Message_To_Criteria_If_Specified()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(false, "Foo");

                    // Then
                    Assert.Single(task.Criterias);
                    Assert.Equal("Foo", task.Criterias[0].Message);
                }
            }

            public sealed class ThatAcceptsBooleanLambda
            {
                [Fact]
                public void Should_Add_Criteria_To_Task()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(() => true);

                    // Then
                    Assert.Single(task.Criterias);
                }

                [Fact]
                public void Should_Add_Message_To_Criteria_If_Specified()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(() => true, "Foo");

                    // Then
                    Assert.Single(task.Criterias);
                    Assert.Equal("Foo", task.Criterias[0].Message);
                }
            }

            public sealed class ThatAcceptsCakeContextToBooleanLambda
            {
                [Fact]
                public void Should_Add_Criteria_To_Task()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(context => true);

                    // Then
                    Assert.Single(task.Criterias);
                }

                [Fact]
                public void Should_Add_Message_To_Criteria_If_Specified()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder(task);

                    // When
                    builder.WithCriteria(context => true, "Foo");

                    // Then
                    Assert.Single(task.Criterias);
                    Assert.Equal("Foo", task.Criterias[0].Message);
                }
            }
        }

        public sealed class TheDoesMethod
        {
            public sealed class ThatIsAsynchronous
            {
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Func<ICakeContext, string, Task>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "func");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does<string>(async (data, context) => { await Task.Delay(0); });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Func<string, Task>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "func");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does<string>(async data => { await Task.Delay(0); });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Func<ICakeContext, Task>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "func");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does(async c => { await Task.Delay(0); });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Func<Task>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "func");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does(async () => { await Task.Delay(0); });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }
                }
            }

            public sealed class ThatIsSynchronous
            {
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Action<ICakeContext, string>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does<string>((data, context) => { });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Action<string>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does<string>(data => { });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Action<ICakeContext>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does(c => { });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() => builder.Does((Action)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Action_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.Does(() => { });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }
                }
            }
        }

        public sealed class TheOnErrorMethod
        {
            public sealed class WithData
            {
                public sealed class WithContext
                {
                    public sealed class WithException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError<string>((exception, context, data) => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }
                }

                public sealed class WithoutContext
                {
                    public sealed class WithException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError((Exception exception, string data) => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }

                    public sealed class WithoutException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError<string>(data => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }
                }
            }

            public sealed class WithoutData
            {
                public sealed class WithContext
                {
                    public sealed class WithException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError((exception, context) => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }
                }

                public sealed class WithoutContext
                {
                    public sealed class WithException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError(exception => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }

                    public sealed class WithoutException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            builder.OnError(() => { });

                            // Then
                            Assert.NotNull(builder.Target.ErrorHandler);
                            Assert.IsType<Func<Exception, ICakeContext, Task>>(builder.Target.ErrorHandler);
                        }
                    }
                }
            }
        }

        public sealed class TheContinueOnErrorMethod
        {
            [Fact]
            public void Should_Set_The_Error_Handler()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                builder.ContinueOnError();

                // Then
                Assert.NotNull(builder.Target.ErrorHandler);
            }
        }

        public sealed class TheFinallyMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.Finally(null, () => { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.Finally(builder, null));

                // Then
                AssertEx.IsArgumentNullException(result, "finallyHandler");
            }

            [Fact]
            public void Should_Set_The_Finally_Handler()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                builder.Finally(() => { });

                // Then
                Assert.NotNull(builder.Target.FinallyHandler);
            }
        }

        public sealed class TheReportErrorMethod
        {
            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    CakeTaskBuilderExtensions.ReportError(null, exception => { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.ReportError(builder, null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorReporter");
            }

            [Fact]
            public void Should_Set_The_Finally_Handler()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);

                // When
                builder.ReportError(exception => { });

                // Then
                Assert.NotNull(builder.Target.ErrorReporter);
            }
        }

        public sealed class TheDoesForEachMethod
        {
            public sealed class ForDeferredItemsWithContext
            {
                private static readonly Func<ICakeContext, IEnumerable<int>> FuncDeferredItemsWithContext = ctx => new[] { 1, 2, 3 };
                private static readonly Action<int> DefaultFunc = (item) => { };
                private static readonly Action<int> NullFunc = null;
                private static readonly Action<int> ExceptionFunc = (item) => throw new NotImplementedException();
                private static readonly Action<int, ICakeContext> DefaultFuncWithContext = (item, ctx) => { };
                private static readonly Action<int, ICakeContext> NullFuncWithContext = null;
                private static readonly Action<int, ICakeContext> ExceptionFuncWithContext = (item, ctx) => throw new NotImplementedException();
                private static readonly Action<string, int> DefaultFuncWithData = (data, item) => { };
                private static readonly Action<string, int> NullFuncWithData = null;
                private static readonly Action<string, int> ExceptionFuncWithData = (data, item) => throw new NotImplementedException();
                private static readonly Action<string, int, ICakeContext> DefaultFuncWithDataAndContext = (data, item, ctx) => { };
                private static readonly Action<string, int, ICakeContext> NullFuncWithDataAndContext = null;
                private static readonly Action<string, int, ICakeContext> ExceptionFuncWithDataAndContext = (data, item, ctx) => throw new NotImplementedException();
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithContext, DefaultFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, NullFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithContext, DefaultFuncWithDataAndContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithContext, ExceptionFuncWithDataAndContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithContext, DefaultFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithContext, NullFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithContext, DefaultFuncWithData);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithContext, ExceptionFuncWithData);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, FuncDeferredItemsWithContext, DefaultFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, NullFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, DefaultFuncWithContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, ExceptionFuncWithContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, FuncDeferredItemsWithContext, DefaultFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, NullFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, DefaultFunc);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithContext, ExceptionFunc);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }
            }

            public sealed class ForDeferredItemsWithData
            {
                private static readonly Func<string, IEnumerable<int>> FuncDeferredItemsWithData = data => new[] { 1, 2, 3 };
                private static readonly Action<int> DefaultFunc = (item) => { };
                private static readonly Action<int> NullFunc = null;
                private static readonly Action<int> ExceptionFunc = (item) => throw new NotImplementedException();
                private static readonly Action<int, ICakeContext> DefaultFuncWithContext = (item, ctx) => { };
                private static readonly Action<int, ICakeContext> NullFuncWithContext = null;
                private static readonly Action<int, ICakeContext> ExceptionFuncWithContext = (item, ctx) => throw new NotImplementedException();
                private static readonly Action<string, int> DefaultFuncWithData = (data, item) => { };
                private static readonly Action<string, int> NullFuncWithData = null;
                private static readonly Action<string, int> ExceptionFuncWithData = (data, item) => throw new NotImplementedException();
                private static readonly Action<string, int, ICakeContext> DefaultFuncWithDataAndContext = (data, item, ctx) => { };
                private static readonly Action<string, int, ICakeContext> NullFuncWithDataAndContext = null;
                private static readonly Action<string, int, ICakeContext> ExceptionFuncWithDataAndContext = (data, item, ctx) => throw new NotImplementedException();
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithData, DefaultFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithData, NullFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, DefaultFuncWithDataAndContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, ExceptionFuncWithDataAndContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithData, DefaultFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, NullFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, DefaultFuncWithData);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, ExceptionFuncWithData);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithData, DefaultFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, NullFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, DefaultFuncWithContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, ExceptionFuncWithContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithData, DefaultFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, NullFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, DefaultFunc);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithData, ExceptionFunc);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }
            }

            public sealed class ForDeferredItemsWithDataAndContext
            {
                private static readonly Func<string, ICakeContext, IEnumerable<int>> FuncDeferredItemsWithDataAndContext = (data, ctx) => new[] { 1, 2, 3 };
                private static readonly Action<int> DefaultFunc = (item) => { };
                private static readonly Action<int> NullFunc = null;
                private static readonly Action<int> ExceptionFunc = (item) => throw new NotImplementedException();
                private static readonly Action<int, ICakeContext> DefaultFuncWithContext = (item, ctx) => { };
                private static readonly Action<int, ICakeContext> NullFuncWithContext = null;
                private static readonly Action<int, ICakeContext> ExceptionFuncWithContext = (item, ctx) => throw new NotImplementedException();
                private static readonly Action<string, int> DefaultFuncWithData = (data, item) => { };
                private static readonly Action<string, int> NullFuncWithData = null;
                private static readonly Action<string, int> ExceptionFuncWithData = (data, item) => throw new NotImplementedException();
                private static readonly Action<string, int, ICakeContext> DefaultFuncWithDataAndContext = (data, item, ctx) => { };
                private static readonly Action<string, int, ICakeContext> NullFuncWithDataAndContext = null;
                private static readonly Action<string, int, ICakeContext> ExceptionFuncWithDataAndContext = (data, item, ctx) => throw new NotImplementedException();
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithDataAndContext, DefaultFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, FuncDeferredItemsWithDataAndContext, NullFuncWithDataAndContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, DefaultFuncWithDataAndContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, ExceptionFuncWithDataAndContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithDataAndContext, DefaultFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, NullFuncWithData));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, DefaultFuncWithData);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, ExceptionFuncWithData);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithDataAndContext, DefaultFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, NullFuncWithContext));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, DefaultFuncWithContext);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, ExceptionFuncWithContext);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, FuncDeferredItemsWithDataAndContext, DefaultFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, NullFunc));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, DefaultFunc);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, FuncDeferredItemsWithDataAndContext, ExceptionFunc);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }
            }

            public sealed class ForDeferredItems
            {
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, () => new[] { 1, 2, 3 }, (data, item, ctx) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { 1, 2, 3 }, (Action<string, int, ICakeContext>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, () => new[] { 1, 2, 3 }, (item, data, ctx) => { });

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, () => new[] { 1, 2, 3 }, (data, item, ctx) => throw new NotImplementedException());
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, () => new[] { 1, 2, 3 }, (data, item) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, () => new[] { 1, 2, 3 }, (Action<string, int>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, () => new[] { 1, 2, 3 }, (data, item) => { });

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, () => new[] { 1, 2, 3 }, (data, item) => throw new NotImplementedException());
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, () => new[] { "a", "b", "c" }, (item, context) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { 1, 2, 3 }, (Action<int, ICakeContext>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, (item, ctx) => { });

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, () => new[] { "a", "b", "c" }, item => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { 1, 2, 3 }, (Action<int>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, item => { });

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(context);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);
                            var context = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, () => new[] { "a", "b", "c" }, (item, c) => throw new NotImplementedException());
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                            // Then
                            Assert.IsType<NotImplementedException>(result);
                        }
                    }
                }
            }

            public sealed class ForImmediateItems
            {
                public sealed class WithData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, new[] { 1, 2, 3 }, (data, item, context) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { 1, 2, 3 }, (Action<string, int, ICakeContext>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Actions_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, new[] { 1, 2, 3 }, (data, item, context) => { });

                            // Then
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach<string, int>(null, new[] { 1, 2, 3 }, (data, item) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { 1, 2, 3 }, (Action<string, int>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Actions_Foreach_Item()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            CakeTaskBuilderExtensions.DoesForEach<string, int>(builder, new[] { 1, 2, 3 }, (data, item) => { });

                            // Then
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }
                    }
                }

                public sealed class WithoutData
                {
                    public sealed class WithContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, new[] { "a", "b", "c" }, (item, context) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { 1, 2, 3 }, (Action<int, ICakeContext>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Actions_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, new[] { "a", "b", "c" }, (item, context) => { });

                            // Then
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }
                    }

                    public sealed class WithoutContext
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(null, new string[0], item => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderExtensions.DoesForEach(builder, new[] { 1, 2, 3 }, (Action<int>)null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Actions_Foreach_Item()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder(task);

                            // When
                            CakeTaskBuilderExtensions.DoesForEach(builder, new[] { "a", "b", "c" }, item => { });

                            // Then
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }
                    }
                }
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
            public async Task Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.Does(() => throw new NotSupportedException());
                builder.Does(() => throw new OutOfMemoryException());
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }

            [Fact]
            public async Task Should_Aggregate_Exceptions_From_Actions()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.Does(() => throw new NotSupportedException());
                builder.Does(() => throw new OutOfMemoryException());
                builder.DeferOnError();
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                // Then
                Assert.IsType<AggregateException>(result);
                var ex = result as AggregateException;
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotImplementedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotSupportedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(OutOfMemoryException));
            }

            [Fact]
            public async Task Should_Only_Aggregate_Exceptions_When_There_Are_Many()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder(task);
                var context = new CakeContextFixture().CreateContext();

                // When
                builder.Does(() => throw new NotImplementedException());
                builder.DeferOnError();
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }
    }
}