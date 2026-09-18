// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Cli;
using Cake.Commands;
using Cake.Core;
using Cake.Core.IO;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cake
{
    /// <summary>
    /// The main program class for Cake.
    /// </summary>
    public sealed class Program
    {
        private readonly Action<IServiceCollection> _overrides;
        private readonly bool _propagateExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="overrides">Optional service collection overrides.</param>
        /// <param name="propagateExceptions">Whether to propagate exceptions.</param>
        public Program(
            Action<IServiceCollection> overrides = null,
            bool propagateExceptions = false)
        {
            _overrides = overrides;
            _propagateExceptions = propagateExceptions;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The exit code.</returns>
        public static async Task<int> Main(string[] args)
        {
            return await new Program().Run(args);
        }

        /// <summary>
        /// Runs the program with the specified arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The exit code.</returns>
        public async Task<int> Run(string[] args)
        {
            var registrar = BuildTypeRegistrar();

            var app = new CommandApp<DefaultCommand>(registrar);
            app.Configure(config =>
            {
                config.SetApplicationName("dotnet cake");
                config.ValidateExamples();

                if (_propagateExceptions)
                {
                    config.PropagateExceptions();
                }

                // Top level examples.
                config.AddExample(Array.Empty<string>());
                config.AddExample(new[] { "build.cake", "--verbosity", "quiet" });
                config.AddExample(new[] { "build.cake", "--tree" });
            });

            return await app.RunAsync(args);
        }

        // Register everything that the CLI needs to function.
        private ITypeRegistrar BuildTypeRegistrar()
        {
            var services = new ServiceCollection();

            // Commands
            services.AddSingleton<DefaultCommandSettings>();

            // Converters
            services.AddSingleton<Cli.FilePathConverter>();
            services.AddSingleton<VerbosityConverter>();

            // Utilities
            services.AddSingleton<IContainerConfigurator, ContainerConfigurator>();
            services.AddSingleton<IVersionResolver, VersionResolver>();
            services.AddSingleton<IModuleSearcher, ModuleSearcher>();

            // Features
            services.AddSingleton<IBuildFeature, BuildFeature>();
            services.AddSingleton<IBootstrapFeature, BootstrapFeature>();
            services.AddSingleton<ICakeVersionFeature, VersionFeature>();
            services.AddSingleton<ICakeInfoFeature, InfoFeature>();

            // Core
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ICakeEnvironment, CakeEnvironment>();
            services.AddSingleton<ICakePlatform, CakePlatform>();
            services.AddSingleton<ICakeRuntime, CakeRuntime>();
            services.AddCakeDiagnostics();

            // Register custom registrations.
            _overrides?.Invoke(services);

            return new TypeRegistrar(services);
        }
    }
}
