using System;
using Cake.Core;
using Xunit;
using Xunit.Extensions;

namespace Cake.Tests
{
    public sealed class CakeTaskTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given, When
                var exception = Record.Exception(() => new CakeTask(null));

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
                var exception = Record.Exception(() => new CakeTask(name));

                // Then
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("Task name cannot be empty.", exception.Message);
            }
        }

        public sealed class TheIsDependentOnMethod
        {
            [Fact]
            public void Should_Add_Dependency_If_Not_Already_Present()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.IsDependentOn("other");

                // Then
                Assert.Equal(1, task.Dependencies.Count);
                Assert.Equal("other", task.Dependencies[0]);
            }

            [Fact]
            public void Should_Throw_If_Dependency_Already_Exist()
            {
                // Given
                var task = new CakeTask("task");
                task.IsDependentOn("other");

                // When
                var exception = Record.Exception(() => task.IsDependentOn("other"));

                // Then
                Assert.IsType<CakeException>(exception);
                Assert.Equal("The task 'task' already have a dependency on 'other'.", exception.Message);
            }
        }

        public sealed class TheWithCriteriaMethod
        {
            [Fact]
            public void Should_Throw_If_Criteria_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var exception = Record.Exception(() => task.WithCriteria(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("criteria", ((ArgumentNullException) exception).ParamName);
            }

            [Fact]
            public void Should_Add_Criteria()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.WithCriteria(c => true);

                // Then
                Assert.Equal(1, task.Criterias.Count);
            }
        }

        public sealed class TheDoesMethod
        {
            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var exception = Record.Exception(() => task.Does(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("action", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Add_Action()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.Does(c => { });

                // Then
                Assert.Equal(1, task.Actions.Count);
            }
        }
    }
}
