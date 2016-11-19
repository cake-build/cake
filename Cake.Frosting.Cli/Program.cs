// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;
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
                var application = CreateApplication();
                return application.Run(args);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
        }

        private static Application CreateApplication()
        {
            var fileSystem = new FileSystem();
            var environment = new CakeEnvironment(new CakePlatform(), new CakeRuntime(), new NullLog());
            var projectServices = new ProjectServices(fileSystem, environment);
            var startupFinder = new StartupFinder();

            return new Application(environment, projectServices, startupFinder);
        }
    }
}
