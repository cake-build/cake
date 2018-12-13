// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Contains extensions for <see cref="FilePath"/> and <see cref="DirectoryPath"/>.
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// Expands all environment variables in the provided <see cref="FilePath"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var path = new FilePath("%APPDATA%/foo.bar");
        /// var expanded = path.ExpandEnvironmentVariables(environment);
        /// </code>
        /// </example>
        /// <param name="path">The file path to expand.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>A new <see cref="FilePath"/> with each environment variable replaced by its value.</returns>
        public static FilePath ExpandEnvironmentVariables(this FilePath path, ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var result = environment.ExpandEnvironmentVariables(path.FullPath);
            return new FilePath(result);
        }

        /// <summary>
        /// Expands all environment variables in the provided <see cref="DirectoryPath"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var path = new DirectoryPath("%APPDATA%");
        /// var expanded = path.ExpandEnvironmentVariables(environment);
        /// </code>
        /// </example>
        /// <param name="path">The directory to expand.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>A new <see cref="DirectoryPath"/> with each environment variable replaced by its value.</returns>
        public static DirectoryPath ExpandEnvironmentVariables(this DirectoryPath path, ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var result = environment.ExpandEnvironmentVariables(path.FullPath);
            return new DirectoryPath(result);
        }
    }
}
