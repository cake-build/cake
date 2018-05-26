// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core.Graph;
using Xunit;

namespace Cake.Core.Tests.Unit.Graph
{
    public sealed class CakeGraphBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Add_All_Tasks_As_Nodes_In_Graph()
            {
                // Given, When
                var tasks = new List<CakeTask> { new CakeTask("A"), new CakeTask("B") };
                var graph = CakeGraphBuilder.Build(tasks);

                // Then
                Assert.Equal(2, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Create_Edges_Between_Dependencies()
            {
                // Given
                var task1 = new CakeTask("A");
                var task2 = new CakeTask("B");
                task2.AddDependency("A");

                var tasks = new List<CakeTask>
                {
                    task1, task2
                };
                var graph = CakeGraphBuilder.Build(tasks);

                // When
                var result = graph.Edges.SingleOrDefault();

                // Then
                Assert.NotNull(result);
                Assert.Equal("A", result.Start);
                Assert.Equal("B", result.End);
            }

            [Fact]
            public void Should_Create_Edges_Between_Reversed_Dependencies()
            {
                // Given
                var task1 = new CakeTask("A");
                var task2 = new CakeTask("B");
                task2.AddDependee("A");

                var graph = CakeGraphBuilder.Build(new List<CakeTask>
                {
                    task1, task2
                });

                // When
                var result = graph.Edges.SingleOrDefault();

                // Then
                Assert.NotNull(result);
                Assert.Equal("B", result.Start);
                Assert.Equal("A", result.End);
            }

            [Fact]
            public void Should_Throw_When_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependency("C");
                var tasks = new List<CakeTask> { task };

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.NotNull(result);
                Assert.Equal("Task 'A' is dependent on task 'C' which does not exist.", result.Message);
            }

            [Fact]
            public void Should_Not_Throw_When_Depending_On_Optional_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependency("C", false);
                var tasks = new List<CakeTask> { task };

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Throw_When_Reverse_Dependency_Is_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependee("C");
                var tasks = new List<CakeTask> { task };

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.NotNull(result);
                Assert.Equal("Task 'A' has specified that it's a dependency for task 'C' which does not exist.", result.Message);
            }

            [Fact]
            public void Should_Not_Throw_When_An_Reverse_Dependency_Is_Depending_On_An_Optional_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependee("C", required: false);
                var tasks = new List<CakeTask> { task };

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.Null(result);
            }
        }
    }
}