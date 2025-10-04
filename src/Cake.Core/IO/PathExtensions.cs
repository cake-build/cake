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

        /// <summary>
        /// Expands short paths (e.g. C:/Users/ABCDEF~1) to long paths (e.g. C:/Users/abcdefghij).
        /// <para/>
        /// Note that this method only works for absolute paths, as relative paths cannot be expanded without impact.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>The path for which, if available, the short paths are expanded to long paths.</returns>
        public static FilePath ExpandShortPath(this FilePath path)
        {
            if (!path.IsRelative)
            {
                // Only when not relative, resolve short paths to long paths, e.g.:
                //
                // C:/Users/ABCDEF~1/AppData/Local/Temp/cake-build/addins
                // C:/Users/abcdefghij/AppData/Local/Temp/cake-build/addins
                //
                // The reason this is required is that tools / addins can't be located
                // when using short paths
                //
                // Note that a path can contain multiple ~, thus we need to check for just ~
                if (path.FullPath.Contains('~'))
                {
                    return new FilePath(System.IO.Path.GetFullPath(path.FullPath));
                }
            }

            return path;
        }

        /// <summary>
        /// Expands short paths (e.g. C:/Users/ABCDEF~1) to long paths (e.g. C:/Users/abcdefghij).
        /// <para/>
        /// Note that this method only works for absolute paths, as relative paths cannot be expanded without impact.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>The path for which, if available, the short paths are expanded to long paths.</returns>
        public static DirectoryPath ExpandShortPath(this DirectoryPath path)
        {
            if (!path.IsRelative)
            {
                // Only when not relative, resolve short paths to long paths, e.g.:
                //
                // C:/Users/ABCDEF~1/AppData/Local/Temp/cake-build/addins
                // C:/Users/abcdefghij/AppData/Local/Temp/cake-build/addins
                //
                // The reason this is required is that tools / addins can't be located
                // when using short paths
                //
                // Note that a path can contain multiple ~, thus we need to check for just ~
                if (path.FullPath.Contains('~'))
                {
                    return new DirectoryPath(System.IO.Path.GetFullPath(path.FullPath));
                }
            }

            return path;
        }
    }
}
