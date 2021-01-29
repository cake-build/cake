// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Autofac;
using Cake.Cli;
using Cake.Commands;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Spectre.Console.Cli;

namespace Cake
{
    public sealed class Program
    {
        private readonly Action<ContainerBuilder> _overrides;
        private readonly bool _propagateExceptions;

        public Program(
            Action<ContainerBuilder> overrides = null,
            bool propagateExceptions = false)
        {
            _overrides = overrides;
            _propagateExceptions = propagateExceptions;
        }

        public static async Task<int> Main(string[] args)
        {
            return await new Program().Run(args);
        }

        public async Task<int> Run(string[] args)
        {
            var registrar = BuildTypeRegistrar();

            var app = new CommandApp<DefaultCommand>(registrar);
            app.Configure(config =>
            {
#pragma warning disable SA1114 // Parameter list should follow declaration
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
#pragma warning disable SA1111 // Closing parenthesis should be on line of last parameter
                config.SetApplicationName(
#if NETCOREAPP2_0

                    "dotnet Cake.dll"
#elif NETFRAMEWORK
                    "Cake.exe"
#else
                    "dotnet cake"
#endif
                );
#pragma warning restore SA1111 // Closing parenthesis should be on line of last parameter
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
#pragma warning restore SA1114 // Parameter list should follow declaration
                config.ValidateExamples();

                if (_propagateExceptions)
                {
                    config.PropagateExceptions();
                }

                // Top level examples.
                config.AddExample(new[] { string.Empty });
                config.AddExample(new[] { "build.cake", "--verbosity", "quiet" });
                config.AddExample(new[] { "build.cake", "--showtree" });
            });

            return await app.RunAsync(args);
        }

        // Register everything that the CLI needs to function.
        private ITypeRegistrar BuildTypeRegistrar()
        {
            var builder = new ContainerBuilder();

            // Converters
            builder.RegisterType<FilePathConverter>();
            builder.RegisterType<VerbosityConverter>();

            // Utilities
            builder.RegisterType<ContainerConfigurator>().As<IContainerConfigurator>().SingleInstance();
            builder.RegisterType<VersionResolver>().As<IVersionResolver>().SingleInstance();
            builder.RegisterType<ModuleSearcher>().As<IModuleSearcher>().SingleInstance();

            // Features
            builder.RegisterType<BuildFeature>().As<IBuildFeature>().SingleInstance();
            builder.RegisterType<BootstrapFeature>().As<IBootstrapFeature>().SingleInstance();
            builder.RegisterType<VersionFeature>().As<ICakeVersionFeature>().SingleInstance();
            builder.RegisterType<InfoFeature>().As<ICakeInfoFeature>().SingleInstance();

            // Core
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().SingleInstance();
            builder.RegisterType<CakePlatform>().As<ICakePlatform>().SingleInstance();
            builder.RegisterType<CakeRuntime>().As<ICakeRuntime>().SingleInstance();
            builder.RegisterType<CakeBuildLog>().As<ICakeLog>().SingleInstance();
            builder.RegisterType<CakeConsole>().As<IConsole>().SingleInstance();

            // Register custom registrations.
            _overrides?.Invoke(builder);

            return new AutofacTypeRegistrar(builder);
        }
    }
}
