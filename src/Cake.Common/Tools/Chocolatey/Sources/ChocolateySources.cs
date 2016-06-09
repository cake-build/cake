// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Sources
{
    /// <summary>
    /// The Chocolatey sources is used to work with user config feeds &amp; credentials
    /// </summary>
    public sealed class ChocolateySources : ChocolateyTool<ChocolateySourcesSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateySources"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateySources(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Adds Chocolatey package source using the specified settings to global user config
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void AddSource(string name, string source, ChocolateySourcesSettings settings)
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

            Run(settings, GetAddArguments(name, source, settings));
        }

        /// <summary>
        /// Remove specified Chocolatey package source
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void RemoveSource(string name, ChocolateySourcesSettings settings)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", "name");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetRemoveArguments(name, settings));
        }

        /// <summary>
        /// Enable specified Chocolatey package source
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void EnableSource(string name, ChocolateySourcesSettings settings)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", "name");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetEnableArguments(name, settings));
        }

        /// <summary>
        /// Disable specified Chocolatey package source
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void DisableSource(string name, ChocolateySourcesSettings settings)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", "name");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetDisableArguments(name, settings));
        }

        private static ProcessArgumentBuilder GetAddArguments(string name, string source, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source add");

            AddCommonParameters(name, source, settings, builder);

            // User name specified?
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.UserName);
            }

            // Password specified?
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("-p");
                builder.AppendQuotedSecret(settings.Password);
            }

            return builder;
        }

        private static ProcessArgumentBuilder GetRemoveArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source remove");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private static ProcessArgumentBuilder GetEnableArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source enable");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private static ProcessArgumentBuilder GetDisableArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source disable");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private static void AddCommonParameters(string name, string source, ChocolateySourcesSettings settings, ProcessArgumentBuilder builder)
        {
            builder.Append("-n");
            builder.AppendQuoted(name);

            if (!string.IsNullOrWhiteSpace(source))
            {
                builder.Append("-s");
                builder.AppendQuoted(source);
            }

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unofficial
            if (settings.AllowUnofficial)
            {
                builder.Append("--allowunofficial");
            }

            if (settings.Priority > 0)
            {
                builder.Append("--priority");
                builder.AppendQuoted(settings.Priority.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
