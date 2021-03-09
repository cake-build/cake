// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeEngineTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Data_Service_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture { DataService = null };

                // When
                var result = Record.Exception(() => fixture.CreateEngine());

                // Then
                AssertEx.IsArgumentNullException(result, "data");
            }

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
                Assert.Equal("task", result.Target.Name);
            }

            [Fact]
            public void Should_Register_Created_Task()
            {
                // Given
                var engine = new CakeEngineFixture().CreateEngine();

                // When
                var result = engine.RegisterTask("task");

                // Then
                Assert.Contains(result.Target, engine.Tasks);
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

        public sealed class TheRunTargetAsyncMethod
        {
            [Fact]
            public async Task Should_Throw_If_Execution_Settings_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public async Task Should_Throw_If_Execution_Strategy_Is_Null()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                // When
                var result = await Record.ExceptionAsync(() => engine.RunTargetAsync(fixture.Context, null, settings));

                // Then
                AssertEx.IsArgumentNullException(result, "strategy");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("\t ")]
            public async Task Should_Throw_If_Target_Is_Not_Specified(string target)
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget(target);

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                AssertEx.IsArgumentException(result, "settings", "No target specified.");
            }

            [Fact]
            public async Task Should_Execute_Tasks_In_Order()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("E");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));
                engine.RegisterTask("D").IsDependentOn("C").IsDependeeOf("E").Does(() => { result.Add("D"); });
                engine.RegisterTask("E").Does(() => { result.Add("E"); });

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(5, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
                Assert.Equal("D", result[3]);
                Assert.Equal("E", result[4]);
            }

            [Fact]
            public async Task Should_Only_Execute_Target_Task_In_Exclusive_Mode()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C").UseExclusiveTarget();
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));
                engine.RegisterTask("D").IsDependentOn("C").IsDependeeOf("E").Does(() => { result.Add("D"); });
                engine.RegisterTask("E").Does(() => { result.Add("E"); });

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Single(result);
                Assert.Equal("C", result[0]);
            }

            [Fact]
            public async Task Should_Skip_Tasks_Where_Boolean_Criterias_Are_Not_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => false).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
                Assert.Contains(fixture.Log.Entries, e => e.Message == "Skipping task: B");
            }

            [Fact]
            public async Task Should_Only_Write_Single_Skipped_Entry_To_Report_If_Multiple_Boolean_Criterias_Evaluated_To_False()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("Default");

                engine.RegisterTask("Default").IsDependentOn("A");
                engine.RegisterTask("A")
                    .WithCriteria(() => false)
                    .WithCriteria(() => false, "Foo")
                    .WithCriteria(context => false)
                    .WithCriteria(context => false, "Bar")
                    .WithCriteria<string>((context, data) => false)
                    .WithCriteria<string>((context, data) => false, "Baz");

                // When
                var result = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);
                var entries = result.ToList();

                // Then
                Assert.Equal(2, entries.Count);
                Assert.Equal("A", entries[0].TaskName);
                Assert.Equal(CakeTaskExecutionStatus.Skipped, entries[0].ExecutionStatus);
                Assert.Equal("Default", entries[1].TaskName);
                Assert.Equal(CakeTaskExecutionStatus.Delegated, entries[1].ExecutionStatus);
            }

            [Fact]
            public async Task Should_Skip_Tasks_Where_Boolean_Criterias_Are_Not_Fulfilled_And_Write_Reason_To_Log()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("C");

                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => false, "Foo").Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
                Assert.Contains(fixture.Log.Entries, e => e.Message == "Skipping task: Foo");
            }

            [Fact]
            public async Task Should_Skip_Tasks_Where_Boolean_Criterias_Are_Not_Fulfilled_Async()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() =>
                {
                    result.Add("A");
                    return Task.CompletedTask;
                });
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => false).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
                Assert.Contains(fixture.Log.Entries, e => e.Message == "Skipping task: B");
            }

            [Fact]
            public async Task Should_Not_Skip_Tasks_Where_Boolean_Criterias_Are_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(() => true).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
            }

            [Fact]
            public async Task Should_Skip_Tasks_Where_CakeContext_Criterias_Are_Not_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");

                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(context => false).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
                Assert.Contains(fixture.Log.Entries, e => e.Message == "Skipping task: B");
            }

            [Fact]
            public async Task Should_Skip_Tasks_Where_CakeContext_Criterias_Are_Not_Fulfilled_And_Write_Reason_To_Log()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("C");

                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(context => false, "Foo").Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("C", result[1]);
                Assert.Contains(fixture.Log.Entries, e => e.Message == "Skipping task: Foo");
            }

            [Fact]
            public async Task Should_Not_Skip_Tasks_Where_CakeContext_Criterias_Are_Fulfilled()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");

                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => result.Add("A"));
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(context => true).Does(() => result.Add("B"));
                engine.RegisterTask("C").IsDependentOn("B").Does(() => result.Add("C"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("A", result[0]);
                Assert.Equal("B", result[1]);
                Assert.Equal("C", result[2]);
            }

            [Fact]
            public async Task Should_Throw_If_Target_Was_Not_Found()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("Run-Some-Tests");
                var engine = fixture.CreateEngine();

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The target 'Run-Some-Tests' was not found.", result?.Message);
            }

            [Fact]
            public async Task Should_Not_Catch_Exceptions_From_Task_If_ContinueOnError_Is_Not_Set()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Whoopsie"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Whoopsie", result?.Message);
            }

            [Fact]
            public async Task Should_Swallow_Exceptions_If_ContinueOnError_Is_Set()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").ContinueOnError().Does(() => { throw new InvalidOperationException(); });

                // When, Then
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);
            }

            [Fact]
            public void Should_Not_Throw_If_More_Than_One_Setup_Action_Is_Registered()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(context => { });

                // When
                var result = Record.Exception(() => engine.RegisterSetupAction(context => { }));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Throw_If_More_Than_One_Setup_Action_With_Same_Data_Type_Is_Registered()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(context => "foo");

                // When
                var result = Record.Exception(() => engine.RegisterSetupAction(context => "bar"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("More than one setup action have been registered that accepts data of type 'System.String'.", result.Message);
            }

            [Fact]
            public void Should_Not_Throw_If_More_Than_One_Teardown_Action_Is_Registered()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTeardownAction(context => { });

                // When
                var result = Record.Exception(() => engine.RegisterTeardownAction(context => { }));

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Throw_If_More_Than_One_Task_Setup_Action_Is_Registered()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => { });

                // When
                var result = Record.Exception(() => engine.RegisterTaskSetupAction(context => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("More than one task setup action have been registered.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Teardown_With_Non_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => { });
                engine.RegisterTeardownAction<string>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a teardown action that accepts data of " +
                             "type 'System.String', but no such data has been setup.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Task_Setup_With_Non_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => { });
                engine.RegisterTaskSetupAction<string>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a task setup action that accepts data of " +
                             "type 'System.String', but no such data has been setup.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Task_Teardown_With_Non_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => { });
                engine.RegisterTaskTeardownAction<string>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a task teardown action that accepts data of " +
                             "type 'System.String', but no such data has been setup.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Teardown_Action_With_Wrong_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => string.Empty);
                engine.RegisterTeardownAction<CakeTask>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a teardown action that accepts data of " +
                             "type 'Cake.Core.CakeTask', but no such data has been setup.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Task_Setup_Action_With_Wrong_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => string.Empty);
                engine.RegisterTaskSetupAction<CakeTask>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a task setup action that accepts data of " +
                             "type 'Cake.Core.CakeTask', but no such data has been setup.", result.Message);
            }

            [Fact]
            public async Task Should_Throw_If_Registering_Task_Teardown_Action_With_Wrong_Registered_Data_Type()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => { });
                engine.RegisterSetupAction(context => string.Empty);
                engine.RegisterTaskTeardownAction<CakeTask>((context, data) => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Trying to register a task teardown action that accepts data of " +
                             "type 'Cake.Core.CakeTask', but no such data has been setup.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_More_Than_One_Task_Teardown_Action_Is_Registered()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterTaskTeardownAction(context => { });

                // When
                var result = Record.Exception(() => engine.RegisterTaskTeardownAction(context => { }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("More than one task teardown action have been registered.", result.Message);
            }

            [Fact]
            public async Task Should_Invoke_Task_Error_Handler_If_Exception_Is_Thrown()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { invoked = true; });

                // When
                await Record.ExceptionAsync(() => engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public async Task Should_Propagate_Exception_From_Error_Handler()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Totally my fault", result?.Message);
            }

            [Fact]
            public async Task Should_Log_Exception_Handled_By_Error_Handler()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoops"); })
                    .OnError(exception => { throw new InvalidOperationException("Totally my fault"); });

                // When
                await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Contains(fixture.Log.Entries, x => x.Message == "Error: Whoops");
            }

            [Fact]
            public async Task Should_Throw_If_Target_Cannot_Be_Reached_Due_To_Constraint()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A").WithCriteria(false);

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not reach target 'B' since it was skipped due to a criteria.", result?.Message);
            }

            [Fact]
            public async Task Should_Run_Setup_Before_First_Task()
            {
                // Given
                var result = new List<string>();
                var settings = new ExecutionSettings().SetTarget("A");
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(context => result.Add("Setup"));
                engine.RegisterTask("A").Does(() => result.Add("A"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Setup", result[0]);
            }

            [Fact]
            public async Task Should_Not_Run_Tasks_If_Setup_Failed()
            {
                // Given
                var runTask = false;
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTask("A").Does(() => runTask = true);

                // When
                await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.False(runTask, "Task A was executed although it shouldn't have been.");
                Assert.Contains(fixture.Log.Entries, x => x.Message == "Executing custom setup action...");
            }

            [Fact]
            public async Task Should_Run_Teardown_After_Last_Running_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => result.Add("Setup"));
                engine.RegisterTeardownAction(context => result.Add("Teardown"));
                engine.RegisterTask("A").Does(() => result.Add("A"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("Teardown", result[2]);
            }

            [Fact]
            public async Task Should_Run_Teardown_After_Last_Running_Task_Even_If_Task_Failed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { });
                engine.RegisterTeardownAction(context => { });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Fail"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result?.Message);
                Assert.Contains(fixture.Log.Entries, x => x.Message == "Executing custom teardown action...");
            }

            [Fact]
            public async Task Should_Run_Teardown_If_Setup_Failed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Fail"); });
                engine.RegisterTeardownAction(context => { });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Fail", result?.Message);
                Assert.Contains(fixture.Log.Entries, x => x.Message == "Executing custom teardown action...");
            }

            [Fact]
            public async Task Should_Throw_Exception_Thrown_From_Setup_Action_If_Both_Setup_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => { throw new InvalidOperationException("Setup"); });
                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Setup", result?.Message);
            }

            [Fact]
            public async Task Should_Throw_Exception_Occurring_In_Teardown_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                var expected = new InvalidOperationException("Teardown");

                engine.RegisterTeardownAction(context => { throw expected; });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public async Task Should_Throw_Aggregated_Exception_If_Multiple_Teardown_Methods_Throw_And_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTeardownAction(context => throw new InvalidOperationException("Foo"));
                engine.RegisterTeardownAction(context => throw new InvalidOperationException("Bar"));
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                var ex = Assert.IsType<AggregateException>(result);
                Assert.Equal(2, ex.InnerExceptions.Count);
                Assert.Equal("Foo", ex.InnerExceptions[0].Message);
                Assert.Equal("Bar", ex.InnerExceptions[1].Message);
            }

            [Fact]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public async Task Should_Throw_Single_Exception_If_Only_One_Of_Multiple_Teardown_Methods_Throw_And_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTeardownAction(context => { });
                engine.RegisterTeardownAction(context => throw new InvalidOperationException("Foo"));
                engine.RegisterTeardownAction(context => { });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                var ex = Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Foo", ex.Message);
            }

            [Fact]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public async Task Should_Execute_All_Teardown_Methods_Even_If_One_Or_More_Throws()
            {
                // Given
                var captured = new List<string>();
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTeardownAction(context => { captured.Add("First"); });
                engine.RegisterTeardownAction(context => throw new InvalidOperationException("Foo"));
                engine.RegisterTeardownAction(context => { captured.Add("Third"); });
                engine.RegisterTeardownAction(context => throw new InvalidOperationException("Bar"));
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Contains("First", captured);
                Assert.Contains("Third", captured);
            }

            [Fact]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public async Task Should_Log_Teardown_Exception_If_Both_Setup_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterSetupAction(context => throw new InvalidOperationException("Setup"));
                engine.RegisterTeardownAction(context => throw new CakeException("Teardown #1"));
                engine.RegisterTeardownAction(context => throw new CakeException("Teardown #2"));
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Teardown error: Teardown #1"));
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Teardown error: Teardown #2"));
            }

            [Fact]
            public async Task Should_Throw_Exception_Thrown_From_Task_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTeardownAction(context => { throw new InvalidOperationException("Teardown"); });
                engine.RegisterTask("A").Does(context => { throw new InvalidOperationException("Task"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task", result?.Message);
            }

            [Fact]
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public async Task Should_Log_Teardown_Exception_If_Both_Task_And_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var engine = fixture.CreateEngine();
                var settings = new ExecutionSettings().SetTarget("A");

                engine.RegisterTask("A").Does(() => throw new InvalidOperationException("Task"));
                engine.RegisterTeardownAction(context => throw new CakeException("Teardown #1"));
                engine.RegisterTeardownAction(context => throw new CakeException("Teardown #2"));

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Teardown error: Teardown #1"));
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Teardown error: Teardown #2"));
            }

            [Fact]
            public async Task Should_Execute_Finally_Handler_If_Task_Succeeds()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Finally(() => invoked = true);

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public async Task Should_Execute_Finally_Handler_If_Task_Throws()
            {
                // Given
                var invoked = false;
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .ContinueOnError()
                    .Finally(() => invoked = true);

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.True(invoked);
            }

            [Fact]
            public async Task Should_Execute_Finally_Handler_After_Error_Handler_If_Task_Succeeds()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(() => result.Add("Error"))
                    .Finally(() => result.Add("Finally"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Error", result[0]);
            }

            [Fact]
            public async Task Should_Execute_Error_Reporter_Before_Error_Handler_If_Task_Succeeds()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Whoopsie"); })
                    .OnError(ex => result.Add("Error"))
                    .ReportError(ex => result.Add("Report"));

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Report", result[0]);
            }

            [Fact]
            public async Task Should_Swallow_Exceptions_Thrown_In_Error_Reporter()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Task"); })
                    .ReportError(ex => { throw new InvalidOperationException("Report"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Equal("Task", result?.Message);
            }

            [Fact]
            public async Task Should_Execute_Error_Handler_Even_If_Exception_Was_Thrown_In_Error_Reporter()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A")
                    .Does(() => { throw new InvalidOperationException("Task"); })
                    .OnError(() => { throw new InvalidOperationException("Error"); })
                    .ReportError(ex => { throw new InvalidOperationException("Report"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Equal("Error", result?.Message);
            }

            [Fact]
            public async Task Should_Run_Task_Setup_Before_Each_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(new List<string> { "TASK_SETUP:A", "Executing A", "TASK_SETUP:B", "Executing B" }, result);
            }

            [Fact]
            public async Task Should_Not_Run_Task_If_Task_Setup_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => { throw new Exception("fake exception"); });
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                await Record.ExceptionAsync(() => engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Equal(new List<string>(), result);

                Assert.Contains(fixture.Log.Entries, x => x.Message == "Executing custom task setup action (A)...");
                Assert.DoesNotContain(fixture.Log.Entries, x => x.Message == "Executing custom task setup action (B)...");
            }

            [Fact]
            public async Task Should_Run_Task_Teardown_After_Each_Running_Task()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() => result.Add("Executing A"));
                engine.RegisterTask("B").Does(() => result.Add("Executing B")).IsDependentOn("A");

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
            public async Task Should_Run_Task_Teardown_After_Each_Running_Task_Even_If_Task_Is_Skipped()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
            public async Task Should_Run_Task_Teardown_After_Each_Running_Task_Even_If_Task_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTaskSetupAction(context => result.Add("TASK_SETUP:" + context.Task.Name));
                engine.RegisterTaskTeardownAction(context => result.Add("TASK_TEARDOWN:" + context.Task.Name));
                engine.RegisterTask("A").Does(() =>
                {
                    result.Add("FAILING (A)");
                    throw new InvalidOperationException("Fail");
                });

                // When
                var exception = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

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
            public async Task Should_Run_Task_Teardown_If_Task_Setup_Failed()
            {
                // Given
                var result = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                var exception = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

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
            public async Task Should_Throw_Exception_Thrown_From_Task_Setup_Action_If_Both_Task_Setup_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTaskSetupAction(
                    context => { throw new InvalidOperationException("Task Setup: " + context.Task.Name); });
                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Setup: A", result?.Message);
            }

            [Fact]
            public async Task Should_Throw_Exception_Occurring_In_Task_Teardown_If_No_Previous_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A");

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Teardown: A", result?.Message);
            }

            [Fact]
            public async Task Should_Log_Task_Teardown_Exception_If_Both_Task_Setup_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTaskSetupAction(
                    context => { throw new InvalidOperationException("Task Setup: " + context.Task.Name); });
                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task Setup: A", result?.Message);
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Task Teardown error (A):"));
            }

            [Fact]
            public async Task Should_Log_Exception_Thrown_From_Task_If_Both_Task_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task: A"); });

                // When
                var result = await Record.ExceptionAsync(() =>
                    engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Task: A", result?.Message);
            }

            [Fact]
            public async Task Should_Log_Task_Teardown_Exception_If_Both_Task_And_Task_Teardown_Actions_Throw()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();

                engine.RegisterTaskTeardownAction(
                    context => { throw new InvalidOperationException("Task Teardown: " + context.Task.Name); });
                engine.RegisterTask("A").Does(() => { throw new InvalidOperationException("Task: A"); });

                // When
                await Record.ExceptionAsync(() => engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.Contains(fixture.Log.Entries, x => x.Message.StartsWith("Task Teardown error (A):"));
            }

            [Fact]
            public async Task Should_Return_Report_That_Contains_Entry_For_Setup_First()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterSetupAction(x => { });
                engine.RegisterSetupAction(x => { });
                engine.RegisterTask("A");

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, report.Count());
                Assert.Equal("Setup", report.ElementAt(0).TaskName);
                Assert.Equal(CakeReportEntryCategory.Setup, report.ElementAt(0).Category);
                Assert.Equal("A", report.ElementAt(1).TaskName);
                Assert.Equal(CakeReportEntryCategory.Task, report.ElementAt(1).Category);
            }

            [Fact]
            public async Task Should_Return_Report_That_Contains_Entry_For_Teardown_Last()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTeardownAction(x => { });
                engine.RegisterTeardownAction(x => { });
                engine.RegisterTask("A");

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, report.Count());
                Assert.Equal("A", report.ElementAt(0).TaskName);
                Assert.Equal(CakeReportEntryCategory.Task, report.ElementAt(0).Category);
                Assert.Equal("Teardown", report.ElementAt(1).TaskName);
                Assert.Equal(CakeReportEntryCategory.Teardown, report.ElementAt(1).Category);
            }

            [Fact]
            public async Task Should_Return_Report_That_Contains_Executed_Tasks_In_Order()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(3, report.Count());
                Assert.Equal("A", report.ElementAt(0).TaskName);
                Assert.Equal("B", report.ElementAt(1).TaskName);
                Assert.Equal("C", report.ElementAt(2).TaskName);
            }

            [Fact]
            public async Task Should_Return_Report_That_Marks_Executed_Tasks_As_Executed()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B").Does(() => { });
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false).Does(() => { });

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(CakeTaskExecutionStatus.Executed, report.First(e => e.TaskName == "A").ExecutionStatus);
            }

            [Fact]
            public async Task Should_Return_Report_That_Marks_Skipped_Tasks_As_Skipped()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B");
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false);

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(CakeTaskExecutionStatus.Skipped, report.First(e => e.TaskName == "C").ExecutionStatus);
            }

            [Fact]
            public async Task Should_Return_Report_That_Marks_Delegated_Tasks_As_Delegated()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A").IsDependentOn("B");
                engine.RegisterTask("B").IsDependentOn("C");
                engine.RegisterTask("C").WithCriteria(() => false);

                // When
                var report = await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<SetupEventArgs>(
                    handler => engine.Setup += handler,
                    handler => engine.Setup -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
                Assert.Equal(fixture.Context, result.Arguments.Context);
            }

            [Fact]
            public void Should_Raise_PostSetup_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<SetupEventArgs>(
                    handler => engine.PostSetup += handler,
                    handler => engine.PostSetup -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
                Assert.Equal(fixture.Context, result.Arguments.Context);
            }

            [Fact]
            public async Task Should_Invoke_All_Handlers()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, list.Count);
                Assert.Contains(list, s => s == "HANDLER_1");
                Assert.Contains(list, s => s == "HANDLER_2");
            }

            [Fact]
            public async Task Should_Raise_The_Setup_Event_Only_Once()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");
                engine.Setup += (sender, args) =>
                {
                    list.Add("SETUP_EVENT");
                };

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskSetupEventArgs>(
                    handler => engine.TaskSetup += handler,
                    handler => engine.TaskSetup -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_PostSetup_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskSetupEventArgs>(
                    handler => engine.PostTaskSetup += handler,
                    handler => engine.PostTaskSetup -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_Setup_Event_With_Task_Context()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskSetupEventArgs>(
                    handler => engine.TaskSetup += handler,
                    handler => engine.TaskSetup -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<TaskSetupEventArgs>(result.Arguments);
                Assert.NotNull(result.Arguments.TaskSetupContext.Task);
                Assert.Equal("A", result.Arguments.TaskSetupContext.Task.Name);
            }

            [Fact]
            public async Task Should_Raise_Task_Setup_Event_After_Setup_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
            public async Task Should_Raise_Task_Setup_Event_For_All_Tasks()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.TaskSetup += (sender, args) =>
                {
                    list.Add("TASK_SETUP_EVENT_" + args.TaskSetupContext.Task.Name);
                };

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskTeardownEventArgs>(
                    handler => engine.TaskTeardown += handler,
                    handler => engine.TaskTeardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_PostTeardown_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskTeardownEventArgs>(
                    handler => engine.PostTaskTeardown += handler,
                    handler => engine.PostTaskTeardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Task_Teardown_Event_With_Task_Context()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TaskTeardownEventArgs>(
                    handler => engine.TaskTeardown += handler,
                    handler => engine.TaskTeardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.IsType<TaskTeardownEventArgs>(result.Arguments);
                Assert.NotNull(result.Arguments.TaskTeardownContext.Task);
                Assert.Equal("A", result.Arguments.TaskTeardownContext.Task.Name);
            }

            [Fact]
            public async Task Should_Raise_Task_Teardown_Event_After_Task_Setup_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TASK_SETUP_EVENT",
                        "TASK_TEARDOWN_EVENT"
                    }, list);
            }

            [Fact]
            public async Task Should_Raise_Task_Teardown_Event_For_All_Tasks()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("B");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.TaskTeardown += (sender, args) =>
                {
                    list.Add("TASK_TEARDOWN_EVENT_" + args.TaskTeardownContext.Task.Name);
                };

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TeardownEventArgs>(
                    handler => engine.Teardown += handler,
                    handler => engine.Teardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_PostTeardown_Event()
            {
                // Given
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TeardownEventArgs>(
                    handler => engine.PostTeardown += handler,
                    handler => engine.PostTeardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(engine, result.Sender);
            }

            [Fact]
            public void Should_Raise_Teardown_Event_With_Teardown_Context()
            {
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");

                // When
                var result = Assert.Raises<TeardownEventArgs>(
                    handler => engine.Teardown += handler,
                    handler => engine.Teardown -= handler,
                    async () => await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings));

                // Then
                Assert.NotNull(result);
                Assert.Equal(fixture.Context.Environment, result.Arguments.TeardownContext.Environment);
                Assert.True(result.Arguments.TeardownContext.Successful);
            }

            [Fact]
            public async Task Should_Invoke_All_Handlers()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(2, list.Count);
                Assert.Contains(list, s => s == "HANDLER_1");
                Assert.Contains(list, s => s == "HANDLER_2");
            }

            [Fact]
            public async Task Should_Raise_The_Teardown_Event_Only_Once()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("C");
                var engine = fixture.CreateEngine();
                engine.RegisterTask("A");
                engine.RegisterTask("B").IsDependentOn("A");
                engine.RegisterTask("C").IsDependentOn("B");
                engine.Teardown += (sender, args) =>
                {
                    list.Add("TEARDOWN_EVENT");
                };

                // When
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

                // Then
                Assert.Equal(
                    new List<string>
                    {
                        "TEARDOWN_EVENT"
                    }, list);
            }

            [Fact]
            public async Task Should_Raise_The_Teardown_Event_After_Task_Teardown_Event()
            {
                // Given
                var list = new List<string>();
                var fixture = new CakeEngineFixture();
                var settings = new ExecutionSettings().SetTarget("A");
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
                await engine.RunTargetAsync(fixture.Context, fixture.ExecutionStrategy, settings);

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