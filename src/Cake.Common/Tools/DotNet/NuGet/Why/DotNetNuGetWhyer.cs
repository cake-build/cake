// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// .NET NuGet why runner. Shows why a package is in the restore graph.
/// </summary>
public sealed class DotNetNuGetWhyer : DotNetTool<DotNetNuGetWhySettings>
{
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetWhyer" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetNuGetWhyer(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
        _environment = environment;
    }

    /// <summary>
    /// Shows why a package is in the restore graph.
    /// </summary>
    /// <param name="project">The project or solution file to operate on. If not specified, <see cref="DotNetNuGetWhySettings.Project"/> is used.</param>
    /// <param name="package">The package identifier.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The parsed dependency graphs.</returns>
    public DotNetNuGetWhyResult Why(string project, string package, DotNetNuGetWhySettings settings)
    {
        if (string.IsNullOrWhiteSpace(package))
        {
            throw new ArgumentException("Package cannot be null or empty.", nameof(package));
        }

        ArgumentNullException.ThrowIfNull(settings);

        project ??= settings.Project;

        var processSettings = new ProcessSettings
        {
            RedirectStandardOutput = true,
            EnvironmentVariables = new Dictionary<string, string>(settings.EnvironmentVariables)
            {
                { "DOTNET_CLI_UI_LANGUAGE", "en" },
                { "NO_COLOR", "1" }
            }
        };

        IEnumerable<string> result = null;
        RunCommand(settings, GetArguments(project, package, settings), processSettings,
            process => result = process.GetStandardOutput());

        return DotNetNuGetWhyParser.Parse(result);
    }

    private ProcessArgumentBuilder GetArguments(string project, string package, DotNetNuGetWhySettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("nuget");
        builder.Append("why");

        if (!string.IsNullOrWhiteSpace(project))
        {
            builder.AppendQuoted(new FilePath(project).MakeAbsolute(_environment).FullPath);
        }

        builder.Append(package);

        if (settings.Frameworks != null)
        {
            foreach (var framework in settings.Frameworks)
            {
                if (!string.IsNullOrWhiteSpace(framework))
                {
                    builder.AppendSwitch("--framework", framework);
                }
            }
        }

        return builder;
    }
}
