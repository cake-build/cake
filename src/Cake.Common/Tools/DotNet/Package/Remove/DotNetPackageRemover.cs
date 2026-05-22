// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.Remove
{
    /// <summary>
    /// .NET package remover.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DotNetPackageRemover" /> class.
    /// </remarks>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public sealed class DotNetPackageRemover(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools)
            : DotNetTool<DotNetPackageRemoveSettings>(fileSystem, environment, processRunner, tools)
    {
        /// <summary>
        /// Removes package reference from a project file.
        /// </summary>
        /// <param name="packageName">The package reference to remove.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        public void Remove(string packageName, string project)
            => Remove(packageName, project, new DotNetPackageRemoveSettings());

        /// <summary>
        /// Removes package reference from a project file.
        /// </summary>
        /// <param name="packageName">The package reference to remove.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="settings">The settings.</param>
        public void Remove(string packageName, string project, DotNetPackageRemoveSettings settings)
        {
            ArgumentNullException.ThrowIfNull(packageName);
            ArgumentNullException.ThrowIfNull(settings);

            RunCommand(settings, GetArguments(packageName, project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageName, string project, DotNetPackageRemoveSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("remove");

            // Project path
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            // Package Name
            builder.AppendSwitch("package", packageName);

            return builder;
        }
    }
}
