// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.Update;

/// <summary>
/// .NET package updater.
/// </summary>
public sealed class DotNetPackageUpdater : DotNetTool<DotNetPackageUpdateSettings>
{
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetPackageUpdater" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetPackageUpdater(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
        _environment = environment;
    }

    /// <summary>
    /// Updates referenced packages in a project.
    /// </summary>
    /// <param name="packages">The packages to update. When null or empty, all packages are updated.</param>
    /// <param name="project">The target project file or directory. When set, this value wins over <see cref="DotNetPackageUpdateSettings.Project"/>.</param>
    /// <param name="settings">The settings.</param>
    public void Update(IEnumerable<string> packages, string project, DotNetPackageUpdateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        RunCommand(settings, GetArguments(packages, project, settings));
    }

    private ProcessArgumentBuilder GetArguments(IEnumerable<string> packages, string project, DotNetPackageUpdateSettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("package");
        builder.Append("update");

        if (packages != null)
        {
            foreach (var package in packages.Where(package => !string.IsNullOrWhiteSpace(package)))
            {
                builder.AppendQuoted(package);
            }
        }

        var projectPath = project ?? settings.Project?.FullPath;
        if (!string.IsNullOrEmpty(projectPath))
        {
            builder.AppendSwitchQuoted("--project", new FilePath(projectPath).MakeAbsolute(_environment).FullPath);
        }

        if (settings.Interactive)
        {
            builder.Append("--interactive");
        }

        if (settings.Vulnerable)
        {
            builder.Append("--vulnerable");
        }

        return builder;
    }
}
