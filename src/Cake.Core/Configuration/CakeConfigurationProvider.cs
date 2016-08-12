// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Configuration.Parser;
using Cake.Core.IO;

namespace Cake.Core.Configuration
{
    /// <summary>
    /// Implementation of the Cake configuration provider.
    /// </summary>
    public sealed class CakeConfigurationProvider
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeConfigurationProvider"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public CakeConfigurationProvider(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Creates a configuration from the provided arguments.
        /// </summary>
        /// <param name="path">The directory to look for the configuration file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The created configuration.</returns>
        public ICakeConfiguration CreateConfiguration(DirectoryPath path, IDictionary<string, string> arguments)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Get all environment variables.
            foreach (var variable in _environment.GetEnvironmentVariables())
            {
                if (variable.Key.StartsWith("CAKE_", StringComparison.OrdinalIgnoreCase))
                {
                    var key = variable.Key.Substring(5);
                    result[KeyNormalizer.Normalize(key)] = variable.Value;
                }
            }

            // Parse the configuration file.
            var configurationPath = path.CombineWithFilePath("cake.config").MakeAbsolute(_environment);
            if (_fileSystem.Exist(configurationPath))
            {
                var parser = new ConfigurationParser(_fileSystem, _environment);
                var configuration = parser.Read(configurationPath);
                foreach (var key in configuration.Keys)
                {
                    result[KeyNormalizer.Normalize(key)] = configuration[key];
                }
            }

            // Add all arguments.
            foreach (var key in arguments.Keys)
            {
                result[KeyNormalizer.Normalize(key)] = arguments[key];
            }

            return new CakeConfiguration(result);
        }
    }
}