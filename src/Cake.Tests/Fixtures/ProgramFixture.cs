using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Features.Building;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Cake.Testing;
using Cake.Tests.Fakes;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class ProgramFixture
    {
        public List<Action<ContainerBuilder>> Overrides { get; }

        public FakeFileSystem FileSystem { get; }
        public FakeEnvironment Environment { get; }
        public TestContainerConfigurator Bootstrapper { get; }
        public FakeLog Log { get; }
        public FakeConsole Console { get; }
        public IModuleSearcher ModuleSearcher { get; }
        public BuildFeatureFixture Builder { get; }

        public ProgramFixture()
        {
            Bootstrapper = new TestContainerConfigurator();
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Log = new FakeLog();
            Console = new FakeConsole();
            ModuleSearcher = Substitute.For<IModuleSearcher>();
            Builder = new BuildFeatureFixture(FileSystem, Environment, Bootstrapper, Log, Console, ModuleSearcher);

            // CLI overrides
            Overrides = new List<Action<ContainerBuilder>>()
            {
                builder => builder.RegisterInstance(Bootstrapper).As<IContainerConfigurator>(),
                builder => builder.RegisterInstance(Environment).As<ICakeEnvironment>(),
                builder => builder.RegisterInstance(FileSystem).As<IFileSystem>(),
                builder => builder.RegisterInstance(Log).As<ICakeLog>(),
                builder => builder.RegisterInstance(Console).As<IConsole>(),
                builder => builder.RegisterInstance(ModuleSearcher).As<IModuleSearcher>(),
                builder => builder.RegisterInstance(Builder).As<IBuildFeature>()
            };
        }

        public async Task<ProgramFixtureResult> Run(params string[] args)
        {
            // Create the application and override registrations.
            var application = new Program(
                builder => Overrides.ForEach(action => action(builder)),
                propagateExceptions: true);

            // Execute the application with the provided arguments.
            var exitCode = await application.Run(args);
            return new ProgramFixtureResult
            {
                ExitCode = exitCode
            };
        }
    }
}
