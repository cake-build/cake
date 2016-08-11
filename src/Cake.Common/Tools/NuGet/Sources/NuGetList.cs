// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// The NuGet list is used to list the contents of a source
    /// </summary>
    public sealed class NuGetList : NuGetTool<NuGetSourcesSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetList"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetList(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// List NuGet packages for a source using the specified settings to global user config
        /// </summary>
        /// <param name="source">Path to the package(s) source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A list of package names and versions</returns>
        public IEnumerable<string> List(string source, NuGetSourcesSettings settings)
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
                Arguments = GetListArguments(source, settings),
                RedirectStandardOutput = true
            };

            IEnumerable<string> result = null;
            Run(settings, null, processSettings,
                process => result = process.GetStandardOutput().ToList());

            return result;
        }
 
        private static ProcessArgumentBuilder GetListArguments(string source, NuGetSourcesSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("list");

            AddCommonParameters(source, settings, builder);

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

        private static void AddCommonParameters(string source, NuGetSourcesSettings settings, ProcessArgumentBuilder builder)
        {
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