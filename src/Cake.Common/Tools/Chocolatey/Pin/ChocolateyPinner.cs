﻿using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.Pin
{
    /// <summary>
    /// The Chocolatey package pinner used to pin Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyPinner : ChocolateyTool<ChocolateyPinSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPinner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyPinner(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, IChocolateyToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
        }

        /// <summary>
        /// Pins Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="settings">The settings.</param>
        public void Pin(string name, ChocolateyPinSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Run(settings, GetArguments(name, settings));
        }

        private ProcessArgumentBuilder GetArguments(string name, ChocolateyPinSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("pin");
            builder.Append("add");

            builder.Append("-n");
            builder.AppendQuoted(name);

            // Version
            if (settings.Version != null)
            {
                builder.Append("--version");
                builder.AppendQuoted(settings.Version);
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

            return builder;
        }
    }
}