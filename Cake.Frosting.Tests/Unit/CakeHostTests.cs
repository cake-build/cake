// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Packaging;
using Cake.Frosting.Testing;
using Cake.Frosting.Tests.Data.Tasks;
using Cake.Frosting.Tests.Fakes;
using Cake.Frosting.Tests.Fixtures;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Frosting.Tests.Unit
{
    public sealed class CakeHostTests
    {
        [Fact]
        public void Should_Set_Log_Verbosity_From_Options()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.Options.Verbosity = Verbosity.Diagnostic;

            // When
            fixture.Run();

            // Then
            Assert.Equal(Verbosity.Diagnostic, fixture.Log.Verbosity);
        }

        [Fact]
        public void Should_Set_Working_Directory_From_Options_If_Set()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.FileSystem.CreateDirectory("/Working/Foo");
            fixture.Options.WorkingDirectory = "./Foo";

            // When
            fixture.Run();

            // Then
            Assert.Equal("/Working/Foo", fixture.Environment.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Use_Working_Directory_From_Service_Configuration_If_Set()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.FileSystem.CreateDirectory("/Working/Foo");
            fixture.Builder.ConfigureServices(s => s.UseWorkingDirectory("./Foo"));

            // When
            fixture.Run();

            // Then
            Assert.Equal("/Working/Foo", fixture.Environment.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Prefer_Working_Directory_From_Options_Over_Configuration()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.FileSystem.CreateDirectory("/Working/Foo");
            fixture.FileSystem.CreateDirectory("/Working/Bar");
            fixture.Options.WorkingDirectory = "./Bar";
            fixture.Builder.ConfigureServices(s => s.UseWorkingDirectory("./Foo"));

            // When
            fixture.Run();

            // Then
            Assert.Equal("/Working/Bar", fixture.Environment.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Register_Tasks_With_Engine()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();

            // When
            fixture.Run();

            // Then
            Assert.True(fixture.Engine.IsTaskRegistered("DummyTask"));
        }

        [Fact]
        public void Should_Call_Setup_On_Registered_Lifetime()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            var lifetime = fixture.RegisterDefaultTask()
                .RegisterLifetimeSubstitute();

            // When
            fixture.Run();

            // Then
            Assert.True(lifetime.CalledSetup);
        }

        [Fact]
        public void Should_Not_Call_Setup_On_Registered_Lifetime_If_Not_Overridden()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.RegisterLifetimeSubstitute(new FakeLifetime.WithoutOverrides());
            fixture.UseExecutionStrategySubstitute();

            // When
            fixture.Run();

            // Then
            fixture.Strategy.Received(0).PerformSetup(
                Arg.Any<Action<ICakeContext>>(),
                Arg.Any<ISetupContext>());
        }

        [Fact]
        public void Should_Call_Teardown_On_Registered_Lifetime()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            var lifetime = fixture.RegisterDefaultTask()
                .RegisterLifetimeSubstitute();

            // When
            fixture.Run();

            // Then
            Assert.True(lifetime.CalledTeardown);
        }

        [Fact]
        public void Should_Not_Call_Teardown_On_Registered_Lifetime_If_Not_Overridden()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.RegisterLifetimeSubstitute(new FakeLifetime.WithoutOverrides());
            fixture.UseExecutionStrategySubstitute();

            // When
            fixture.Run();

            // Then
            fixture.Strategy.Received(0).PerformTeardown(
                Arg.Any<Action<ITeardownContext>>(),
                Arg.Any<ITeardownContext>());
        }

        [Fact]
        public void Should_Call_Setup_On_Registered_Task_Lifetime()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            var lifetime = fixture.RegisterDefaultTask()
                .RegisterTaskLifetimeSubstitute();

            // When
            fixture.Run();

            // Then
            Assert.True(lifetime.CalledSetup);
        }

        [Fact]
        public void Should_Not_Call_Setup_On_Registered_Task_Lifetime_If_Not_Overridden()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.RegisterTaskLifetimeSubstitute(new FakeTaskLifetime.WithoutOverrides());
            fixture.UseExecutionStrategySubstitute();

            // When
            fixture.Run();

            // Then
            fixture.Strategy.Received(0).PerformTaskSetup(
                Arg.Any<Action<ITaskSetupContext>>(),
                Arg.Any<ITaskSetupContext>());
        }

        [Fact]
        public void Should_Call_Teardown_On_Registered_Task_Lifetime()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            var lifetime = fixture.RegisterDefaultTask()
                .RegisterTaskLifetimeSubstitute();

            // When
            fixture.Run();

            // Then
            Assert.True(lifetime.CalledTeardown);
        }

        [Fact]
        public void Should_Not_Call_Teardown_On_Registered_Task_Lifetime_If_Not_Overridden()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.RegisterTaskLifetimeSubstitute(new FakeTaskLifetime.WithoutOverrides());
            fixture.UseExecutionStrategySubstitute();

            // When
            fixture.Run();

            // Then
            fixture.Strategy.Received(0).PerformTaskTeardown(
                Arg.Any<Action<ITaskTeardownContext>>(),
                Arg.Any<ITaskTeardownContext>());
        }

        [Fact]
        public void Should_Execute_Tasks()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.UseExecutionStrategySubstitute();

            // When
            fixture.Run();

            // Then
            fixture.Strategy.Received(1).ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "DummyTask"), Arg.Any<ICakeContext>());
        }

        [Fact]
        public void Should_Not_Abort_Build_If_Task_That_Is_ContinueOnError_Throws()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<ContinueOnErrorTask>();
            fixture.Options.Target = "Continue-On-Error";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Abort_Build_If_Task_That_Is_Not_ContinueOnError_Throws()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<NoContinueOnErrorTask>();
            fixture.Options.Target = "No-Continue-On-Error";

            // When
            var result = fixture.Run();

            // Then
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Execute_Tasks_In_Correct_Order()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<BuildTask>();
            fixture.RegisterTask<CleanTask>();
            fixture.RegisterTask<UnitTestsTask>();
            fixture.UseExecutionStrategySubstitute();
            fixture.Options.Target = "Run-Unit-Tests";

            // When
            fixture.Run();

            // Then
            Received.InOrder(() =>
            {
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "Clean"), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "Build"), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "Run-Unit-Tests"), Arg.Any<ICakeContext>());
            });
        }

        [Fact]
        public void Should_Throw_If_Dependency_Is_Not_A_Valid_Task()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<InvalidDependencyTask>();
            fixture.UseExecutionStrategySubstitute();
            fixture.Options.Target = "InvalidDependencyTask";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(1, result);
            fixture.Log.Received(1).Write(
                Verbosity.Quiet, LogLevel.Error,
                "Error: {0}", "The dependency 'DateTime' is not a valid task.");
        }

        [Fact]
        public void Should_Return_Zero_On_Success()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Execute_OnError_Method_If_Run_Failed()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<OnErrorRunFailedTask>();
            fixture.Options.Target = "On-Error-Run-Failed";

            // When
            fixture.Run();

            // Then
            fixture.Log.Received(1).Error("An error has occurred. {0}", "On test exception");
        }

        [Fact]
        public void Should_Execute_OnError_Method_If_RunAsync_Failed()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<OnErrorRunAsyncFailedTask>();
            fixture.Options.Target = "On-Error-RunAsync-Failed";

            // When
            fixture.Run();

            // Then
            fixture.Log.Received(1).Error("An error has occurred. {0}", "On test exception");
        }

        [Fact]
        public void Should_Not_Execute_OnError_Method_If_Run_Completed()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<OnErrorRunCompletedTask>();
            fixture.Options.Target = "On-Error-Run-Completed";

            // When
            fixture.Run();

            // Then
            fixture.Log.DidNotReceive().Error("OnErrorRunCompletedTask Exception");
        }

        [Fact]
        public void Should_Execute_Finally_Method_After_All_Methods()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterTask<FinallyTask>();
            fixture.Options.Target = "Finally";

            // When
            fixture.Run();

            // Then
            Received.InOrder(() =>
            {
                fixture.Log.Information("Run method called");
                fixture.Log.Information("OnError method called");
                fixture.Log.Information("Finally method called");
            });
        }

        [Fact]
        public void Should_Install_Tools()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.Builder.ConfigureServices(s => s.UseTool(new Uri("foo:?package=Bar")));
            fixture.RegisterDefaultTask();

            // When
            fixture.Run();

            // Then
            fixture.Installer.Received(1).Install(Arg.Is<PackageReference>(
                p => p.OriginalString == "foo:?package=Bar"));
        }
    }
}
