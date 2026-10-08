// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.NuGet.Locals;

/// <summary>
/// .NET NuGet locals runner. Lists or clears local NuGet cache folders.
/// </summary>
public sealed class DotNetNuGetLocalser : DotNetTool<DotNetNuGetLocalsSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetNuGetLocalser" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetNuGetLocalser(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
    }

    /// <summary>
    /// Lists local NuGet cache folders.
    /// </summary>
    /// <param name="folder">The local folder to list.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The parsed local folder paths.</returns>
    public DotNetNuGetLocalsListResult List(DotNetNuGetLocalsFolder folder, DotNetNuGetLocalsSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

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
        RunCommand(settings, GetArguments(folder, list: true, settings), processSettings,
            process => result = process.GetStandardOutput());

        return DotNetNuGetLocalsParser.Parse(result);
    }

    /// <summary>
    /// Clears local NuGet cache folders.
    /// </summary>
    /// <param name="folder">The local folder to clear.</param>
    /// <param name="settings">The settings.</param>
    public void Clear(DotNetNuGetLocalsFolder folder, DotNetNuGetLocalsSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        RunCommand(settings, GetArguments(folder, list: false, settings));
    }

    private ProcessArgumentBuilder GetArguments(DotNetNuGetLocalsFolder folder, bool list, DotNetNuGetLocalsSettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("nuget");
        builder.Append("locals");
        builder.Append(GetFolderArgument(folder));
        builder.Append(list ? "--list" : "--clear");

        if (settings.ForceEnglishOutput)
        {
            builder.Append("--force-english-output");
        }

        return builder;
    }

    private static string GetFolderArgument(DotNetNuGetLocalsFolder folder)
    {
        return folder switch
        {
            DotNetNuGetLocalsFolder.All => "all",
            DotNetNuGetLocalsFolder.HttpCache => "http-cache",
            DotNetNuGetLocalsFolder.GlobalPackages => "global-packages",
            DotNetNuGetLocalsFolder.Temp => "temp",
            DotNetNuGetLocalsFolder.PluginsCache => "plugins-cache",
            _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, "Invalid value")
        };
    }
}
