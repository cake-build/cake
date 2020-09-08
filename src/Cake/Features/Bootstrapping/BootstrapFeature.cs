// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Autofac;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Infrastructure;
using Spectre.Cli;

namespace Cake.Features.Bootstrapping
{
    public interface IBootstrapFeature
    {
        int Run(IRemainingArguments arguments, BootstrapFeatureSettings settings);
    }

    public sealed class BootstrapFeature : Feature, IBootstrapFeature
    {
        private readonly ICakeEnvironment _environment;

        public BootstrapFeature(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IContainerConfigurator configurator) : base(fileSystem, environment, configurator)
        {
            _environment = environment;
        }

        public int Run(IRemainingArguments arguments, BootstrapFeatureSettings settings)
        {
            // Fix the script path.
            settings.Script = settings.Script ?? new FilePath("build.cake");
            settings.Script = settings.Script.MakeAbsolute(_environment);

            // Read the configuration.
            var configuration = ReadConfiguration(arguments, settings.Script.GetDirectory());

            // Create the scope where we will perform the bootstrapping.
            using (var scope = CreateScope(configuration, arguments))
            {
                var analyzer = scope.Resolve<IScriptAnalyzer>();
                var processor = scope.Resolve<IScriptProcessor>();

                // Set log verbosity for log in new scope.
                var log = scope.Resolve<ICakeLog>();
                log.Verbosity = settings.Verbosity;

                // Get the root directory.
                var root = settings.Script.GetDirectory();

                // Analyze the script.
                log.Debug("Looking for modules...");
                ScriptAnalyzerResult result = PerformAnalysis(analyzer, root, settings);
                if (result.Modules.Count == 0)
                {
                    log.Debug("No modules found to install.");
                    return 0;
                }

                // Install modules.
                processor.InstallModules(
                    result.Modules,
                    configuration.GetModulePath(root, _environment));
            }

            return 0;
        }

        private static ScriptAnalyzerResult PerformAnalysis(IScriptAnalyzer analyzer, DirectoryPath root, BootstrapFeatureSettings settings)
        {
            var result = analyzer.Analyze(settings.Script, new ScriptAnalyzerSettings() { Mode = ScriptAnalyzerMode.Modules });
            if (!result.Succeeded)
            {
                var messages = string.Join("\n", result.Errors.Select(s => $"{root.GetRelativePath(s.File).FullPath}, line #{s.Line}: {s.Message}"));
                throw new AggregateException($"Bootstrapping failed for '{settings.Script}'.\n{messages}");
            }

            return result;
        }
    }
}
