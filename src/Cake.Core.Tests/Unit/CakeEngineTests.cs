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
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { Log = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                Assert.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheRegisterTaskMethod
        {
            [Fact]
            public void Should_Return_A_New_Task()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();

                // When
                var result = engine.RegisterTask("task");

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
                var result = engine.RegisterTask("task");

                // Then
                Assert.True(engine.Tasks.Contains(result.Task));
            }

            [Fact]
            public void Should_Throw_If_Another_Task_With_The_Same_Name_Already_Been_Added()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.RegisterTask("task");

                // When
                var result = Record.Exception(() => engine.RegisterTask("task"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Another task with the name 'task' has already been added.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Another_Task_With_The_Same_Name_Already_Been_Added_Regardless_Of_Casing()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();
                engine.RegisterTask("task");

                // When
                var result = Record.Exception(() => engine.RegisterTask("TASK"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Another task with the name 'TASK' has already been added.", result.Message);
            }
        }

        public sealed class TheRunTargetMethod
        {
            public sealed class HandlingDefaultTarget
            {
                [Fact]
                public void Should_Run_Engine_Configured_Target_If_Target_Is_Null()
                {
                    // Given
                    var fixture = new CakeEngineFixture();
                    var engine = fixture.CreateEngine();
                    engine.Target = "A";
                    bool targetCalled = false;
                    engine.RegisterTask("Default").Does(() => { throw new Exception(); });
                    engine.RegisterTask("A").Does(() => targetCalled = true );

                    // When
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, null);

                    // Then
                    Assert.True(targetCalled);
                }

                [Fact]
                public void Should_Run_Default_Target_If_Target_And_Engine_Configured_Target_Are_Both_Null()
                {
                    // Given
                    var fixture = new CakeEngineFixture();
                    var engine = fixture.CreateEngine();
                    bool defaultTargetCalled = false;
                    engine.RegisterTask("Default").Does(() => defaultTargetCalled = true);

                    // When
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, null);

                    // Then
                    Assert.True(defaultTargetCalled);
                }

            }

            [Fact]
            public void Should_Throw_If_Execution_Strategy_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                // When
                var result = Record.Exception(() => engine.RunTarget(fixture.Context, null, "A"));

                // Then
                Assert.IsArgumentNullException(result, "strategy");
            }

            [Fact]
            public void Should_Execute_Tasks_In_Order()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

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
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => false).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

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
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => true).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

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
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "Run-Some-Tests"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The target 'Run-Some-Tests' was not found.", result.Message);
            }

            [Fact]
            public void Should_Not_Catch_Exceptions_From_Task_If_ContinueOnError_Is_Not_Set()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Whoopsie"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Whoopsie", result.Message);
            }

            [Fact]
            public void Should_Swallow_Exceptions_If_ContinueOnError_Is_Set()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").ContinueOnError().Does(() => { throw new InvalidOperationException(); });

                // When, Then
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");
            }

            [Fact]
            public void Should_Invoke_Task_Error_Handler_If_Exception_Is_Thrown()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { invoked = true; });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public void Should_Propagate_Exception_From_Error_Handler()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

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
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Messages.Contains("Error: Whoopsie"));
            }

            [Fact]
            public void Should_Throw_If_Target_Cannot_Be_Reached_Due_To_Constraint()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(false);

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not reach target 'B' since it was skipped due to a criteria.", result.Message);
            }

            [Fact]
            public void Should_Run_Setup_Before_First_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(() => result.Add("Setup"));
                engine.RegisterTask("A").Does(() => result.Add("A"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Setup", result[0]);
            }

            [Fact]
            public void Should_Not_Run_Tasks_If_Setup_Failed()
            {
                // Given
                var runTask = false;
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTask("A").Does(() => runTask = true);

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.False(runTask, "Task A was executed although it shouldn't have been.");
                Assert.True(fixture.Log.Messages.Contains("Executing custom setup action..."));
            }

            [Fact]
            public void Should_Run_Teardown_After_Last_Running_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => result.Add("Setup"));
                engine.RegisterTeardownAction(() => result.Add("Teardown"));
                engine.RegisterTask("A").Does(() => result.Add("A"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("Teardown", result[2]);
            }

            [Fact]
            public void Should_Run_Teardown_After_Last_Running_Task_Even_If_Task_Failed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => { });
                engine.RegisterTeardownAction(() => { });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Fail"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result.Message);
                Assert.True(fixture.Log.Messages.Contains("Executing custom teardown action..."));
            }

            [Fact]
            public void Should_Run_Teardown_If_Setup_Failed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTeardownAction(() => { });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result.Message);
                Assert.True(fixture.Log.Messages.Contains("Executing custom teardown action..."));
            }

            [Fact]
            public void Should_Throw_Exception_Thrown_From_Setup_Action_If_Both_Setup_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => { throw new InvalidOperationException("Setup"); });
                engine.RegisterTeardownAction(() => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Setup", result.Message);
            }

            [Fact]
            public void Should_Throw_Exception_Occuring_In_Teardown_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var expected = new InvalidOperationException("Teardown");

                engine.RegisterTeardownAction(() => { throw expected; });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Log_Teardown_Exception_If_Both_Setup_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(() => { throw new InvalidOperationException("Setup"); });
                engine.RegisterTeardownAction(() => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Messages.Any(x => x.StartsWith("Teardown error:")));
            }

            [Fact]
            public void Should_Exception_Thrown_From_Task_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTeardownAction(() => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task", result.Message);
            }

            [Fact]
            public void Should_Log_Teardown_Exception_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTeardownAction(() => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task"); });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Messages.Any(x => x.StartsWith("Teardown error:")));
            }

            [Fact]
            public void Should_Execute_Finally_Handler_If_Task_Succeeds()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Finally(() => invoked = true);

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public void Should_Execute_Finally_Handler_If_Task_Throws()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .ContinueOnError()
                    .Finally(() => invoked = true);

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public void Should_Execute_Finally_Handler_After_Error_Handler_If_Task_Succeeds()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(() => result.Add("Error"))
                    .Finally(() => result.Add("Finally"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Error", result[0]);
            }

            [Fact]
            public void Should_Execute_Error_Reporter_Before_Error_Handler_If_Task_Succeeds()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(ex => result.Add("Error"))
                    .ReportError(ex => result.Add("Report"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Report", result[0]);
            }

            [Fact]
            public void Should_Swallow_Exceptions_Thrown_In_Error_Reporter()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Task"); })
                    .ReportError(ex => { throw new InvalidOperationException("Report"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.Equal("Task", result.Message);
            }

            [Fact]
            public void Should_Execute_Error_Handler_Even_If_Exception_Was_Thrown_In_Error_Reporter()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Task"); })
                    .OnError(() => { throw new InvalidOperationException("Error"); })
                    .ReportError(ex => { throw new InvalidOperationException("Report"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.Equal("Error", result.Message);
            }
        }
    }
}
