// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Autofac;
using Cake.Arguments;
using Cake.Common.Modules;
using Cake.Composition;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Cake.Core.Text;
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
                var args = QuoteAwareStringSplitter
                    .Split(Environment.CommandLine)
                    .Skip(1) // Skip executable.
                    .ToArray();

                var builder = new ContainerRegistrar();
                builder.RegisterModule(new CakeModule());
                builder.RegisterModule(new CoreModule());
                builder.RegisterModule(new CommonModule());

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
                    builder = new ContainerRegistrar();
                    var provider = container.Resolve<CakeConfigurationProvider>();
                    builder.RegisterModule(new ConfigurationModule(provider, options));
                    builder.RegisterModule(new ArgumentsModule(options));
                    builder.RegisterModule(new ScriptingModule(options, log));
                    builder.Update(container);

                    // Register the NuGetModule
                    builder = new ContainerRegistrar();
                    var configuration = container.Resolve<ICakeConfiguration>();
                    builder.RegisterModule(new NuGetModule(configuration));
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
                return LogException(log, ex);
            }
        }

        private static int LogException<T>(ICakeLog log, T ex) where T : Exception
        {
            log = log ?? new CakeBuildLog(new CakeConsole());
            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", ex);
            }
            else
            {
                log.Error("Error: {0}", ex.Message);
                var aex = ex as AggregateException;
                if (aex != null)
                {
                    foreach (var exception in aex.InnerExceptions)
                    {
                        log.Error("\t{0}", exception.Message);
                    }
                }
            }
            return 1;
        }
    }
}