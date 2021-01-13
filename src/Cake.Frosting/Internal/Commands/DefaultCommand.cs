// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cake.Frosting.Internal
{
    internal sealed class DefaultCommand : Command<DefaultCommandSettings>
    {
        private readonly IServiceCollection _services;

        public DefaultCommand(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public override int Execute(CommandContext context, DefaultCommandSettings settings)
        {
            // Register arguments
            var arguments = new CakeArguments(context.Remaining.Parsed);
            _services.AddSingleton<ICakeArguments>(arguments);

            var provider = _services.BuildServiceProvider();

            try
            {
                if (settings.Version)
                {
                    // Show version
                    var console = provider.GetRequiredService<IConsole>();
                    provider.GetRequiredService<VersionFeature>().Run(console);
                    return 0;
                }
                else if (settings.Info)
                {
                    // Show information
                    var console = provider.GetRequiredService<IConsole>();
                    provider.GetRequiredService<InfoFeature>().Run(console);
                    return 0;
                }

                // Install tools
                InstallTools(provider);

                // Run
                var runner = GetFrostingEngine(provider, settings);

                // Set the working directory
                SetWorkingDirectory(provider, settings);

                if (settings.Exclusive)
                {
                    runner.Settings.UseExclusiveTarget();
                }

                runner.Run(settings.Target, settings.Verbosity);
            }
            catch (Exception ex)
            {
                LogException(provider.GetService<ICakeLog>(), ex);
                return -1;
            }

            return 0;
        }

        private static int LogException<T>(ICakeLog log, T ex)
            where T : Exception
        {
            log = log ?? new CakeBuildLog(
                new CakeConsole(new CakeEnvironment(new CakePlatform(), new CakeRuntime())));

            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", ex);
            }
            else
            {
                log.Error("Error: {0}", ex.Message);
                if (ex is AggregateException aex)
                {
                    foreach (var exception in aex.Flatten().InnerExceptions)
                    {
                        log.Error("\t{0}", exception.Message);
                    }
                }
            }

            return 1;
        }

        private void InstallTools(ServiceProvider provider)
        {
            var installer = provider.GetRequiredService<IToolInstaller>();
            var tools = provider.GetServices<PackageReference>();
            var log = provider.GetService<ICakeLog>();

            // Install tools.
            if (tools.Any())
            {
                log.Verbose("Installing tools...");
                foreach (var tool in tools)
                {
                    installer.Install(tool);
                }
            }
        }

        private void SetWorkingDirectory(ServiceProvider provider, DefaultCommandSettings settings)
        {
            var fileSystem = provider.GetRequiredService<IFileSystem>();
            var environment = provider.GetRequiredService<ICakeEnvironment>();

            var directory = settings.WorkingDirectory ?? provider.GetService<WorkingDirectory>()?.Path;
            directory = directory?.MakeAbsolute(environment) ?? environment.WorkingDirectory;

            if (!fileSystem.Exist(directory))
            {
                throw new FrostingException($"The working directory '{directory.FullPath}' does not exist.");
            }

            environment.WorkingDirectory = directory;
        }

        private IFrostingEngine GetFrostingEngine(ServiceProvider provider, DefaultCommandSettings settings)
        {
            if (settings.DryRun)
            {
                return provider.GetRequiredService<FrostingDryRunner>();
            }
            else if (settings.Tree)
            {
                return provider.GetRequiredService<FrostingTreeRunner>();
            }
            else if (settings.Description)
            {
                return provider.GetRequiredService<FrostingDescriptionRunner>();
            }
            else
            {
                return provider.GetRequiredService<FrostingRunner>();
            }
        }
    }
}
