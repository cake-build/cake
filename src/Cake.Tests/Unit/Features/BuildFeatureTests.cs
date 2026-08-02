using System;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Features.Building;
using Cake.Tests.Fixtures;
using Xunit;

namespace Cake.Tests.Unit.Features
{
    public sealed partial class BuildFeatureTests
    {
        [Fact]
        public void Should_Execute_Default_Script_If_Not_Specified()
        {
            // Given
            var fixture = new BuildFeatureFixture();

            // When
            var result = fixture.Run(new BuildFeatureSettings(BuildHostKind.Build));

            // Then
            Assert.Equal(0, result.ExitCode);
            Assert.NotNull(result.ExecutedScript);
        }

        [Fact]
        public void Should_Attach_Debugger_If_Specified()
        {
            // Given
            var fixture = new BuildFeatureFixture();

            // When
            var result = fixture.Run(new BuildFeatureSettings(BuildHostKind.Build)
            {
                Debug = true
            });

            // Then
            Assert.True(result.AttachedDebugger);
        }

        [Theory]
        [InlineData(BuildHostKind.Build, typeof(BuildScriptHost))]
        [InlineData(BuildHostKind.DryRun, typeof(DryRunScriptHost))]
        [InlineData(BuildHostKind.Description, typeof(DescriptionScriptHost))]
        [InlineData(BuildHostKind.Tree, typeof(TreeScriptHost))]
        public void Should_Resolve_Correct_Script_Host_For_The_Different_Build_Host_Kinds(BuildHostKind kind, Type expected)
        {
            // Given
            var fixture = new BuildFeatureFixture();

            // When
            var result = fixture.Run(new BuildFeatureSettings(kind));

            // Then
            Assert.NotNull(fixture.ScriptEngine.ScriptHost);
            Assert.IsType(expected, fixture.ScriptEngine.ScriptHost);
        }

        [Fact]
        public void Should_Use_Verbosity_From_Environment_When_Not_Specified()
        {
            // Given
            var fixture = new BuildFeatureFixture();
            fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", "Diagnostic");

            // When
            fixture.Run(new BuildFeatureSettings(BuildHostKind.Build));

            // Then
            Assert.Equal(Verbosity.Diagnostic, fixture.Log.Verbosity);
            Assert.Equal(Verbosity.Diagnostic, fixture.ScopedLog.Verbosity);
        }

        [Fact]
        public void Should_Prefer_Command_Line_Verbosity_Over_Environment()
        {
            // Given
            var fixture = new BuildFeatureFixture();
            fixture.Environment.SetEnvironmentVariable("CAKE_SETTINGS_VERBOSITY", "Diagnostic");

            // When
            fixture.Run(new BuildFeatureSettings(BuildHostKind.Build)
            {
                Verbosity = Verbosity.Quiet
            });

            // Then
            Assert.Equal(Verbosity.Quiet, fixture.Log.Verbosity);
            Assert.Equal(Verbosity.Quiet, fixture.ScopedLog.Verbosity);
        }
    }
}
