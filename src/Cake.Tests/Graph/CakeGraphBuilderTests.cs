using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Graph;
using Xunit;

namespace Cake.Tests.Graph
{
    public sealed class CakeGraphBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Add_All_Tasks_As_Nodes_In_Graph()
            {
                // Given, When
                var tasks = new List<CakeTask> {new CakeTask("A"), new CakeTask("B")};
                var graph = CakeGraphBuilder.Build(tasks);

                // Then
                Assert.Equal(2, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Create_Edges_Between_Dependencies()
            {
                // Given
                var tasks = new List<CakeTask>
                {
                    new CakeTask("A"), new CakeTask("B").IsDependentOn("A")
                };
                var graph = CakeGraphBuilder.Build(tasks);

                // When
                var result = graph.Edges.SingleOrDefault(x => x.Start.Name == "A" && x.End.Name == "B");

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Throw_Exception_When_Depending_On_Task_That_Does_Not_Exist()
            {
                // Given
                var tasks = new List<CakeTask>
                {
                    new CakeTask("A").IsDependentOn("C")
                };

                // When
                var exception = Assert.Throws<CakeException>(() => CakeGraphBuilder.Build(tasks));

                // Then
                Assert.Equal("Task 'A' is dependent on task 'C' which do not exist.", exception.Message);
            }
        }
    }
}
