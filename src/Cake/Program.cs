// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using Autofac;
using Cake.Arguments;
using Cake.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Cake.Diagnostics;
using Cake.Modules;
using Cake.NuGet;

namespace Cake
{
    /// <summary>
    /// The Cake program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <returns>The application exit code.</returns>
        public static int Main()
        {
            ICakeLog log = null;

            try
            {
                // Parse arguments.
                var args = ArgumentTokenizer
                    .Tokenize(Environment.CommandLine)
                    .Skip(1) // Skip executable.
                    .ToArray();

                var builder = new CakeContainerBuilder();
                builder.Registry.RegisterModule(new CakeModule());
                builder.Registry.RegisterModule(new CoreModule());
                builder.Registry.RegisterModule(new NuGetModule());

                // Build the container.
                using (var container = builder.Build())
                {
                    // Resolve the log.
                    log = container.Resolve<ICakeLog>();

                    // Parse the options.
                    var parser = container.Resolve<IArgumentParser>();
                    var options = parser.Parse(args);

                    // Set verbosity.
                    log.Verbosity = options.Verbosity;

                    // Rebuild the container.
                    builder = new CakeContainerBuilder();
                    builder.Registry.RegisterModule(new ArgumentsModule(options));
                    builder.Registry.RegisterModule(new ConfigurationModule(container, options));
                    builder.Registry.RegisterModule(new ScriptingModule(options));
                    builder.Update(container);

                    // Load all modules.
                    var loader = container.Resolve<ModuleLoader>();
                    loader.LoadModules(container, options);

                    // Resolve and run the application.
                    var application = container.Resolve<CakeApplication>();
                    return application.Run(options);
                }
            }
            catch (Exception ex)
            {
                log = log ?? new CakeBuildLog(new CakeConsole());
                if (log.Verbosity == Verbosity.Diagnostic)
                {
                    log.Error("Error: {0}", ex);
                }
                else
                {
                    log.Error("Error: {0}", ex.Message);
                }
                return 1;
            }
        }
    }
}
