// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;

namespace Cake.Frosting.Internal
{
    internal sealed class Configuration : ICakeConfiguration
    {
        private readonly CakeConfigurationProvider _provider;
        private readonly CakeHostOptions _options;
        private readonly ICakeEnvironment _environment;
        private readonly IEnumerable<ConfigurationSetting> _values;
        private readonly object _lock;
        private ICakeConfiguration _configuration;

        public Configuration(
            CakeConfigurationProvider provider,
            CakeHostOptions options,
            ICakeEnvironment environment,
            IEnumerable<ConfigurationSetting> values)
        {
            _provider = provider;
            _options = options;
            _environment = environment;
            _values = values;
            _lock = new object();
            _configuration = null;
        }

        public string GetValue(string key)
        {
            lock (_lock)
            {
                if (_configuration == null)
                {
                    var arguments = new Dictionary<string, string>(_options.Arguments, StringComparer.OrdinalIgnoreCase);
                    if (_values != null)
                    {
                        // Add additional configuration values.
                        foreach (var value in _values)
                        {
                            arguments[value.Key] = value.Value;
                        }
                    }
                    _configuration = _provider.CreateConfiguration(_environment.WorkingDirectory, arguments);
                }
                return _configuration.GetValue(key);
            }
        }
    }
}
