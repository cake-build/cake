using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Builder;
using Cake.Arguments;
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
        private static bool isRunningOnMono = false;

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <returns>The application exit code.</returns>
        public static int Main()
        {
            isRunningOnMono = Type.GetType("Mono.Runtime") != null;
           
            using (var container = CreateContainer())
            {
                // Parse arguments.
                var args = ArgumentTokenizer
                    .Tokenize(Environment.CommandLine)
                    .Skip(1) // Skip executable.
                    .ToArray();

                // Resolve and run the application.
                var application = container.Resolve<CakeApplication>();
                return application.Run(args);
            }
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            // Core services.
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

            if (isRunningOnMono) 
            {
                // Mono scripting
                builder.RegisterType<Cake.Scripting.Mono.MonoScriptEngine>().As<IScriptEngine>().SingleInstance();
            } 
            else 
            {
                // Roslyn related services.
                builder.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().SingleInstance();
                builder.RegisterType<RoslynScriptSessionFactory>().SingleInstance();
                builder.RegisterType<RoslynNightlyScriptSessionFactory>().SingleInstance();
            }

            // Cake services.
            builder.RegisterType<ArgumentParser>().As<IArgumentParser>().SingleInstance();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>().SingleInstance();
            builder.RegisterType<CakeApplication>().SingleInstance();
            builder.RegisterType<ScriptRunner>().As<IScriptRunner>().SingleInstance();
            builder.RegisterType<CakeBuildLog>().As<ICakeLog>().As<IVerbosityAwareLog>().SingleInstance();

            // Register script hosts.
            builder.RegisterType<BuildScriptHost>().SingleInstance();
            builder.RegisterType<DescriptionScriptHost>().SingleInstance();
            builder.RegisterType<DryRunScriptHost>().SingleInstance();

            // Register commands.
            builder.RegisterType<BuildCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DescriptionCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<DryRunCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<HelpCommand>().AsSelf().InstancePerDependency();
            builder.RegisterType<VersionCommand>().AsSelf().InstancePerDependency();

            return builder.Build();
        }
    }
}
