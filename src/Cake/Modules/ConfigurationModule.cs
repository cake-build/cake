// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using Autofac;
using Cake.Core.Composition;
using Cake.Core.Configuration;

namespace Cake.Modules
{
    internal sealed class ConfigurationModule : ICakeModule
    {
        private readonly CakeConfigurationProvider _provider;
        private readonly CakeOptions _options;

        public ConfigurationModule(IContainer container, CakeOptions options)
        {
            _provider = container.Resolve<CakeConfigurationProvider>();
            _options = options;
        }

        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var configuration = _provider.CreateConfiguration(_options.Arguments);
            Debug.Assert(configuration != null, "Configuration should not be null.");
            registry.RegisterInstance(configuration).As<ICakeConfiguration>();
        }
    }
}
