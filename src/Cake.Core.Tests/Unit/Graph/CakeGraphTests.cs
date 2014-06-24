using System;
using System.Linq;
using Cake.Core.Graph;
using Xunit;

namespace Cake.Core.Tests.Unit.Graph
{
    public sealed class CakeGraphTests
    {
        public sealed class TheAddMethod
        {
            [Fact]
            public void Should_Throw_If_Provided_Node_Is_Null()
            {
                // Given
                var graph = new CakeGraph();

                // When
                var exception = Record.Exception(() => graph.Add(null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("node", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Add_Node_To_Graph()
            {
                // Given
                var graph = new CakeGraph();

                // When
                graph.Add("start");

                // Then
                Assert.Equal(1, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Throw_If_Node_Already_Is_Present_In_Graph()
            {
                // Given
                var graph = new CakeGraph();
                graph.Add("start");

                // When
                var exception = Record.Exception(() => graph.Add("start"));

                // Then
                Assert.IsType<CakeException>(exception);
                Assert.Equal("Node has already been added to graph.", exception.Message);
            }
        }

        public sealed class TheConnectMethod
        {
            [Fact]
            public void Should_Create_Edge_Between_Connected_Nodes()
            {
                // Given
                var graph = new CakeGraph();

                // When
                graph.Connect("start", "end");

                // Then
                Assert.Equal("start", graph.Edges[0].Start);
                Assert.Equal("end", graph.Edges[0].End);
            }

            [Fact]
            public void Should_Add_Start_Node_If_Missing_To_Node_Collection()
            {
                // Given
                var graph = new CakeGraph();
                graph.Add("end");

                // When
                graph.Connect("start", "end");      
     
                // Then
                Assert.Equal(2, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Add_End_Node_If_Missing_To_Node_Collection()
            {
                // Given
                var graph = new CakeGraph();
                graph.Add("start");

                // When
                graph.Connect("start", "end");

                // Then
                Assert.Equal(2, graph.Nodes.Count);
            }

            [Fact]
            public void Should_Not_Create_Edge_Between_Connected_Nodes_If_An_Edge_Already_Exist()
            {
                // Given
                var graph = new CakeGraph();
                graph.Connect("start", "end");

                // When
                graph.Connect("start", "end");

                // Then
                Assert.Equal(1, graph.Edges.Count);                
            }

            [Fact]
            public void Should_Throw_If_Edge_Is_Reflexive()
            {
                // Given
                var graph = new CakeGraph();

                // When
                var exception = Record.Exception(() => graph.Connect("start", "start"));

                // Then
                Assert.IsType<CakeException>(exception);
                Assert.Equal("Reflexive edges in graph are not allowed.", exception.Message);
            }

            [Fact]
            public void Should_Throw_If_Edge_Is_Unidirectional()
            {
                // Given
                var graph = new CakeGraph();
                graph.Connect("start", "end");

                // When
                var exception = Record.Exception(() => graph.Connect("end", "start"));

                // Then
                Assert.IsType<CakeException>(exception);
                Assert.Equal("Unidirectional edges in graph are not allowed.", exception.Message);
            }
        }

        public sealed class TheExistMethod
        {
            [Fact]
            public void Should_Return_True_If_Node_Exist()
            {
                // Given
                var graph = new CakeGraph();
                graph.Add("start");

                // When
                var result = graph.Exist("start");

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Node_Do_Not_Exist()
            {
                // Given
                var graph = new CakeGraph();
                graph.Add("start");

                // When
                var result = graph.Exist("other");

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheTraverseMethod
        {
            [Fact]
            public void Should_Return_Empty_Collection_Of_Nodes_If_Target_Was_Not_Found()
            {
                // Given
                var graph = new CakeGraph();
                graph.Connect("A", "B");
                graph.Connect("C", "D");
                graph.Connect("B", "C");

                // When
                var result = graph.Traverse("E").ToArray();

                // Then
                Assert.Equal(0, result.Length);
            }

            [Fact]
            public void Should_Traverse_Graph_In_Correct_Order()
            {
                // Given
                var graph = new CakeGraph();
                graph.Connect("A", "B");
                graph.Connect("C", "D");
                graph.Connect("B", "C");

                // When
                var result = graph.Traverse("D").ToArray();

                // Then
                Assert.Equal(4, result.Length);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
                Assert.Equal("D", result[3]);
            }

            [Fact]
            public void Should_Skip_Nodes_That_Are_Not_On_The_Way_To_The_Target()
            {
                // Given
                var graph = new CakeGraph();
                graph.Connect("A", "B");
                graph.Connect("B", "C");
                graph.Connect("B", "D");
                graph.Connect("D", "E");                

                // When
                var result = graph.Traverse("E").ToArray();                

                // Then
                Assert.Equal(4, result.Length);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("D", result[2]);
                Assert.Equal("E", result[3]);
            }

            [Fact]
            public void Should_Throw_If_Encountering_Circular_Reference()
            {
                var graph = new CakeGraph();
                graph.Connect("A","B");
                graph.Connect("B","C");
                graph.Connect("C", "A");

                var exception = Record.Exception(() => graph.Traverse("C"));

                Assert.IsType<CakeException>(exception);
                Assert.Equal("Graph contains circular references.", exception.Message);
            }
        }
    }
}
