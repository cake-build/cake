// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// Base class for all GitReleaseManager related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class GitReleaseManagerTool<TSettings> : Tool<TSettings>
        where TSettings : ToolSettings
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseManagerTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected GitReleaseManagerTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected sealed override string GetToolName()
        {
            return "GitReleaseManager";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected sealed override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "GitReleaseManager.exe", "gitreleasemanager.exe", "grm.exe", "dotnet-gitreleasemanager", "dotnet-gitreleasemanager.exe" };
        }

        /// <summary>
        /// Adds common arguments to the process builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The process argument builder.</returns>
        protected ProcessArgumentBuilder AddBaseArguments(GitReleaseManagerSettings settings, ProcessArgumentBuilder builder)
        {
            // Target Directory
            if (settings.TargetDirectory != null)
            {
                builder.Append("-d");
                builder.AppendQuoted(settings.TargetDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Log File Path
            if (settings.LogFilePath != null)
            {
                builder.Append("-l");
                builder.AppendQuoted(settings.LogFilePath.MakeAbsolute(_environment).FullPath);
            }

            // Debug
            if (settings.Debug)
            {
                builder.Append("--debug");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("--verbose");
            }

            // NoLogo
            if (settings.NoLogo)
            {
                builder.Append("--no-logo");
            }

            return builder;
        }
    }
}
