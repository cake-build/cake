using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Features.Building;
using Cake.Tests.Fixtures;
using NSubstitute;
using Spectre.Console.Cli;
using Xunit;

namespace Cake.Tests.Unit
{
    public class ProgramTests
    {
        [Fact]
        public async Task Should_Use_Default_Parameters_By_Default()
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<IBuildFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run();

            // Then
            feature.Received(1).Run(
                Arg.Any<IRemainingArguments>(),
                Arg.Is<BuildFeatureSettings>(settings =>
                    settings.BuildHostKind == BuildHostKind.Build &&
                    settings.Debug == false &&
                    settings.Exclusive == false &&
                    settings.Script.FullPath == "build.cake" &&
                    settings.Verbosity == Verbosity.Normal &&
                    settings.NoBootstrapping == false));
        }

        [Theory]
        [InlineData("--dryrun")]
        [InlineData("--whatif")]
        [InlineData("--noop")]
        public async Task The_DryRun_Option_Should_Perform_A_Dry_Run_Of_Script(params string[] args)
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<IBuildFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run(args);

            // Then
            feature.Received(1).Run(
                Arg.Any<IRemainingArguments>(),
                Arg.Is<BuildFeatureSettings>(settings =>
                    settings.BuildHostKind == BuildHostKind.DryRun));
        }

        [Theory]
        [InlineData("--showtree")]
        [InlineData("--tree")]
        public async Task The_Tree_Option_Should_Show_The_Script_Tree(params string[] args)
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<IBuildFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run(args);

            // Then
            feature.Received(1).Run(
                Arg.Any<IRemainingArguments>(),
                Arg.Is<BuildFeatureSettings>(settings =>
                    settings.BuildHostKind == BuildHostKind.Tree));
        }

        [Theory]
        [InlineData("--showdescription")]
        [InlineData("--description")]
        public async Task The_Description_Option_Should_Show_Script_Descriptions(params string[] args)
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<IBuildFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run(args);

            // Then
            feature.Received(1).Run(
                Arg.Any<IRemainingArguments>(),
                Arg.Is<BuildFeatureSettings>(settings =>
                    settings.BuildHostKind == BuildHostKind.Description));
        }

        [Theory]
        [InlineData("--version")]
        [InlineData("--ver")]
        public async Task The_Version_Option_Should_Call_Version_Feature(params string[] args)
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<ICakeVersionFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run(args);

            // Then
            feature.Received(1).Run(fixture.Console);
        }

        [Theory]
        [InlineData("--info")]
        public async Task The_Info_Option_Should_Call_Info_Feature(params string[] args)
        {
            // Given
            var fixture = new ProgramFixture();
            var feature = Substitute.For<ICakeInfoFeature>();
            fixture.Overrides.Add(builder => builder.RegisterInstance(feature));

            // When
            var result = await fixture.Run(args);

            // Then
            feature.Received(1).Run(fixture.Console);
        }
    }
}
