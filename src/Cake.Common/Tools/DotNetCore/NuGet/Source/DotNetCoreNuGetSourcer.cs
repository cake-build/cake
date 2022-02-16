// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Common.Tools.DotNet.NuGet.Source;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.NuGet.Source
{
    /// <summary>
    /// .NET Core NuGet sourcer.
    /// </summary>
    public sealed class DotNetCoreNuGetSourcer : DotNetCoreTool<DotNetNuGetSourceSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreNuGetSourcer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreNuGetSourcer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Add the specified NuGet source.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void AddSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.Source))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(settings.Source));
            }

            RunCommand(settings, GetAddSourceArguments(name, settings));
        }

        /// <summary>
        /// Disable the specified NuGet source.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void DisableSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetDisableSourceArguments(name, settings));
        }

        /// <summary>
        /// Enable the specified NuGet source.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void EnableSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetEnableSourceArguments(name, settings));
        }

        /// <summary>
        /// Determines whether the specified NuGet source exists.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Whether the specified NuGet source exists.</returns>
        public bool HasSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var sources = ListSource("detailed", settings);
            var matches = Regex.Matches(sources, @"\d+\.\s+(?<name>.+?)\s+\[(?:Enabled|Disabled)\]", RegexOptions.IgnoreCase);

            return matches.Cast<Match>().Any(match => match.Groups["name"].Value.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Lists the NuGet sources.
        /// </summary>
        /// <param name="format">The output format. Accepts two values: detailed (the default) and short.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The NuGet sources.</returns>
        public string ListSource(string format, DotNetNuGetSourceSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            string output = null;
            Run(settings, GetListSourceArguments(format, settings), new ProcessSettings { RedirectStandardOutput = true },
                process => output = string.Join(Environment.NewLine, process.GetStandardOutput()));

            return output;
        }

        /// <summary>
        /// Remove the specified NuGet source.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void RemoveSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetRemoveSourceArguments(name, settings));
        }

        /// <summary>
        /// Update the specified NuGet source.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        public void UpdateSource(string name, DotNetNuGetSourceSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetUpdateSourceArguments(name, settings));
        }

        private ProcessArgumentBuilder GetAddSourceArguments(string name, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget add source");
            if (settings.IsSensitiveSource)
            {
                builder.AppendQuotedSecret(settings.Source);
            }
            else
            {
                builder.AppendQuoted(settings.Source);
            }
            builder.Append("--name");
            builder.AppendQuoted(name);
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.Append("--username");
                builder.AppendQuoted(settings.UserName);
            }
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("--password");
                builder.AppendQuotedSecret(settings.Password);
            }
            if (settings.StorePasswordInClearText)
            {
                builder.Append("--store-password-in-clear-text");
            }
            if (!string.IsNullOrWhiteSpace(settings.ValidAuthenticationTypes))
            {
                builder.Append("--valid-authentication-types");
                builder.AppendQuoted(settings.ValidAuthenticationTypes);
            }
            AddCommonArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetDisableSourceArguments(string name, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget disable source");
            builder.AppendQuoted(name);
            AddCommonArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetEnableSourceArguments(string name, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget enable source");
            builder.AppendQuoted(name);
            AddCommonArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetListSourceArguments(string format, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget list source");
            if (!string.IsNullOrWhiteSpace(format))
            {
                builder.Append("--format");
                builder.AppendQuoted(format);
            }
            AddCommonArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetRemoveSourceArguments(string name, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget remove source");
            builder.AppendQuoted(name);
            AddCommonArguments(settings, builder);

            return builder;
        }

        private ProcessArgumentBuilder GetUpdateSourceArguments(string name, DotNetNuGetSourceSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget update source");
            builder.AppendQuoted(name);
            if (!string.IsNullOrWhiteSpace(settings.Source))
            {
                builder.Append("--source");
                if (settings.IsSensitiveSource)
                {
                    builder.AppendQuotedSecret(settings.Source);
                }
                else
                {
                    builder.AppendQuoted(settings.Source);
                }
            }
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.Append("--username");
                builder.AppendQuoted(settings.UserName);
            }
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("--password");
                builder.AppendQuotedSecret(settings.Password);
            }
            if (settings.StorePasswordInClearText)
            {
                builder.Append("--store-password-in-clear-text");
            }
            if (!string.IsNullOrWhiteSpace(settings.ValidAuthenticationTypes))
            {
                builder.Append("--valid-authentication-types");
                builder.AppendQuoted(settings.ValidAuthenticationTypes);
            }
            AddCommonArguments(settings, builder);

            return builder;
        }

        private void AddCommonArguments(DotNetNuGetSourceSettings settings, ProcessArgumentBuilder builder)
        {
            if (settings.ConfigFile != null)
            {
                builder.Append("--configfile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }
        }
    }
}