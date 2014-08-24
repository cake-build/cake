using System;
using Xunit;
using Xunit.Extensions;

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
                var exception = Record.Exception(() => new ActionTask(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("name", ((ArgumentNullException)exception).ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            [InlineData("  ")]
            [InlineData(" \n")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given, When
                var exception = Record.Exception(() => new ActionTask(name));

                // Then
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("Task name cannot be empty.", exception.Message);
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
                var exception = Record.Exception(() => task.AddDependency("other"));

                // Then
                Assert.IsType<CakeException>(exception);
                Assert.Equal("The task 'task' already have a dependency on 'other'.", exception.Message);
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
                var exception = Record.Exception(() => task.AddCriteria(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("criteria", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Add_Criteria()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.AddCriteria(() => true);

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
                var exception = Record.Exception(() => task.SetErrorHandler(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("errorHandler", ((ArgumentNullException)exception).ParamName);
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
                Assert.Equal("There can only be one error handler per task.", result.Message);
            }
        }
    }
}
