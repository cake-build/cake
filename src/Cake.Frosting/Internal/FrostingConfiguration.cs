// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake.Frosting.Internal
{
    internal sealed class FrostingConfiguration : ICakeConfiguration
    {
        private readonly ICakeConfiguration _cakeConfiguration;

        public FrostingConfiguration(IEnumerable<FrostingConfigurationValue> values, IFileSystem fileSystem, ICakeEnvironment environment, ICakeArguments arguments)
        {
            ArgumentNullException.ThrowIfNull(values);

            ArgumentNullException.ThrowIfNull(fileSystem);

            ArgumentNullException.ThrowIfNull(environment);

            ArgumentNullException.ThrowIfNull(arguments);

            var baseConfiguration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in values)
            {
                baseConfiguration[value.Key] = value.Value;
            }

            var provider = new CakeConfigurationProvider(fileSystem, environment);
            var args = arguments.GetArguments().ToDictionary(x => x.Key, x => x.Value?.FirstOrDefault() ?? string.Empty);

            _cakeConfiguration = provider.CreateConfiguration(environment.WorkingDirectory, baseConfiguration, args);
        }

        public string GetValue(string key) => _cakeConfiguration.GetValue(key);
    }
}
