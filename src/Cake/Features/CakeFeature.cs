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

namespace Cake.Features
{
    /// <summary>
    /// Represents a base feature for Cake.
    /// </summary>
    public abstract class Feature
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IContainerConfigurator _configurator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="configurator">The container configurator.</param>
        public Feature(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IContainerConfigurator configurator)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _configurator = configurator;
        }

        /// <summary>
        /// Creates a container scope with the specified configuration and arguments.
        /// </summary>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="arguments">The Cake arguments.</param>
        /// <param name="action">An optional action to configure the container registrar.</param>
        /// <returns>A container scope.</returns>
        protected IContainer CreateScope(
            ICakeConfiguration configuration,
            ICakeArguments arguments,
            Action<ICakeContainerRegistrar> action = null)
        {
            var registrar = new AutofacTypeRegistrar(new ContainerBuilder());

            _configurator.Configure(registrar, configuration, arguments);
            action?.Invoke(registrar);

            return registrar.BuildContainer();
        }

        /// <summary>
        /// Reads the Cake configuration from the specified arguments and root directory.
        /// </summary>
        /// <param name="arguments">The Cake arguments.</param>
        /// <param name="root">The root directory.</param>
        /// <returns>The Cake configuration.</returns>
        protected ICakeConfiguration ReadConfiguration(
            ICakeArguments arguments, DirectoryPath root)
        {
            var provider = new CakeConfigurationProvider(_fileSystem, _environment);
            var args = arguments.GetArguments().ToDictionary(x => x.Key, x => x.Value?.FirstOrDefault() ?? string.Empty);

            return provider.CreateConfiguration(root, args);
        }
    }
}
