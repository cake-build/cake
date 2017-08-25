// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Base class for all .NET Core related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class DotNetCoreTool<TSettings> : Tool<TSettings>
        where TSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected DotNetCoreTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return ".NET Core CLI";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "dotnet", "dotnet.exe" };
        }

        /// <summary>
        /// Runs the dotnet cli command using the specified settings and arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        protected void RunCommand(TSettings settings, ProcessArgumentBuilder arguments)
        {
            // add arguments common to all commands last
            AppendCommonArguments(arguments, settings);

            Run(settings, arguments, null, null);
        }

        /// <summary>
        /// Runs the dotnet cli command using the specified settings and arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processSettings">The processSettings.</param>
        protected void RunCommand(TSettings settings, ProcessArgumentBuilder arguments, ProcessSettings processSettings)
        {
            // add arguments common to all commands last
            AppendCommonArguments(arguments, settings);

            Run(settings, arguments, processSettings, null);
        }

        /// <summary>
        /// Creates a <see cref="ProcessArgumentBuilder"/> and adds common commandline arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Instance of <see cref="ProcessArgumentBuilder"/>.</returns>
        protected ProcessArgumentBuilder CreateArgumentBuilder(TSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.DiagnosticOutput)
            {
                builder.Append("--diagnostics");
            }

            return builder;
        }

        /// <summary>
        /// Adds common commandline arguments.
        /// </summary>
        /// <param name="builder">Process argument builder to update.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Returns <see cref="ProcessArgumentBuilder"/> updated with common commandline arguments.</returns>
        private ProcessArgumentBuilder AppendCommonArguments(ProcessArgumentBuilder builder, TSettings settings)
        {
            // Verbosity
            if (settings.Verbosity.HasValue)
            {
                builder.Append("--verbosity");
                builder.Append(settings.Verbosity.ToString());
            }

            return builder;
        }
    }
}
