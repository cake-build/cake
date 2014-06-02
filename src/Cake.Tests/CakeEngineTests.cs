using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Xunit;

namespace Cake.Tests
{
    public sealed class CakeEngineTests
    {
        public sealed class TheTaskMethod
        {
            [Fact]
            public void Should_Return_A_New_Task()
            {
                // Given
                var engine = new CakeEngine();

                // When
                var result = engine.Task("task");

                // Then
                Assert.NotNull(result);
                Assert.Equal("task", result.Name);
            }

            [Fact]
            public void Should_Register_Created_Task()
            {
                // Given
                var engine = new CakeEngine();

                // When
                var result = engine.Task("task");

                // Then
                Assert.True(engine.Tasks.Contains(result));         
            }

            [Fact]
            public void Should_Throw_If_Another_Task_With_The_Same_Name_Already_Been_Added()
            {
                // Given
                var engine = new CakeEngine();
                engine.Task("task");

                // When
                var result = Record.Exception(() => engine.Task("task"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Another task with the name 'task' has already been added.", result.Message);
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Execute_Tasks_In_Order()
            {
                // Given
                var result = new List<string>();
                var engine = new CakeEngine();
                engine.Task("A").Does(x => result.Add("A"));
                engine.Task("B").IsDependentOn("A").Does(x => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(x => result.Add("C"));

                // When
                engine.Run("C");

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
            }

            [Fact]
            public void Should_Skip_Tasks_Where_Criterias_Are_Not_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var engine = new CakeEngine();
                engine.Task("A").Does(x => result.Add("A"));
                engine.Task("B").IsDependentOn("A").WithCriteria(c => false).Does(x => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(x => result.Add("C"));

                // When
                engine.Run("C");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
            }

            [Fact]
            public void Should_Not_Skip_Tasks_Where_Criterias_Are_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var engine = new CakeEngine();
                engine.Task("A").Does(x => result.Add("A"));
                engine.Task("B").IsDependentOn("A").WithCriteria(c => true).Does(x => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(x => result.Add("C"));

                // When
                engine.Run("C");

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
            }
        }
    }
}
