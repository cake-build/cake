using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests
{
    public sealed class CakeEngineTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture {FileSystem = null};

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Environment = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Log = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Create_Default_Globber_If_The_Provided_One_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Globber = null };

                // When
                var engine = fixture.CreateEngine();

                // Then
                Assert.NotNull(engine.Globber);
            }

            [Fact]
            public void Should_Keep_Provided_Globber_If_It_Is_Not_Null()
            {
                // Given
                var fixture = new CakeEngineFixture();

                // When
                var engine = fixture.CreateEngine();

                // Then
                Assert.Equal(fixture.Globber, engine.Globber);
            }
        }
            
        public sealed class TheTaskMethod
        {
            [Fact]
            public void Should_Return_A_New_Task()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();

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
                var engine = new CakeEngineFixture().CreateEngine();

                // When
                var result = engine.Task("task");

                // Then
                Assert.True(engine.Tasks.Contains(result));         
            }

            [Fact]
            public void Should_Throw_If_Another_Task_With_The_Same_Name_Already_Been_Added()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
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
                var engine = new CakeEngineFixture().CreateEngine();
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
                var engine = new CakeEngineFixture().CreateEngine();
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
                var engine = new CakeEngineFixture().CreateEngine();
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

            [Fact]
            public void Should_Throw_If_Target_Was_Not_Found()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();

                // When
                var result = Record.Exception(() => engine.Run("Run-Some-Tests"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The target 'Run-Some-Tests' was not found.", result.Message);
            }
        }
    }
}
