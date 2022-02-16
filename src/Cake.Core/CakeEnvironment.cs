// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class CakeEnvironment : ICakeEnvironment
    {
        /// <inheritdoc/>
        public DirectoryPath WorkingDirectory
        {
            get { return System.IO.Directory.GetCurrentDirectory(); }
            set { SetWorkingDirectory(value); }
        }

        /// <inheritdoc/>
        public DirectoryPath ApplicationRoot { get; }

        /// <inheritdoc/>
        public ICakePlatform Platform { get; }

        /// <inheritdoc/>
        public ICakeRuntime Runtime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEnvironment" /> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="runtime">The runtime.</param>
        public CakeEnvironment(ICakePlatform platform, ICakeRuntime runtime)
        {
            Platform = platform;
            Runtime = runtime;

            // Get the application root.
            var assembly = AssemblyHelper.GetExecutingAssembly();
            var path = PathHelper.GetDirectoryName(assembly.Location);
            ApplicationRoot = new DirectoryPath(path);

            // Get the working directory.
            WorkingDirectory = new DirectoryPath(System.IO.Directory.GetCurrentDirectory());
        }

        /// <inheritdoc/>
        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            return SpecialPathHelper.GetFolderPath(Platform, path);
        }

        /// <inheritdoc/>
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        /// <inheritdoc/>
        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .Aggregate(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                    (dictionary, entry) =>
                    {
                        var key = (string)entry.Key;
                        if (!dictionary.TryGetValue(key, out _))
                        {
                            dictionary.Add(key, entry.Value as string);
                        }
                        return dictionary;
                    },
                    dictionary => dictionary);
        }

        private static void SetWorkingDirectory(DirectoryPath path)
        {
            if (path.IsRelative)
            {
                throw new CakeException("Working directory can not be set to a relative path.");
            }
            System.IO.Directory.SetCurrentDirectory(path.FullPath);
        }
    }
}