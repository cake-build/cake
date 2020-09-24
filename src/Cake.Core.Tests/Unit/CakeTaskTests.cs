// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeTaskTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeTask(null));

                // Then
                AssertEx.IsArgumentNullException(result, "name");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            [InlineData("  ")]
            [InlineData(" \n")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given, When
                var result = Record.Exception(() => new CakeTask(name));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("Task name cannot be empty.", result?.Message);
            }
        }

        public sealed class TheAddDependencyMethod
        {
            [Fact]
            public void Should_Add_Dependency_If_Not_Already_Present()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.AddDependency("other");

                // Then
                Assert.Single(task.Dependencies);
                Assert.Equal("other", task.Dependencies[0].Name);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Already_Exist()
            {
                // Given
                var task = new CakeTask("task");
                task.AddDependency("other");

                // When
                var result = Record.Exception(() => task.AddDependency("other"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The task 'task' already have a dependency on 'other'.", result?.Message);
            }
        }

        public sealed class TheAddReverseDependencyMethod
        {
            [Fact]
            public void Should_Add_Dependency_If_Not_Already_Present()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.AddDependee("other");

                // Then
                Assert.Single(task.Dependees);
                Assert.Equal("other", task.Dependees[0].Name);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Already_Exist()
            {
                // Given
                var task = new CakeTask("task");
                task.AddDependee("other");

                // When
                var result = Record.Exception(() => task.AddDependee("other"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The task 'task' already is a dependee of 'other'.", result?.Message);
            }
        }

        public sealed class TheAddCriteriaMethod
        {
            [Fact]
            public void Should_Throw_If_Criteria_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.AddCriteria(null));

                // Then
                AssertEx.IsArgumentNullException(result, "predicate");
            }

            [Fact]
            public void Should_Add_Criteria()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.AddCriteria(context => true);

                // Then
                Assert.Single(task.Criterias);
            }
        }

        public sealed class TheSetErrorHandlerMethod
        {
            [Fact]
            public void Should_Throw_If_Error_Handler_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.SetErrorHandler(null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorHandler");
            }

            [Fact]
            public void Should_Set_Error_Handler()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.SetErrorHandler((e, c) => { });

                // Then
                Assert.NotNull(task.ErrorHandler);
                Assert.IsType<Func<Exception, ICakeContext, Task>>(task.ErrorHandler);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Error_Handler()
            {
                // Given
                var task = new CakeTask("task");
                task.SetErrorHandler((e, c) => { });

                // When
                var result = Record.Exception(() => task.SetErrorHandler((e, c) => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("There can only be one error handler per task.", result?.Message);
            }
        }

        public sealed class TheSetFinallyHandlerMethod
        {
            [Fact]
            public void Should_Throw_If_Finally_Handler_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.SetFinallyHandler(null));

                // Then
                AssertEx.IsArgumentNullException(result, "finallyHandler");
            }

            [Fact]
            public void Should_Set_Finally_Handler()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.SetFinallyHandler(() => { });

                // Then
                Assert.NotNull(task.FinallyHandler);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Finally_Handler()
            {
                // Given
                var task = new CakeTask("task");
                task.SetFinallyHandler(() => { });

                // When
                var result = Record.Exception(() => task.SetFinallyHandler(() => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("There can only be one finally handler per task.", result?.Message);
            }
        }

        public sealed class TheSetErrorReportHandlerMethod
        {
            [Fact]
            public void Should_Throw_If_Error_Reporter_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.SetErrorReporter(null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorReporter");
            }

            [Fact]
            public void Should_Set_Error_Reporter()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.SetErrorReporter(exception => { });

                // Then
                Assert.NotNull(task.ErrorReporter);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Error_Reporter()
            {
                // Given
                var task = new CakeTask("task");
                task.SetErrorReporter(error => { });

                // When
                var result = Record.Exception(() => task.SetErrorReporter(exception => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("There can only be one error reporter per task.", result?.Message);
            }
        }

        public sealed class TheAddActionMethod
        {
            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.AddAction(null));

                // Then
                AssertEx.IsArgumentNullException(result, "action");
            }

            [Fact]
            public void Should_Add_Action_To_Task()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.AddAction(c => Task.CompletedTask);

                // Then
                Assert.Single(task.Actions);
            }

            [Fact]
            public async Task Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new CakeTask("task");
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.Actions.Add((c) => throw new NotSupportedException());
                task.Actions.Add((c) => throw new OutOfMemoryException());
                var result = await Record.ExceptionAsync(() => task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }

            [Fact]
            public async Task Should_Aggregate_Exceptions_From_Actions()
            {
                // Given
                var task = new CakeTask("task");
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.Actions.Add((c) => throw new NotSupportedException());
                task.Actions.Add((c) => throw new OutOfMemoryException());
                task.SetDeferExceptions(true);
                var result = await Record.ExceptionAsync(() => task.Execute(context));

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
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.SetDeferExceptions(true);
                var result = await Record.ExceptionAsync(() => task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }

        [Fact]
        public void Should_Implement_ICakeTaskInfo()
        {
            // Given
            var task = new CakeTask("task");
            task.AddDependency("dependency1");
            task.AddDependency("dependency2");
            task.Description = "my description";

            // When
            ICakeTaskInfo result = task;

            // Then
            Assert.IsAssignableFrom<ICakeTaskInfo>(task);
            Assert.Equal("task", result.Name);
            Assert.Equal("my description", result.Description);
            Assert.Equal(new[] { "dependency1", "dependency2" }, result.Dependencies.Select(x => x.Name).ToArray());
        }
    }
}