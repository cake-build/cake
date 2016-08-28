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
        private readonly ProjectLocator _locator;
        private readonly ProjectBuilder _builder;
        private readonly ProjectLoader _loader;
        private readonly StartupFinder _finder;

        public Application(ICakeEnvironment environment, ProjectLocator locator,
            ProjectBuilder builder, ProjectLoader loader, StartupFinder finder)
        {
            _environment = environment;
            _locator = locator;
            _builder = builder;
            _loader = loader;
            _finder = finder;
        }

        public int Run(string[] args)
        {
            // Get the project.
            var project = _locator.GetProject(_environment.WorkingDirectory);
            if (project == null)
            {
                throw new FrostingException("The specified path does not contain a Cake.Frosting project.");
            }

            // Build the project.
            var buildResult = _builder.Build(project);
            if (buildResult != 0)
            {
                throw new FrostingException($"Build failed (exit code {buildResult}).");
            }

            // Load project output.
            var assembly = _loader.Load(project);
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
