// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core.Graph;
using Xunit;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests.Unit.Graph
{
    public sealed class CakeGraphBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public async Task Should_Add_All_Tasks_As_Nodes_In_Graph()
            {
                // Given, When
                var graph = CakeGraphBuilder.Build(
                                [
                                    new CakeTask("A"),
                                    new CakeTask("B")
                                ]);

                // Then
                await Verify(graph.Nodes);
            }

            [Fact]
            public async Task Should_Create_Edges_Between_Dependencies()
            {
                // Given
                var task1 = new CakeTask("A");
                var task2 = new CakeTask("B");
                task2.AddDependency("A");

                var graph = CakeGraphBuilder.Build(
                                [
                                    task1,
                                    task2
                                ]);

                // When
                var result = graph.Edges.SingleOrDefault();

                // Then
                await Verify(result);
            }

            [Fact]
            public async Task Should_Create_Edges_Between_Reversed_Dependencies()
            {
                // Given
                var task1 = new CakeTask("A");
                var task2 = new CakeTask("B");
                task2.AddDependee("A");

                var graph = CakeGraphBuilder.Build(
                                [
                                    task1,
                                    task2
                                ]);

                // When
                var result = graph.Edges.SingleOrDefault();

                // Then
                await Verify(result);
            }

            [Fact]
            public void Should_Throw_When_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependency("C");

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build([task]));

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

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build([task]));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Throw_When_Reverse_Dependency_Is_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new CakeTask("A");
                task.AddDependee("C");

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build([task]));

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

                // When
                var result = Record.Exception(() => CakeGraphBuilder.Build([task]));

                // Then
                Assert.Null(result);
            }
        }
    }
}