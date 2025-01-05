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
    public sealed class CakeTaskBuilderOfTExtensionsTests
    {
        public sealed class TheIsDependentOnMethod
        {
            [Fact]
            public void Should_Add_Dependency_To_Task()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder<string>(task);

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
                    var builder = new CakeTaskBuilder<string>(parentTask);
                    var cakeTaskBuilder = new CakeTaskBuilder<string>(childTask);

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
                    var builder = new CakeTaskBuilder<string>(parentTask);
                    var childTaskBuilder = new CakeTaskBuilder<string>(childTask);

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
                    CakeTaskBuilder<string> builder = null;
                    var childTaskBuilder = new CakeTaskBuilder<string>(childTask);

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
                    var builder = new CakeTaskBuilder<string>(parentTask);
                    CakeTaskBuilder<string> childTaskBuilder = null;

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
                var builder = new CakeTaskBuilder<string>(task);

                // When
                builder.IsDependeeOf("other");

                // Then
                Assert.Single(task.Dependees);
                Assert.Equal("other", task.Dependees[0].Name);
            }

            [Fact]
            public void Should_Add_Dependee_To_Task_From_Other()
            {
                // Given
                var task = new CakeTask("task");
                var other = new CakeTask("other");
                var builder = new CakeTaskBuilder<string>(task);
                var otherBuilder = new CakeTaskBuilder<string>(other);

                // When
                builder.IsDependeeOf(otherBuilder);

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
                    var builder = new CakeTaskBuilder<string>(task);

                    // When
                    builder.WithCriteria((ctx, data) => false);

                    // Then
                    Assert.Single(task.Criterias);
                }

                [Fact]
                public void Should_Add_Message_To_Criteria_If_Specified()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder<string>(task);

                    // When
                    builder.WithCriteria((ctx, data) => false, "Foo");

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
                    var builder = new CakeTaskBuilder<string>(task);

                    // When
                    builder.WithCriteria((ctx, data) => true);

                    // Then
                    Assert.Single(task.Criterias);
                }

                [Fact]
                public void Should_Add_Message_To_Criteria_If_Specified()
                {
                    // Given
                    var task = new CakeTask("task");
                    var builder = new CakeTaskBuilder<string>(task);

                    // When
                    builder.WithCriteria((ctx, data) => true, "Foo");

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
                    public sealed class Withctx
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

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
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            builder.Does(async (data, ctx) => { await Task.Delay(0, TestContext.Current.CancellationToken); });

                            // Then
                            Assert.Single(task.Actions);
                        }
                    }

                    public sealed class Withoutctx
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

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
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            builder.Does(async (ctx, data) => { await Task.Delay(0, TestContext.Current.CancellationToken); });

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
                    public sealed class Withctx
                    {
                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

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
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            builder.Does((data, ctx) => { });

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
                public sealed class Withctx
                {
                    public sealed class WithException
                    {
                        [Fact]
                        public void Should_Set_The_Error_Handler()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            builder.OnError((exception, ctx, data) => { });

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
                var builder = new CakeTaskBuilder<string>(task);

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
                // Given
                CakeTaskBuilder<string> builder = null;

                // When
                var result = Record.Exception(() => CakeTaskBuilderOfTExtensions.Finally(builder, (ctx, data) => { }));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder<string>(task);

                // When
                var result = Record.Exception(() => CakeTaskBuilderOfTExtensions.Finally(builder, default(Action<ICakeContext, string>)));

                // Then
                AssertEx.IsArgumentNullException(result, "finallyHandler");
            }

            [Fact]
            public void Should_Set_The_Finally_Handler()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder<string>(task);

                // When
                builder.Finally((ctx, data) => { });

                // Then
                Assert.NotNull(builder.Target.FinallyHandler);
            }
        }

        public sealed class TheDoesForEachMethod
        {
            public sealed class ForDeferredItemsWithDataAndctx
            {
                private static readonly Func<string, ICakeContext, IEnumerable<int>> FuncDeferredItemsWithDataAndctx = (data, ctx) => new[] { 1, 2, 3 };
                private static readonly Action<string, int, ICakeContext> DefaultFuncWithDataAndctx = (data, item, ctx) => { };
                private static readonly Action<string, int, ICakeContext> NullFuncWithDataAndctx = null;
                private static readonly Action<string, int, ICakeContext> ExceptionFuncWithDataAndctx = (data, item, ctx) => throw new NotImplementedException();
                public sealed class WithData
                {
                    public sealed class Withctx
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderOfTExtensions.DoesForEach(null, FuncDeferredItemsWithDataAndctx, DefaultFuncWithDataAndctx));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderOfTExtensions.DoesForEach(builder, FuncDeferredItemsWithDataAndctx, NullFuncWithDataAndctx));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);
                            var ctx = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderOfTExtensions.DoesForEach(builder, FuncDeferredItemsWithDataAndctx, DefaultFuncWithDataAndctx);

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(ctx);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);
                            var ctx = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderOfTExtensions.DoesForEach(builder, FuncDeferredItemsWithDataAndctx, ExceptionFuncWithDataAndctx);
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(ctx));

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
                    public sealed class Withctx
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderOfTExtensions.DoesForEach(default(CakeTaskBuilder<string>), (data, ctx) => new[] { 1, 2, 3 }, (data, item, ctx) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            var result = Record.Exception(() =>
                                builder.DoesForEach((data, ctx) => new[] { 1, 2, 3 }, null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public async Task Should_Add_Actions_To_Task_After_Execution()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);
                            var ctx = new CakeContextFixture().CreateContext();

                            // When
                            builder.DoesForEach((data, ctx) => new[] { 1, 2, 3 }, (item, data, ctx) => { });

                            // Then
                            Assert.Empty(builder.Target.Actions);
                            Assert.Single(builder.Target.DelayedActions);

                            // When
                            await builder.Target.Execute(ctx);

                            // Then
                            Assert.Empty(builder.Target.DelayedActions);
                            Assert.Equal(3, builder.Target.Actions.Count);
                        }

                        [Fact]
                        public async Task Should_Throw_On_First_Failed_Action()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);
                            var ctx = new CakeContextFixture().CreateContext();

                            // When
                            CakeTaskBuilderOfTExtensions.DoesForEach(builder, (data, ctx) => new[] { 1, 2, 3 }, (data, item, ctx) => throw new NotImplementedException());
                            var result = await Record.ExceptionAsync(() => builder.Target.Execute(ctx));

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
                    public sealed class Withctx
                    {
                        [Fact]
                        public void Should_Throw_If_Builder_Is_Null()
                        {
                            // Given, When
                            var result = Record.Exception(() =>
                                CakeTaskBuilderOfTExtensions.DoesForEach(default(CakeTaskBuilder<string>), new[] { 1, 2, 3 }, (data, item, ctx) => { }));

                            // Then
                            AssertEx.IsArgumentNullException(result, "builder");
                        }

                        [Fact]
                        public void Should_Throw_If_Action_Is_Null()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            var result = Record.Exception(() =>
                                builder.DoesForEach(new[] { 1, 2, 3 }, null));

                            // Then
                            AssertEx.IsArgumentNullException(result, "action");
                        }

                        [Fact]
                        public void Should_Add_Actions_To_Task()
                        {
                            // Given
                            var task = new CakeTask("task");
                            var builder = new CakeTaskBuilder<string>(task);

                            // When
                            builder.DoesForEach(new[] { 1, 2, 3 }, (data, item, ctx) => { });

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
                // Given
                CakeTaskBuilder<string> builder = null;

                // When
                var result = Record.Exception(() => CakeTaskBuilderOfTExtensions.DeferOnError(builder));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public async Task Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder<string>(task);
                var ctx = new CakeContextFixture().CreateContext();

                // When
                builder.Does((ctx, data) => throw new NotImplementedException());
                builder.Does((ctx, data) => throw new NotSupportedException());
                builder.Does((ctx, data) => throw new OutOfMemoryException());
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(ctx));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }

            [Fact]
            public async Task Should_Aggregate_Exceptions_From_Actions()
            {
                // Given
                var task = new CakeTask("task");
                var builder = new CakeTaskBuilder<string>(task);
                var ctx = new CakeContextFixture().CreateContext();

                // When
                builder.Does((ctx, data) => throw new NotImplementedException());
                builder.Does((ctx, data) => throw new NotSupportedException());
                builder.Does((ctx, data) => throw new OutOfMemoryException());
                builder.DeferOnError();
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(ctx));

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
                var builder = new CakeTaskBuilder<string>(task);
                var ctx = new CakeContextFixture().CreateContext();

                // When
                builder.Does((ctx, data) => throw new NotImplementedException());
                builder.DeferOnError();
                var result = await Record.ExceptionAsync(() => builder.Target.Execute(ctx));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }
    }
}