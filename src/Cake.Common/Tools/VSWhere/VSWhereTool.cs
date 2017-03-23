// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere
{
    /// <summary>
    /// Base class for all VSWhere related tools.
    /// Used to locate Visual Studio.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class VSWhereTool<TSettings> : Tool<TSettings>
        where TSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool servce.</param>
        protected VSWhereTool(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator toolLocator)
            : base(fileSystem, environment, processRunner, toolLocator)
        {
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "VSWhere";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "vswhere.exe" };
        }

        /// <summary>
        /// Runs VSWhere with supplied arguments and parses installation paths
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The parsed file paths.</returns>
        protected FilePathCollection RunVSWhere(TSettings settings, ProcessArgumentBuilder builder)
        {
            IEnumerable<string> installationPaths = null;
            Run(settings, builder, new ProcessSettings { RedirectStandardOutput = true },
                process => installationPaths = process.GetStandardOutput());

            return new FilePathCollection(installationPaths?.Select(FilePath.FromString) ?? Enumerable.Empty<FilePath>(),
                PathComparer.Default);
        }

        /// <summary>
        /// Adds common arguments to the process builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The process argument builder.</returns>
        protected ProcessArgumentBuilder AddCommonArguments(VSWhereSettings settings, ProcessArgumentBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("-version");
                builder.AppendQuoted(settings.Version);
            }

            if (!string.IsNullOrWhiteSpace(settings.Requires))
            {
                builder.Append("-requires");
                builder.AppendQuoted(settings.Requires);
            }

            if (!string.IsNullOrWhiteSpace(settings.ReturnProperty))
            {
                builder.Append("-property");
                builder.Append(settings.ReturnProperty);
            }

            builder.Append("-nologo");

            return builder;
        }
    }
}
