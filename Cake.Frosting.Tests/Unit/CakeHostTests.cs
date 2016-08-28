using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting.Tests.Data.Tasks;
using Cake.Frosting.Tests.Fixtures;
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
        public void Should_Set_Working_Directory_From_Options()
        {
            // Given
            var fixture = new CakeHostBuilderFixture();
            fixture.RegisterDefaultTask();
            fixture.Options.WorkingDirectory = "./Temp";

            // When
            fixture.Run();

            // Then
            Assert.Equal("/Working/Temp", fixture.Environment.WorkingDirectory.FullPath);
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
            lifetime.Received(1).Setup(Arg.Any<ICakeContext>());
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
            lifetime.Received(1).Teardown(Arg.Any<ICakeContext>(), Arg.Any<ITeardownContext>());
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
            lifetime.Received(1).Setup(Arg.Any<ICakeContext>(), Arg.Any<ITaskSetupContext>());
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
            lifetime.Received(1).Setup(Arg.Any<ICakeContext>(), Arg.Any<ITaskSetupContext>());
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
            fixture.Strategy.Received(1).Execute(Arg.Is<CakeTask>(t => t.Name == "DummyTask"), Arg.Any<ICakeContext>());
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
                fixture.Strategy.Execute(Arg.Is<CakeTask>(t => t.Name == "Clean"), Arg.Any<ICakeContext>());
                fixture.Strategy.Execute(Arg.Is<CakeTask>(t => t.Name == "Build"), Arg.Any<ICakeContext>());
                fixture.Strategy.Execute(Arg.Is<CakeTask>(t => t.Name == "Run-Unit-Tests"), Arg.Any<ICakeContext>());
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
                "Error: {0}", "The dependency DateTime does not implement IFrostingTask.");
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
    }
}
