// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Autofac.Core;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Packaging;
using Cake.Frosting.Tests.Fakes;
using Cake.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace Cake.Frosting.Tests
{
    public sealed partial class CakeHostTests
    {
        [Fact]
        public void Should_Set_Working_Directory_From_Options_If_Set()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.Host.UseWorkingDirectory("./Foo");
            fixture.FileSystem.CreateDirectory("/Working/Foo");

            // When
            fixture.Run();

            // Then
            Assert.Equal("/Working/Foo", fixture.Environment.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Prefer_Working_Directory_From_Options_Over_Configuration()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.FileSystem.CreateDirectory("/Working/Foo");
            fixture.FileSystem.CreateDirectory("/Working/Bar");
            fixture.Host.UseWorkingDirectory("./Foo");

            // When
            fixture.Run("-w", "./Bar");

            // Then
            Assert.Equal("/Working/Bar", fixture.Environment.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Call_Setup_On_Registered_Setup_Lifetime()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();

            var lifetime = new FakeLifetime();
            fixture.Host.ConfigureServices(services => services.AddSingleton<IFrostingSetup>(lifetime));

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            Assert.Equal(0, result);
            Assert.Equal(1, lifetime.SetupCount);
        }

        [Fact]
        public void Should_Call_Teardown_On_Registered_Teardown_Lifetime()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();

            var lifetime = new FakeLifetime();
            fixture.Host.ConfigureServices(services => services.AddSingleton<IFrostingTeardown>(lifetime));

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            Assert.Equal(0, result);
            Assert.Equal(1, lifetime.TeardownCount);
        }

        [Fact]
        public void Should_Call_Setup_On_Registered_Task_Lifetime()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();

            var lifetime = new FakeTaskLifetime();
            fixture.Host.ConfigureServices(services => services.AddSingleton<IFrostingTaskSetup>(lifetime));

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            Assert.Equal(0, result);
            Assert.Equal(1, lifetime.SetupCount);
        }

        [Fact]
        public void Should_Call_Teardown_On_Registered_Task_Lifetime()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();

            var lifetime = new FakeTaskLifetime();
            fixture.Host.ConfigureServices(services => services.AddSingleton<IFrostingTaskTeardown>(lifetime));

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            Assert.Equal(0, result);
            Assert.Equal(1, lifetime.TeardownCount);
        }

        [Fact]
        public void Should_Execute_Tasks()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();
            fixture.RegisterTask<DummyTask>();

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            fixture.Strategy
                .Received(1)
                .ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "DummyTask"), Arg.Any<ICakeContext>());
        }

        [Fact]
        public void Should_Not_Abort_Build_If_Task_That_Is_ContinueOnError_Throws()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<ContinueOnErrorTask>();

            // When
            var result = fixture.Run("--target", "ContinueOnErrorTask");

            // Then
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Abort_Build_If_Task_That_Is_Not_ContinueOnError_Throws()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<ThrowingTask>();

            // When
            var result = fixture.Run("--target", "ThrowingTask");

            // Then
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Execute_Dependee_Task()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<CleanTask>();
            fixture.RegisterTask<DependeeTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            fixture.Run("--target", nameof(CleanTask));

            // Then
            Received.InOrder(() =>
            {
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == nameof(DependeeTask)), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == nameof(CleanTask)), Arg.Any<ICakeContext>());
            });
        }

        [Fact]
        public void Should_Execute_Tasks_In_Correct_Order()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<BuildTask>();
            fixture.RegisterTask<CleanTask>();
            fixture.RegisterTask<UnitTestsTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            fixture.Run("--target", "UnitTestsTask");

            // Then
            Received.InOrder(() =>
            {
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "CleanTask"), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "BuildTask"), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == "UnitTestsTask"), Arg.Any<ICakeContext>());
            });
        }

        [Fact]
        public void Should_Throw_If_Dependency_Is_Not_A_Valid_Task()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<InvalidDependencyTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            var result = fixture.Run("--target", "InvalidDependencyTask");

            // Then
            Assert.NotEqual(0, result);
            fixture.Log.Received(1).Error("Error: {0}", "The dependency 'DateTime' is not a valid task.");
        }

        [Fact]
        public void Should_Return_Zero_On_Success()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Execute_OnError_Method_If_Run_Failed()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<OnErrorRunFailedTask>();

            // When
            fixture.Run("--target", "OnErrorRunFailedTask");

            // Then
            fixture.Log.Received(1).Error("OnError: {0}", "An exception");
        }

        [Fact]
        public void Should_Execute_OnError_Method_If_RunAsync_Failed()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<OnErrorRunAsyncFailedTask>();

            // When
            fixture.Run("--target", "OnErrorRunAsyncFailedTask");

            // Then
            fixture.Log.Received(1).Error("OnError: {0}", "An exception");
        }

        [Fact]
        public void Should_Not_Execute_OnError_Method_If_Run_Completed()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<OnErrorRunCompletedTask>();

            // When
            fixture.Run("--target", "OnErrorRunCompletedTask");

            // Then
            fixture.Log.DidNotReceive().Error("OnError: {0}", "An exception");
        }

        [Fact]
        public void Should_Execute_Finally_Method_After_All_Methods()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<FinallyTask>();

            // When
            fixture.Run("--target", "FinallyTask");

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
            var fixture = new CakeHostFixture();
            fixture.Host.ConfigureServices(s => s.UseTool(new Uri("foo:?package=Bar")));
            fixture.RegisterTask<DummyTask>();

            // When
            var result = fixture.Run("--target", "dummytask");

            // Then
            fixture.Installer.Received(1).Install(
                Arg.Is<PackageReference>(p => p.OriginalString == "foo:?package=Bar"));
        }

        [Fact]
        public void Should_Pass_Target_Within_CakeContext_Arguments()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            fixture.Run("--target", nameof(DummyTask));

            // Then
            fixture.Strategy
                .Received(1)
                .ExecuteAsync(Arg.Any<CakeTask>(), Arg.Is<ICakeContext>(cc => cc.Arguments.HasArgument("target") && cc.Arguments.GetArgument("target").Equals(nameof(DummyTask))));
        }

        [Theory]
        [InlineData(nameof(DummyTask), nameof(DummyTask2), nameof(DummyTask3))]
        [InlineData(nameof(DummyTask), nameof(DummyTask3), nameof(DummyTask2))]
        [InlineData(nameof(DummyTask2), nameof(DummyTask3), nameof(DummyTask))]
        [InlineData(nameof(DummyTask2), nameof(DummyTask), nameof(DummyTask3))]
        [InlineData(nameof(DummyTask3), nameof(DummyTask2), nameof(DummyTask))]
        [InlineData(nameof(DummyTask3), nameof(DummyTask), nameof(DummyTask2))]
        public void Should_Execute_Multiple_Targets_In_Correct_Order(string task0, string task1, string task2)
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.RegisterTask<DummyTask2>();
            fixture.RegisterTask<DummyTask3>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            fixture.Run("--target", task0, "--target", task1, "--target", task2);

            // Then
            Received.InOrder(() =>
            {
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == task0), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == task1), Arg.Any<ICakeContext>());
                fixture.Strategy.ExecuteAsync(Arg.Is<CakeTask>(t => t.Name == task2), Arg.Any<ICakeContext>());
            });
        }

        [Fact]
        public void The_Version_Option_Should_Call_Version_Feature()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // Replace the Cake.Cli.VersionResolver that CakeHost adds to its service collection
            // with a fake so that the call to VersionFeature can be asserted below
            fixture.Host.ConfigureServices(services => services.Remove(services.Single(sd => sd.ServiceType == typeof(IVersionResolver))));
            fixture.Host.ConfigureServices(services => services.AddSingleton<IVersionResolver, FakeVersionResolver>());

            // When
            fixture.Run("--version");

            // Then
            Assert.Collection(fixture.Console.Messages, s => s.Equals("FakeVersion"));
        }

        [Fact(Skip = "Excluded to see if the performance implication of this test is causing the integration tests to break on push to PR")]
        public void Should_Pass_Cake_Runner_Argument_And_Value_To_Build_Script()
        {
            // Given
            var fixture = new CakeHostFixture();
            fixture.RegisterTask<DummyTask>();
            fixture.Strategy = Substitute.For<IExecutionStrategy>();

            // When
            fixture.Run("--target", "dummytask", "--version=1.2.3");

            // Then
            fixture.Strategy.Received(1).ExecuteAsync(
                Arg.Any<CakeTask>(),
                Arg.Is<ICakeContext>(cc => cc.Arguments.HasArgument("version") && cc.Arguments.GetArgument("version") == "1.2.3"));
        }
    }
}
