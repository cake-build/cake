// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
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
                var result = Record.Exception(() => new ActionTask(null));

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
                var result = Record.Exception(() => new ActionTask(name));

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
                var task = new ActionTask("task");

                // When
                task.AddDependency("other");

                // Then
                Assert.Equal(1, task.Dependencies.Count);
                Assert.Equal("other", task.Dependencies[0]);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Already_Exist()
            {
                // Given
                var task = new ActionTask("task");
                task.AddDependency("other");

                // When
                var result = Record.Exception(() => task.AddDependency("other"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The task 'task' already have a dependency on 'other'.", result?.Message);
            }
        }

        public sealed class TheAddCriteriaMethod
        {
            [Fact]
            public void Should_Throw_If_Criteria_Is_Null()
            {
                // Given
                var task = new ActionTask("task");

                // When
                var result = Record.Exception(() => task.AddCriteria(null));

                // Then
                AssertEx.IsArgumentNullException(result, "criteria");
            }

            [Fact]
            public void Should_Add_Criteria()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.AddCriteria(context => true);

                // Then
                Assert.Equal(1, task.Criterias.Count);
            }
        }

        public sealed class TheSetErrorHandlerMethod
        {
            [Fact]
            public void Should_Throw_If_Error_Handler_Is_Null()
            {
                // Given
                var task = new ActionTask("task");

                // When
                var result = Record.Exception(() => task.SetErrorHandler(null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorHandler");
            }

            [Fact]
            public void Should_Set_Error_Handler()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.SetErrorHandler(e => { });

                // Then
                Assert.NotNull(task.ErrorHandler);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Error_Handler()
            {
                // Given
                var task = new ActionTask("task");
                task.SetErrorHandler(e => { });

                // When
                var result = Record.Exception(() => task.SetErrorHandler(e => { }));

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
                var task = new ActionTask("task");

                // When
                var result = Record.Exception(() => task.SetFinallyHandler(null));

                // Then
                AssertEx.IsArgumentNullException(result, "finallyHandler");
            }

            [Fact]
            public void Should_Set_Finally_Handler()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.SetFinallyHandler(() => { });

                // Then
                Assert.NotNull(task.FinallyHandler);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Finally_Handler()
            {
                // Given
                var task = new ActionTask("task");
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
                var task = new ActionTask("task");

                // When
                var result = Record.Exception(() => task.SetErrorReporter(null));

                // Then
                AssertEx.IsArgumentNullException(result, "errorReporter");
            }

            [Fact]
            public void Should_Set_Error_Reporter()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.SetErrorReporter(exception => { });

                // Then
                Assert.NotNull(task.ErrorReporter);
            }

            [Fact]
            public void Should_Throw_If_Setting_More_Than_One_Error_Reporter()
            {
                // Given
                var task = new ActionTask("task");
                task.SetErrorReporter(error => { });

                // When
                var result = Record.Exception(() => task.SetErrorReporter(exception => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("There can only be one error reporter per task.", result?.Message);
            }
        }

        [Fact]
        public void Should_Implement_ICakeTaskInfo()
        {
            // Given
            var task = new ActionTask("task");
            task.AddDependency("dependency1");
            task.AddDependency("dependency2");
            task.Description = "my description";

            // When
            ICakeTaskInfo result = task;

            // Then
            Assert.IsAssignableFrom<ICakeTaskInfo>(task);
            Assert.Equal("task", result.Name);
            Assert.Equal("my description", result.Description);
            Assert.Equal(new[] { "dependency1", "dependency2" }, result.Dependencies.ToArray());
        }
    }
}