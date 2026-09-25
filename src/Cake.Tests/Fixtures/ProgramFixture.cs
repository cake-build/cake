using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Features.Building;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Cake.Testing;
using Cake.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Cake.Tests.Fixtures;

public sealed class ProgramFixture
{
    public List<Action<IServiceCollection>> Overrides { get; }

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
        Overrides = new List<Action<IServiceCollection>>()
        {
            services => services.AddSingleton<IContainerConfigurator>(Bootstrapper),
            services => services.AddSingleton<ICakeEnvironment>(Environment),
            services => services.AddSingleton<IFileSystem>(FileSystem),
            services => services.AddSingleton<ICakeLog>(Log),
            services => services.AddSingleton<IConsole>(Console),
            services => services.AddSingleton(ModuleSearcher),
            services => services.AddSingleton<IBuildFeature>(Builder)
        };
    }

    public async Task<ProgramFixtureResult> Run(params string[] args)
    {
        // Create the application and override registrations.
        var application = new Program(
            services => Overrides.ForEach(action => action(services)),
            propagateExceptions: true);

        // Execute the application with the provided arguments.
        var exitCode = await application.Run(args);
        return new ProgramFixtureResult
        {
            ExitCode = exitCode
        };
    }
}
