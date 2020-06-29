// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Base class for all DotCover related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class DotCoverTool<TSettings> : Tool<TSettings>
        where TSettings : DotCoverSettings
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected DotCoverTool(
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
        protected override string GetToolName()
        {
            return "DotCover";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "dotCover.exe" };
        }

        /// <summary>
        /// Get arguments from global settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The process arguments.</returns>
        protected ProcessArgumentBuilder GetArguments(DotCoverSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // LogFile
            if (settings.LogFile != null)
            {
                var logFilePath = settings.LogFile.MakeAbsolute(_environment);
                builder.AppendSwitch("/LogFile", "=", logFilePath.FullPath.Quote());
            }

            return builder;
        }

        /// <summary>
        /// Get configuration full path from coverage settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The process arguments.</returns>
        protected ProcessArgumentBuilder GetConfigurationFileArgument(DotCoverSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.ConfigFile != null)
            {
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}