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
    /// The Chocolatey sources is used to work with user config feeds &amp; credentials.
    /// </summary>
    public sealed class ChocolateySources : ChocolateyTool<ChocolateySourcesSettings>
    {
        private const string Separator = "=";

        private readonly ICakeEnvironment _environment;

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
            _environment = environment;
        }

        /// <summary>
        /// Adds Chocolatey package source using the specified settings to global user config.
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void AddSource(string name, string source, ChocolateySourcesSettings settings)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", nameof(name));
            }

            ArgumentNullException.ThrowIfNull(source);

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source cannot be empty.", nameof(source));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetAddArguments(name, source, settings));
        }

        /// <summary>
        /// Remove specified Chocolatey package source.
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void RemoveSource(string name, ChocolateySourcesSettings settings)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", nameof(name));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetRemoveArguments(name, settings));
        }

        /// <summary>
        /// Enable specified Chocolatey package source.
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void EnableSource(string name, ChocolateySourcesSettings settings)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", nameof(name));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetEnableArguments(name, settings));
        }

        /// <summary>
        /// Disable specified Chocolatey package source.
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void DisableSource(string name, ChocolateySourcesSettings settings)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Source name cannot be empty.", nameof(name));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetDisableArguments(name, settings));
        }

        private ProcessArgumentBuilder GetAddArguments(string name, string source, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source add");

            AddCommonParameters(name, source, settings, builder);

            // User name specified?
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.AppendSwitchQuoted("--user", Separator, settings.UserName);
            }

            // Password specified?
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.AppendSwitchQuoted("--password", Separator, settings.Password);
            }

            // Certificate
            if (settings.Certificate != null)
            {
                builder.AppendSwitchQuoted("--cert", Separator, settings.Certificate.MakeAbsolute(_environment).FullPath);
            }

            // Certificate Password
            if (!string.IsNullOrEmpty(settings.CertificatePassword))
            {
                builder.AppendSwitchQuoted("--certpassword", Separator, settings.CertificatePassword);
            }

            // By Pass Proxy
            if (settings.ByPassProxy)
            {
                builder.Append("--bypass-proxy");
            }

            // Allow Self Service
            if (settings.AllowSelfService)
            {
                builder.Append("--allow-self-service");
            }

            // Admin Only
            if (settings.AdminOnly)
            {
                builder.Append("--admin-only");
            }

            return builder;
        }

        private ProcessArgumentBuilder GetRemoveArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source remove");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetEnableArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source enable");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetDisableArguments(string name, ChocolateySourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("source disable");

            AddCommonParameters(name, string.Empty, settings, builder);

            return builder;
        }

        private void AddCommonParameters(string name, string source, ChocolateySourcesSettings settings, ProcessArgumentBuilder builder)
        {
            builder.AppendSwitchQuoted("--name", Separator, name);

            if (!string.IsNullOrWhiteSpace(source))
            {
                builder.AppendSwitchQuoted("--source", Separator, source);
            }

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            if (settings.Priority > 0)
            {
                builder.AppendSwitchQuoted("--priority", Separator, settings.Priority.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}