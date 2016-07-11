// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// The NuGet sources is used to work with user config feeds &amp; credentials
    /// </summary>
    public sealed class NuGetSources : NuGetTool<NuGetSourcesSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSources"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetSources(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Adds NuGet package source using the specified settings to global user config
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void AddSource(string name, string source, NuGetSourcesSettings settings)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", "name");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source cannot be empty.", "source");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (HasSource(source, settings))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "The source '{0}' already exist.", source);
                throw new InvalidOperationException(message);
            }

            Run(settings, GetAddArguments(name, source, settings));
        }

        /// <summary>
        /// Remove specified NuGet package source
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void RemoveSource(string name, string source, NuGetSourcesSettings settings)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", "name");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source cannot be empty.", "source");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (!HasSource(source, settings))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "The source '{0}' does not exist.", source);
                throw new InvalidOperationException(message);
            }

            Run(settings, GetRemoveArguments(name, source, settings));
        }

        /// <summary>
        /// Determines whether the specified NuGet package source exist.
        /// </summary>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Whether the specified NuGet package source exist.</returns>
        public bool HasSource(string source, NuGetSourcesSettings settings)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source cannot be empty.", "source");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var processSettings = new ProcessSettings
            {
                Arguments = "sources List",
                RedirectStandardOutput = true
            };

            var result = false;
            Run(settings, null,  processSettings,
                process => result = process.GetStandardOutput().Any(line => line.TrimStart() == source));

            // Return whether or not the source exist.
            return result;
        }

        private static ProcessArgumentBuilder GetAddArguments(string name, string source, NuGetSourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("sources Add");

            AddCommonParameters(name, source, settings, builder);

            // User name specified?
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.Append("-UserName");
                builder.AppendQuoted(settings.UserName);
            }

            // Password specified?
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("-Password");
                builder.AppendQuotedSecret(settings.Password);
            }

            // Store password in plain text?
            if (settings.StorePasswordInClearText)
            {
                builder.Append("-StorePasswordInClearText");
            }

            return builder;
        }

        private static ProcessArgumentBuilder GetRemoveArguments(string name, string source, NuGetSourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("sources Remove");

            AddCommonParameters(name, source, settings, builder);

            return builder;
        }

        private static void AddCommonParameters(string name, string source, NuGetSourcesSettings settings, ProcessArgumentBuilder builder)
        {
            builder.Append("-Name");
            builder.AppendQuoted(name);

            builder.Append("-Source");
            if (settings.IsSensitiveSource)
            {
                // Sensitive information in source.
                builder.AppendQuotedSecret(source);
            }
            else
            {
                builder.AppendQuoted(source);
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            builder.Append("-NonInteractive");
        }
    }
}
