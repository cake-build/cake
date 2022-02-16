// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Autofac;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Spectre.Console.Cli;

namespace Cake.Features
{
    public abstract class Feature
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IContainerConfigurator _configurator;

        public Feature(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IContainerConfigurator configurator)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _configurator = configurator;
        }

        protected IContainer CreateScope(
            ICakeConfiguration configuration,
            IRemainingArguments arguments,
            Action<ICakeContainerRegistrar> action = null)
        {
            var registrar = new AutofacTypeRegistrar(new ContainerBuilder());

            _configurator.Configure(registrar, configuration, arguments);
            action?.Invoke(registrar);

            return registrar.BuildContainer();
        }

        protected ICakeConfiguration ReadConfiguration(
            IRemainingArguments remaining, DirectoryPath root)
        {
            var provider = new CakeConfigurationProvider(_fileSystem, _environment);
            var args = remaining.Parsed.ToDictionary(x => x.Key, x => x.FirstOrDefault() ?? string.Empty);

            return provider.CreateConfiguration(root, args);
        }
    }
}
