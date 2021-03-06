// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Cli;
using Cake.Common.Modules;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Modules;
using Cake.DotNetTool.Module;
using Cake.Frosting.Internal;
using Cake.NuGet;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cake.Frosting
{
    /// <summary>
    /// The Cake host.
    /// </summary>
    public sealed class CakeHost
    {
        private readonly IServiceCollection _services;
        private readonly List<Assembly> _assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeHost"/> class.
        /// </summary>
        public CakeHost()
        {
            _services = CreateServiceCollection();
            _assemblies = new List<Assembly>
            {
                Assembly.GetEntryAssembly()
            };
        }

        /// <summary>
        /// Creates a <see cref="CakeHost"/>.
        /// </summary>
        /// <returns>The created <see cref="CakeHost"/>.</returns>
        public static CakeHost Create()
        {
            return new CakeHost();
        }

        /// <summary>
        /// Registers an assembly which will be used to find tasks.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public CakeHost AddAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring additional services for the host.
        /// </summary>
        /// <param name="services">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public CakeHost ConfigureServices(Action<IServiceCollection> services)
        {
            services(_services);
            return this;
        }

        /// <summary>
        /// Runs the build with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code.</returns>
        public int Run(IEnumerable<string> args)
        {
            RegisterTasks(_assemblies);

            // Register all the user's registrations
            var registrar = new TypeRegistrar();
            registrar.RegisterInstance(typeof(IServiceCollection), _services);

            // Run the application
            var app = new CommandApp<DefaultCommand>(registrar);
            app.Configure(config =>
            {
                config.ValidateExamples();

                // Top level examples.
                config.AddExample(new[] { string.Empty });
                config.AddExample(new[] { "--verbosity", "quiet" });
                config.AddExample(new[] { "--tree" });
            });

            return app.Run(args);
        }

        private ServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ICakeLog, CakeBuildLog>();
            services.AddSingleton<IConsole, CakeConsole>();
            services.AddSingleton<ICakeReportPrinter, CakeReportPrinter>();
            services.AddSingleton<ICakeConfiguration, FrostingConfiguration>();

            services.AddSingleton<BuildScriptHost<IFrostingContext>>();
            services.AddSingleton<DryRunScriptHost<IFrostingContext>>();
            services.AddSingleton<TreeScriptHost>();
            services.AddSingleton<DescriptionScriptHost>();

            services.AddSingleton<IVersionResolver, VersionResolver>();
            services.AddSingleton<VersionFeature>();
            services.AddSingleton<InfoFeature>();

            services.AddSingleton<FrostingRunner>();
            services.AddSingleton<FrostingDryRunner>();
            services.AddSingleton<FrostingTreeRunner>();
            services.AddSingleton<FrostingDescriptionRunner>();

            services.AddSingleton<IToolInstaller, ToolInstaller>();

            services.UseModule<CoreModule>();
            services.UseModule<CommonModule>();
            services.UseModule<NuGetModule>();
            services.UseModule<DotNetToolModule>();

            services.AddSingleton<FrostingContext>();
            services.AddSingleton<IFrostingContext>(f => f.GetService<FrostingContext>());

            return services;
        }

        private void RegisterTasks(IEnumerable<Assembly> assemblies)
        {
            // Find tasks in registered assemblies.
            var tasks = GetTasks(assemblies);
            if (tasks.Length > 0)
            {
                foreach (var task in tasks)
                {
                    _services.AddSingleton(typeof(IFrostingTask), task);
                }
            }
        }

        private static Type[] GetTasks(IEnumerable<Assembly> assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }

                foreach (var type in assembly.GetExportedTypes())
                {
                    var info = type.GetTypeInfo();
                    if (typeof(IFrostingTask).IsAssignableFrom(type) && info.IsClass && !info.IsAbstract)
                    {
                        result.Add(type);
                    }
                }
            }

            return result.ToArray();
        }
    }
}
