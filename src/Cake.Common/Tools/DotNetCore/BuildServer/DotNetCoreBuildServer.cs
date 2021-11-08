// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.BuildServer;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.BuildServer
{
    /// <summary>
    /// .NET Core project builder.
    /// </summary>
    public sealed class DotNetCoreBuildServer : DotNetCoreTool<DotNetBuildServerShutdownSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreBuildServer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreBuildServer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Build the project using the specified path and settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Shutdown(DotNetBuildServerShutdownSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(DotNetBuildServerShutdownSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("build-server");
            builder.Append("shutdown");

            if (settings.MSBuild ?? false)
            {
                builder.Append("--msbuild");
            }

            if (settings.Razor ?? false)
            {
                builder.Append("--razor");
            }

            if (settings.VBCSCompiler ?? false)
            {
                builder.Append("--vbcscompiler");
            }

            return builder;
        }
    }
}
