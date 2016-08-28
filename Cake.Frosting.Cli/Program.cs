// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting.Cli.Project;
using Cake.Frosting.Cli.Reflection;

namespace Cake.Frosting.Cli
{
    /// <summary>
    /// The CLI tool.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code.</returns>
        public static int Main(string[] args)
        {
            try
            {
                using (var container = BuildContainer())
                {
                    var application = container.Resolve<Application>();
                    return application.Run(args);
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().SingleInstance();
            builder.RegisterType<CakePlatform>().As<ICakePlatform>().SingleInstance();
            builder.RegisterType<CakeRuntime>().As<ICakeRuntime>().SingleInstance();

            builder.RegisterType<Application>().SingleInstance();
            builder.RegisterType<ProjectLocator>().SingleInstance();
            builder.RegisterType<ProjectBuilder>().SingleInstance();
            builder.RegisterType<ProjectLoader>().SingleInstance();
            builder.RegisterType<StartupFinder>().SingleInstance();

            return builder.Build();
        }
    }
}
