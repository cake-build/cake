// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
            var arguments = CreateCakeArguments(context.Remaining, settings);
            _services.AddSingleton<ICakeArguments>(arguments);
            _services.AddSingleton(context.Remaining);

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

                // Set the log verbosity
                var log = provider.GetRequiredService<ICakeLog>();
                log.Verbosity = settings.Verbosity;

                // Set the working directory
                SetWorkingDirectory(provider, settings);

                // Run
                var runner = GetFrostingEngine(provider, settings);

                // Install tools
                InstallTools(provider);

                if (settings.Exclusive)
                {
                    runner.Settings.UseExclusiveTarget();
                }

                runner.Run(settings.Target);
            }
            catch (Exception ex)
            {
                provider.GetService<ICakeLog>().LogException(ex);
                return -1;
            }

            return 0;
        }

        private static CakeArguments CreateCakeArguments(IRemainingArguments remainingArguments, DefaultCommandSettings settings)
        {
            var arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            // Keep the actual remaining arguments in the cake arguments
            foreach (var group in remainingArguments.Parsed)
            {
                arguments[group.Key] = new List<string>();
                foreach (var argument in group)
                {
                    arguments[group.Key].Add(argument);
                }
            }

            // Fixes #3291, We have to add arguments manually which are defined within the DefaultCommandSettings type. Those are not considered "as remaining" because they could be parsed
            const string targetArgumentName = "target";
            if (!arguments.ContainsKey(targetArgumentName))
            {
                arguments[targetArgumentName] = new List<string>();
            }
            arguments[targetArgumentName].Add(settings.Target);

            var argumentLookUp = arguments.SelectMany(a => a.Value, Tuple.Create).ToLookup(a => a.Item1.Key, a => a.Item2);
            return new CakeArguments(argumentLookUp);
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
