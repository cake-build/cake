// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Configuration;

namespace Cake.Modules
{
    internal sealed class ConfigurationModule : ICakeModule
    {
        private readonly CakeConfigurationProvider _provider;
        private readonly CakeOptions _options;

        public ConfigurationModule(CakeConfigurationProvider provider, CakeOptions options)
        {
            _provider = provider;
            _options = options;
        }

        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var root = _options.Script.GetDirectory();
            var configuration = _provider.CreateConfiguration(root, _options.Arguments);
            registry.RegisterInstance(configuration).As<ICakeConfiguration>();
        }
    }
}