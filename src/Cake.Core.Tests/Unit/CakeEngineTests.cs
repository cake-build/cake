// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                AssertEx.IsArgumentNullException(result, "log");
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
                Assert.Equal("Another task with the name 'task' has already been added.", result?.Message);
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
                Assert.Equal("Another task with the name 'TASK' has already been added.", result?.Message);
            }
        }

        public sealed class TheRunTargetMethod
        {
            public sealed class WithTarget
            {
                [Fact]
                public void Should_Throw_If_Target_Is_Null()
                {
                    // Given
                    var fixture = new CakeEngineFixture();
                    var engine = fixture.CreateEngine();

                    // When
                    var result = Record.Exception(() =>
                        engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "target");
                }
            }

            public sealed class WithExecutionStrategy
            {
                [Fact]
                public void Should_Throw_If_Target_Is_Null()
                {
                    // Given
                    var fixture = new CakeEngineFixture();
                    var engine = fixture.CreateEngine();

                    // When
                    var result = Record.Exception(() =>
                        engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "target");
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
                    AssertEx.IsArgumentNullException(result, "strategy");
                }
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
            public void Should_Skip_Tasks_Where_Boolean_Criterias_Are_Not_Fulfilled()
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
            public void Should_Not_Skip_Tasks_Where_Boolean_Criterias_Are_Fulfilled()
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
            public void Should_Skip_Tasks_Where_CakeContext_Criterias_Are_Not_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();

                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(context => false).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
            }

            [Fact]
            public void Should_Not_Skip_Tasks_Where_CakeContext_Criterias_Are_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();

                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(context => true).Does(() => result.Add("B"));
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
                Assert.Equal("The target 'Run-Some-Tests' was not found.", result?.Message);
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
                Assert.Equal("Whoopsie", result?.Message);
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
                Record.Exception(() => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

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
                Assert.Equal("Totally my fault", result?.Message);
            }

            [Fact]
            public void Should_Log_Exception_Handled_By_Error_Handler()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoops"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Entries.Any(x => x.Message == "Error: Whoops"));
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
                Assert.Equal("Could not reach target 'B' since it was skipped due to a criteria.", result?.Message);
            }

            [Fact]
            public void Should_Run_Setup_Before_First_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(context => result.Add("Setup"));
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

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTask("A").Does(() => runTask = true);

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.False(runTask, "Task A was executed although it shouldn't have been.");
                Assert.True(fixture.Log.Entries.Any(x => x.Message == "Executing custom setup action..."));
            }

            [Fact]
            public void Should_Run_Teardown_After_Last_Running_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => result.Add("Setup"));
                engine.RegisterTeardownAction(context => result.Add("Teardown"));
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

                engine.RegisterSetupAction(context => { });
                engine.RegisterTeardownAction(context => { });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Fail"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result?.Message);
                Assert.True(fixture.Log.Entries.Any(x => x.Message == "Executing custom teardown action..."));
            }

            [Fact]
            public void Should_Run_Teardown_If_Setup_Failed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTeardownAction(context => { });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result?.Message);
                Assert.True(fixture.Log.Entries.Any(x => x.Message == "Executing custom teardown action..."));
            }

            [Fact]
            public void Should_Throw_Exception_Thrown_From_Setup_Action_If_Both_Setup_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Setup"); });
                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Setup", result?.Message);
            }

            [Fact]
            public void Should_Throw_Exception_Occurring_In_Teardown_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var expected = new InvalidOperationException("Teardown");

                engine.RegisterTeardownAction(context => { throw expected; });
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

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Setup"); });
                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Entries.Any(x => x.Message.StartsWith("Teardown error:")));
            }

            [Fact]
            public void Should_Exception_Thrown_From_Task_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(context => { throw new InvalidOperationException("Task"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task", result?.Message);
            }

            [Fact]
            public void Should_Log_Teardown_Exception_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task"); });

                // When
                Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Entries.Any(x => x.Message.StartsWith("Teardown error:")));
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
                Assert.Equal("Task", result?.Message);
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
                Assert.Equal("Error", result?.Message);
            }

            [Fact]
            public void Should_Run_Task_Setup_Before_Each_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B");

                // Then
                Assert.Equal(new List<string> { "TASK_SETUP:A", "Executing A", "TASK_SETUP:B", "Executing B" }, result);
            }

            [Fact]
            public void Should_Not_Run_Task_If_Task_Setup_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => { throw new Exception("fake exception"); });
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                Record.Exception(() => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B"));

                // Then
                Assert.Equal(new List<string>(), result);

                Assert.True(fixture.Log.Entries.Any(x => x.Message == "Executing custom task setup action (A)..."));
                Assert.False(fixture.Log.Entries.Any(x => x.Message == "Executing custom task setup action (B)..."));
            }

            [Fact]
            public void Should_Run_Task_Teardown_After_Each_Running_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP:A",
                        "Executing A",
                        "TASK_TEARDOWN:A",
                        "TASK_SETUP:B",
                        "Executing B",
                        "TASK_TEARDOWN:B"
                    }, result);
            }

            [Fact]
            public void Should_Run_Task_Teardown_After_Each_Running_Task_Even_If_Task_Is_Skipped()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B")
                    .Does(() => result.Add("Executing B"))
                    .WithCriteria(() => false)
                    .IsDependentOn("A");
                engine.RegisterTask("C").Does(() => result.Add("Executing C")).IsDependentOn("B");

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP:A",
                        "Executing A",
                        "TASK_TEARDOWN:A",
                        "TASK_SETUP:B",
                        "TASK_TEARDOWN:B",
                        "TASK_SETUP:C",
                        "Executing C",
                        "TASK_TEARDOWN:C"
                    }, result);
            }

            [Fact]
            public void Should_Run_Task_Teardown_After_Each_Running_Task_Even_If_Task_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() =>
                {
                    result.Add("FAILING (A)");
                    throw new InvalidOperationException("Fail");
                });

                // When
                var exception = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Fail", exception?.Message);
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP:A",
                        "FAILING (A)",
                        "TASK_TEARDOWN:A"
                    }, result);
            }

            [Fact]
            public void Should_Run_Task_Teardown_If_Task_Setup_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context =>
                {
                    throw new InvalidOperationException("Fail");
                });
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() =>
                {
                    result.Add("Executing A");
                });

                // When
                var exception = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Fail", exception?.Message);
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_TEARDOWN:A"
                    }, result);
            }

            [Fact]
            public void Should_Throw_Exception_Thrown_From_Task_Setup_Action_If_Both_Task_Setup_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTaskSetupAction(
                    context => { throw new InvalidOperationException("Task Setup: " + context.Task.Name); });
                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Setup: A", result?.Message);
            }

            [Fact]
            public void Should_Throw_Exception_Occurring_In_Task_Teardown_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A");

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Teardown: A", result?.Message);
            }

            [Fact]
            public void Should_Log_Task_Teardown_Exception_If_Both_Task_Setup_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTaskSetupAction(
                    context => { throw new InvalidOperationException("Task Setup: " + context.Task.Name); });
                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Setup: A", result?.Message);
                Assert.True(fixture.Log.Entries.Any(x => x.Message.StartsWith("Task Teardown error (A):")));
            }

            [Fact]
            public void Should_Log_Exception_Thrown_From_Task_If_Both_Task_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task: A"); });

                // When
                var result = Record.Exception(() =>
                    engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task: A", result?.Message);
            }

            [Fact]
            public void Should_Log_Task_Teardown_Exception_If_Both_Task_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task: A"); });

                // When
                Record.Exception(() => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.True(fixture.Log.Entries.Any(x => x.Message.StartsWith("Task Teardown error (A):")));
            }

            [Fact]
            public void Should_Return_Report_That_Contains_Executed_Tasks_In_Order()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");

                // When
                var report = engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

                // Then
                Assert.Equal(3, report.Count());
                Assert.Equal("A", report.ElementAt(0).TaskName);
                Assert.Equal("B", report.ElementAt(1).TaskName);
                Assert.Equal("C", report.ElementAt(2).TaskName);
            }

            [Fact]
            public void Should_Return_Report_That_Marks_Executed_Tasks_As_Executed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B").Does(() => { });
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false).Does(() => { });

                // When
                var report = engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(CakeTaskExecutionStatus.Executed, report.First(e => e.TaskName == "A").ExecutionStatus);
            }

            [Fact]
            public void Should_Return_Report_That_Marks_Skipped_Tasks_As_Skipped()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B");
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false);

                // When
                var report = engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(CakeTaskExecutionStatus.Skipped, report.First(e => e.TaskName == "C").ExecutionStatus);
            }

            [Fact]
            public void Should_Return_Report_That_Marks_Delegated_Tasks_As_Delegated()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B");
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false);

                // When
                var report = engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(CakeTaskExecutionStatus.Delegated, report.First(e => e.TaskName == "B").ExecutionStatus);
            }
        }

        public sealed class TheSetupEvent
        {
            [Fact]
            public void Should_Raise_Setup_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<SetupEventArgs>(
                    handler => engine.Setup += handler,
                    handler => engine.Setup -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
                Assert.Equal(fixture.Context, result.Arguments.Context);
            }

            [Fact]
            public void Should_Invoke_All_Handlers()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.Setup += (sender, args) =>
                {
                    list.Add("HANDLER_1");
                };
                engine.Setup += (sender, args) =>
                {
                    list.Add("HANDLER_2");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(2, list.Count);
                Assert.Contains(list, s => s == "HANDLER_1");
                Assert.Contains(list, s => s == "HANDLER_2");
            }

            [Fact]
            public void Should_Raise_The_Setup_Event_Only_Once()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");
                engine.Setup += (sender, args) =>
                {
                    list.Add("SETUP_EVENT");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "SETUP_EVENT"
                    }, list);
            }
        }

        public sealed class TheTaskSetupEvent
        {
            [Fact]
            public void Should_Raise_Task_Setup_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskSetupEventArgs>(
                    handler => engine.TaskSetup += handler,
                    handler => engine.TaskSetup -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_Setup_Event_With_Task_Context()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskSetupEventArgs>(
                    handler => engine.TaskSetup += handler,
                    handler => engine.TaskSetup -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.IsType<TaskSetupEventArgs>(result.Arguments);
                Assert.NotNull(result.Arguments.TaskSetupContext.Task);
                Assert.Equal("A", result.Arguments.TaskSetupContext.Task.Name);
            }

            [Fact]
            public void Should_Raise_Task_Setup_Event_After_Setup_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.Setup += (sender, args) =>
                {
                    list.Add("SETUP_EVENT");
                };
                engine.TaskSetup += (sender, args) =>
                {
                    list.Add("TASK_SETUP_EVENT_1");
                };
                engine.TaskSetup += (sender, args) =>
                {
                    list.Add("TASK_SETUP_EVENT_2");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "SETUP_EVENT",
                        "TASK_SETUP_EVENT_1",
                        "TASK_SETUP_EVENT_2"
                    }, list);
            }

            [Fact]
            public void Should_Raise_Task_Setup_Event_For_All_Tasks()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.TaskSetup += (sender, args) =>
                {
                    list.Add("TASK_SETUP_EVENT_" + args.TaskSetupContext.Task.Name);
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP_EVENT_A",
                        "TASK_SETUP_EVENT_B"
                    }, list);
            }
        }

        public sealed class TheTaskTeardownEvent
        {
            [Fact]
            public void Should_Raise_Task_Teardown_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskTeardownEventArgs>(
                    handler => engine.TaskTeardown += handler,
                    handler => engine.TaskTeardown -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_Teardown_Event_With_Task_Context()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskTeardownEventArgs>(
                    handler => engine.TaskTeardown += handler,
                    handler => engine.TaskTeardown -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.IsType<TaskTeardownEventArgs>(result.Arguments);
                Assert.NotNull(result.Arguments.TaskTeardownContext.Task);
                Assert.Equal("A", result.Arguments.TaskTeardownContext.Task.Name);
            }

            [Fact]
            public void Should_Raise_Task_Teardown_Event_After_Task_Setup_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.TaskSetup += (sender, args) =>
                {
                    list.Add("TASK_SETUP_EVENT");
                };
                engine.TaskTeardown += (sender, args) =>
                {
                    list.Add("TASK_TEARDOWN_EVENT");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP_EVENT",
                        "TASK_TEARDOWN_EVENT"
                    }, list);
            }

            [Fact]
            public void Should_Raise_Task_Teardown_Event_For_All_Tasks()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.TaskTeardown += (sender, args) =>
                {
                    list.Add("TASK_TEARDOWN_EVENT_" + args.TaskTeardownContext.Task.Name);
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "B");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_TEARDOWN_EVENT_A",
                        "TASK_TEARDOWN_EVENT_B"
                    }, list);
            }
        }

        public sealed class TheTeardownEvent
        {
            [Fact]
            public void Should_Raise_Teardown_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TeardownEventArgs>(
                    handler => engine.Teardown += handler,
                    handler => engine.Teardown -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Teardown_Event_With_Teardown_Context()
            {
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TeardownEventArgs>(
                    handler => engine.Teardown += handler,
                    handler => engine.Teardown -= handler,
                    () => engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A"));

                // Then
                Assert.NotNull(result);
                Assert.Equal(fixture.Context.Environment, result.Arguments.TeardownContext.Environment);
                Assert.True(result.Arguments.TeardownContext.Successful);
            }

            [Fact]
            public void Should_Invoke_All_Handlers()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.Teardown += (sender, args) =>
                {
                    list.Add("HANDLER_1");
                };
                engine.Teardown += (sender, args) =>
                {
                    list.Add("HANDLER_2");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(2, list.Count);
                Assert.Contains(list, s => s == "HANDLER_1");
                Assert.Contains(list, s => s == "HANDLER_2");
            }

            [Fact]
            public void Should_Raise_The_Teardown_Event_Only_Once()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");
                engine.Teardown += (sender, args) =>
                {
                    list.Add("TEARDOWN_EVENT");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "C");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TEARDOWN_EVENT"
                    }, list);
            }

            [Fact]
            public void Should_Raise_The_Teardown_Event_After_Task_Teardown_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.TaskTeardown += (sender, args) =>
                {
                    list.Add("TASK_TEARDOWN_EVENT");
                };
                engine.Teardown += (sender, args) =>
                {
                    list.Add("TEARDOWN_EVENT");
                };

                // When
                engine.RunTarget(fixture.Context, fixture.ExecutionStrategy, "A");

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_TEARDOWN_EVENT",
                        "TEARDOWN_EVENT"
                    }, list);
            }
        }
    }
}