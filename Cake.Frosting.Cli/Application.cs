// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Frosting.Cli.Project;
using Cake.Frosting.Cli.Reflection;

namespace Cake.Frosting.Cli
{
    internal sealed class Application
    {
        private readonly ICakeEnvironment _environment;
        private readonly ProjectServices _projectServices;
        private readonly StartupFinder _finder;

        public Application(
            ICakeEnvironment environment,
            ProjectServices projectServices,
            StartupFinder finder)
        {
            _environment = environment;
            _projectServices = projectServices;
            _finder = finder;
        }

        public int Run(string[] args)
        {
            // Get the project context.
            var context = _projectServices.Locator.GetProject(_environment.WorkingDirectory);
            if (context == null)
            {
                throw new FrostingException("The specified path does not contain a Cake.Frosting project.");
            }

            // Build the project.
            var buildResult = _projectServices.Builder.Build(context);
            if (buildResult != 0)
            {
                throw new FrostingException($"Build failed (exit code {buildResult}).");
            }

            // Load project output.
            var assembly = _projectServices.Loader.Load(context);
            if (assembly == null)
            {
                throw new FrostingException("Could not load project output.");
            }

            var startup = _finder.FindStartup(assembly);
            if (startup == null)
            {
                throw new FrostingException("Could not find a startup type.");
            }

            // Create a host and run it.
            return new CakeHostBuilder()
                .WithArguments(args)
                .UseStartup(startup)
                .ConfigureServices(s => s.RegisterInstance(assembly))
                .Build()
                .Run();
        }
    }
}
