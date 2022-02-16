// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using Autofac;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Cake.Infrastructure.Scripting;
using Spectre.Console.Cli;

namespace Cake.Features.Building
{
    public interface IBuildFeature
    {
        int Run(IRemainingArguments arguments, BuildFeatureSettings settings);
    }

    public sealed class BuildFeature : Feature, IBuildFeature
    {
        private readonly ICakeEnvironment _environment;
        private readonly IModuleSearcher _searcher;
        private readonly ICakeLog _log;

        public BuildFeature(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IContainerConfigurator configurator,
            IModuleSearcher searcher,
            ICakeLog log) : base(fileSystem, environment, configurator)
        {
            _environment = environment;
            _searcher = searcher;
            _log = log;
        }

        public int Run(IRemainingArguments arguments, BuildFeatureSettings settings)
        {
            using (new ScriptAssemblyResolver(_environment, _log))
            {
                return RunCore(arguments, settings);
            }
        }

        private int RunCore(IRemainingArguments arguments, BuildFeatureSettings settings)
        {
            // Fix the script path.
            settings.Script = settings.Script ?? new FilePath("build.cake");
            settings.Script = settings.Script.MakeAbsolute(_environment);

            // Read the configuration.
            var configuration = ReadConfiguration(arguments, settings.Script.GetDirectory());

            // Define the callback for modifying the scope.
            void ModifyScope(ICakeContainerRegistrar registrar)
            {
                LoadModules(registrar);
                registrar.RegisterInstance(settings).As<IScriptHostSettings>();
            }

            // Define a local method for loading modules into a registrar.
            void LoadModules(ICakeContainerRegistrar registrar)
            {
                var root = settings.Script.GetDirectory();
                var moduleTypes = _searcher.FindModuleTypes(root, configuration).ToArray();
                if (moduleTypes.Length > 0)
                {
                    using (var scope = CreateScope(configuration, arguments))
                    {
                        var loader = new ModuleLoader(scope);
                        var modules = loader.LoadModules(moduleTypes);

                        foreach (var module in modules)
                        {
                            module.Register(registrar);
                        }
                    }
                }
            }

            // Create the scope where we're going to execute the script.
            using (var scope = CreateScope(configuration, arguments, ModifyScope))
            {
                var runner = scope.Resolve<IScriptRunner>();

                // Set log verbosity for log in new scope.
                var log = scope.Resolve<ICakeLog>();
                log.Verbosity = settings.Verbosity;

                // Create the script host.
                var host = CreateScriptHost(settings, scope);
                if (settings.Exclusive)
                {
                    host.Settings.UseExclusiveTarget();
                }

                // Debug?
                if (settings.Debug)
                {
                    var debugger = scope.Resolve<ICakeDebugger>();
                    debugger.WaitForAttach(Timeout.InfiniteTimeSpan);
                }

                runner.Run(host, settings.Script);
            }

            return 0;
        }

        private ScriptHost CreateScriptHost(BuildFeatureSettings settings, IContainer scope)
        {
            switch (settings.BuildHostKind)
            {
                case BuildHostKind.Build:
                    return scope.Resolve<BuildScriptHost>();
                case BuildHostKind.DryRun:
                    return scope.Resolve<DryRunScriptHost>();
                case BuildHostKind.Tree:
                    return scope.Resolve<TreeScriptHost>();
                case BuildHostKind.Description:
                    return scope.Resolve<DescriptionScriptHost>();
            }

            throw new NotSupportedException($"Specified script host not supported.");
        }
    }
}
