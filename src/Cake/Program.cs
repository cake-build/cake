using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Cake.Arguments;
using Cake.Autofac;
using Cake.Commands;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting;
using Cake.Diagnostics;
using Cake.Scripting;
using Cake.Scripting.Roslyn;
using Cake.Scripting.Roslyn.Nightly;
using Cake.Scripting.Roslyn.Stable;

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
            using (var container = CreateContainer())
            {
                // Parse all options.
                var commandLine = Environment.CommandLine;
                var args = ArgumentTokenizer.Tokenize(commandLine);
                var parser = container.Resolve<IArgumentParser>();
                var options = parser.Parse(args);

                // Find all modules.
                var finder = container.Resolve<CakeModuleFinder>();
                var modules = finder.FindModules(options);
                if (modules != null && modules.Count > 0)
                {
                    // Create the bootstrapper.
                    var bootstrapper = new AutofacCakeContainer();

                    // Let all modules register their dependencies.
                    foreach (var module in modules)
                    {
                        module.Register(bootstrapper);
                    }

                    // Update the container.
                    bootstrapper.Update(container);
                }

                // Resolve the Cake application and run it.
                var application = container.Resolve<CakeApplication>();
                return application.Run(options);
            }
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            // Register Cake services.
            builder.RegisterType<ArgumentParser>().As<IArgumentParser>().SingleInstance();
            builder.RegisterType<CakeModuleFinder>().SingleInstance();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>().SingleInstance();
            builder.RegisterType<CakeApplication>().SingleInstance();
            builder.RegisterType<ScriptRunner>().As<IScriptRunner>().SingleInstance();

            // Register log.
            builder.RegisterType<CakeBuildLog>().As<ICakeLog>().As<IVerbosityAwareLog>().SingleInstance();

            // Register hosts.
            builder.RegisterType<BuildScriptHost>().SingleInstance();
            builder.RegisterType<DescriptionScriptHost>().SingleInstance();
            builder.RegisterType<DryRunScriptHost>().SingleInstance();

            // Register commands.
            builder.RegisterType<BuildCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DescriptionCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DryRunCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<HelpCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<VersionCommand>().AsSelf().InstancePerDependency();

            // Register core services.
            builder.RegisterType<CakeEngine>().As<ICakeEngine>().SingleInstance();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().SingleInstance();
            builder.RegisterType<CakeArguments>().As<ICakeArguments>().SingleInstance();
            builder.RegisterType<Globber>().As<IGlobber>().SingleInstance();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>().SingleInstance();
            builder.RegisterType<ScriptAliasFinder>().As<IScriptAliasFinder>().SingleInstance();
            builder.RegisterType<CakeReportPrinter>().As<ICakeReportPrinter>().SingleInstance();
            builder.RegisterType<CakeConsole>().As<IConsole>().SingleInstance();
            builder.RegisterType<ScriptProcessor>().As<IScriptProcessor>().SingleInstance();
            builder.RegisterCollection<IToolResolver>("toolResolvers").As<IEnumerable<IToolResolver>>();
            builder.RegisterType<NuGetToolResolver>().As<IToolResolver>().As<INuGetToolResolver>().SingleInstance().MemberOf("toolResolvers");
            builder.RegisterType<WindowsRegistry>().As<IRegistry>().SingleInstance();
            builder.RegisterType<CakeContext>().As<ICakeContext>().SingleInstance();

            // Register Roslyn services.
            builder.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().SingleInstance();
            builder.RegisterType<RoslynScriptSessionFactory>().SingleInstance();
            builder.RegisterType<RoslynNightlyScriptSessionFactory>().SingleInstance();

            return builder.Build();
        }
    }
}
