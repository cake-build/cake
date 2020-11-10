// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Reflection;

namespace Cake.Cli
{
    /// <summary>
    /// Represents a version resolver.
    /// </summary>
    public interface IVersionResolver
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The version.</returns>
        string GetVersion();

        /// <summary>
        /// Gets the product version.
        /// </summary>
        /// <returns>The product version.</returns>
        string GetProductVersion();
    }

    /// <summary>
    /// The Cake version resolver.
    /// </summary>
    public sealed class VersionResolver : IVersionResolver
    {
        /// <inheritdoc/>
        public string GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).Comments;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }

        /// <inheritdoc/>
        public string GetProductVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }
    }
}
