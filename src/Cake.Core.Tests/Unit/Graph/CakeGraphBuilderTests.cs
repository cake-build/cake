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
                var tasks = new List<CakeTask> { new ActionTask("A"), new ActionTask("B") };
                var graph = CakeGraphBuilder.Build(tasks);

                // Then
                Assert.Equal(2, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Create_Edges_Between_Dependencies()
            {
                // Given
                var task1 = new ActionTask("A");
                var task2 = new ActionTask("B");
                task2.AddDependency("A");

                var tasks = new List<CakeTask>
                {
                    task1, task2
                };
                var graph = CakeGraphBuilder.Build(tasks);

                // When
                var result = graph.Edges.SingleOrDefault(x => x.Start == "A" && x.End == "B");

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Throw_Exception_When_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var task = new ActionTask("A");
                task.AddDependency("C");
                var tasks = new List<CakeTask> { task };

                // When
                var result = Assert.Throws<CakeException>(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.Equal("Task 'A' is dependent on task 'C' which do not exist.", result.Message);
            }
        }
    }
}
