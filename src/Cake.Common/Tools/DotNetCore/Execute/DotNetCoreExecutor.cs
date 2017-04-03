// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Execute
{
    /// <summary>
    /// .NET Core assembly executor.
    /// </summary>
    public sealed class DotNetCoreExecutor : DotNetCoreTool<DotNetCoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreExecutor" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreExecutor(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Execute an assembly using arguments and settings.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Execute(FilePath assemblyPath, ProcessArgumentBuilder arguments, DotNetCoreExecuteSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(assemblyPath, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, ProcessArgumentBuilder arguments, DotNetCoreExecuteSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            if (!string.IsNullOrWhiteSpace(settings.FrameworkVersion))
            {
                builder.Append("--fx-version");
                builder.Append(settings.FrameworkVersion);
            }

            assemblyPath = assemblyPath.IsRelative ? assemblyPath.MakeAbsolute(_environment) : assemblyPath;
            builder.Append(assemblyPath.FullPath);

            if (!arguments.IsNullOrEmpty())
            {
                arguments.CopyTo(builder);
            }

            return builder;
        }
    }
}
