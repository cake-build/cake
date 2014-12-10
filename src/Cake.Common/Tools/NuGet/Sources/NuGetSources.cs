﻿using System;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// The NuGet sources is used to work with user config feeds &amp; credentials
    /// </summary>
    public sealed class NuGetSources : Tool<NuGetSourcesSettings>
    {
        private readonly IGlobber _globber;
        private readonly IFileSystem _fileSystem;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetSources"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetSources(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _globber = globber;
            _fileSystem = fileSystem;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Adds NuGet package source using the specified settings to global user config
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void AddSource(string name, string source, NuGetSourcesSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (HasSource(source, settings))
            {
                throw new ArgumentException("Source already exists", "source");
            }

            Run(settings, GetAddArguments(name, source, settings), settings.ToolPath);
        }

        /// <summary>
        /// Remove specified NuGet package source
        /// </summary>
        /// <param name="name">Name of the source.</param>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public void RemoveSource(string name, string source, NuGetSourcesSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (!HasSource(source, settings))
            {
                throw new ArgumentException("Source doesn't exists", "source");
            }

            Run(settings, GetRemoveArguments(name, source, settings), settings.ToolPath);
        }


        /// <summary>
        /// Check NuGet package source exists
        /// </summary>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        public bool HasSource(string source, NuGetSourcesSettings settings)
        {
            if (string.IsNullOrWhiteSpace(source))
            { 
                throw new ArgumentNullException("source");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("sources List");

            var result = false;
            Run(
                settings,
                null,
                settings.ToolPath,
                new ProcessSettings
                {
                    Arguments = "sources List",
                    RedirectStandardOutput = true
                },
                process => result = process.GetStandardOutput().Any(line => line.TrimStart() == source)
                );
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
            // Sensitive information in source?
            if (settings.IsSensitiveSource)
            {
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

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "NuGet";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetSourcesSettings settings)
        {
            const string expression = "./tools/**/NuGet.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
