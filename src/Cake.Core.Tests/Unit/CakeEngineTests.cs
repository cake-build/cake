using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeEngineTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { FileSystem = null };

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
            public void Should_Throw_If_Arguments_Are_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Arguments = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("arguments", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Globber = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("globber", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_ProcessRunner_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { ProcessRunner = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("processRunner", ((ArgumentNullException)result).ParamName);
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
                Assert.Equal("task", result.Task.Name);
            }

            [Fact]
            public void Should_Register_Created_Task()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();

                // When
                var result = engine.Task("task");

                // Then
                Assert.True(engine.Tasks.Contains(result.Task));
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

            [Fact]
            public void Should_Throw_If_Another_Task_With_The_Same_Name_Already_Been_Added_Regardless_Of_Casing()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("task");

                // When
                var result = Record.Exception(() => engine.Task("TASK"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Another task with the name 'TASK' has already been added.", result.Message);
            }
        }

        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Execute_Tasks_In_Order()
            {
                // Given
                var result = new List<string>();
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A").Does(() => result.Add("A"));
                engine.Task("B").IsDependentOn("A").Does(() => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget("C");

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
                engine.Task("A").Does(() => result.Add("A"));
                engine.Task("B").IsDependentOn("A").WithCriteria(() => false).Does(() => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget("C");

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
                engine.Task("A").Does(() => result.Add("A"));
                engine.Task("B").IsDependentOn("A").WithCriteria(() => true).Does(() => result.Add("B"));
                engine.Task("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget("C");

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
                var result = Record.Exception(() => engine.RunTarget("Run-Some-Tests"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The target 'Run-Some-Tests' was not found.", result.Message);
            }

            [Fact]
            public void Should_Not_Catch_Exceptions_From_Task_If_ContinueOnError_Is_Not_Set()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A").Does(() => { throw new InvalidOperationException("Whoopsie"); });

                // When
                var result = Record.Exception(() => engine.RunTarget("A"));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Whoopsie", result.Message);
            }

            [Fact]
            public void Should_Swallow_Exceptions_If_ContinueOnError_Is_Set()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A").ContinueOnError().Does(() => { throw new InvalidOperationException(); });

                // When, Then
                engine.RunTarget("A");
            }

            [Fact]
            public void Should_Invoke_Task_Error_Handler_If_Exception_Is_Thrown()
            {
                // Given
                var invoked = false;
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { invoked = true; });

                // When
                Record.Exception(() => engine.RunTarget("A"));

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public void Should_Propagate_Exception_From_Error_Handler()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                var result = Record.Exception(() => engine.RunTarget("A"));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Totally my fault", result.Message);
            }

            [Fact]
            public void Should_Log_Exception_Handled_By_Error_Handler()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.Task("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                Record.Exception(() => engine.RunTarget("A"));

                // Then
                Assert.True(fixture.Log.Messages.Contains("Error: Whoopsie"));
            }

            [Fact]
            public void Should_Throw_If_Target_Cannot_Be_Reached_Due_To_Constraint()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.Task("A");
                engine.Task("B").IsDependentOn("A").WithCriteria(false);

                // When
                var result = Record.Exception(() => engine.RunTarget("B"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not reach target 'B' since it was skipped due to a criteria.", result.Message);
            }
        }
    }
}
