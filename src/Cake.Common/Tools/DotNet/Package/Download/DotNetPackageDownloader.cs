// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.Download;

/// <summary>
/// .NET package downloader.
/// </summary>
public sealed class DotNetPackageDownloader : DotNetTool<DotNetPackageDownloadSettings>
{
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetPackageDownloader" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetPackageDownloader(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
        _environment = environment;
    }

    /// <summary>
    /// Downloads NuGet packages to disk without changing project references.
    /// </summary>
    /// <param name="packages">The packages to download. Each value is a package id or <c>id@version</c>.</param>
    /// <param name="settings">The settings.</param>
    public void Download(IEnumerable<string> packages, DotNetPackageDownloadSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (packages == null || !packages.Any(package => !string.IsNullOrWhiteSpace(package)))
        {
            throw new ArgumentException("Packages cannot be empty.", nameof(packages));
        }

        RunCommand(settings, GetArguments(packages, settings));
    }

    private ProcessArgumentBuilder GetArguments(IEnumerable<string> packages, DotNetPackageDownloadSettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("package");
        builder.Append("download");

        foreach (var package in packages.Where(package => !string.IsNullOrWhiteSpace(package)))
        {
            builder.AppendQuoted(package);
        }

        if (settings.Output != null)
        {
            builder.AppendSwitchQuoted("--output", settings.Output.MakeAbsolute(_environment).FullPath);
        }

        if (settings.Sources != null)
        {
            foreach (var source in settings.Sources)
            {
                builder.Append("--source");
                builder.AppendQuoted(source);
            }
        }

        if (settings.Prerelease)
        {
            builder.Append("--prerelease");
        }

        if (settings.ConfigFile != null)
        {
            builder.Append("--configfile");
            builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
        }

        if (settings.Interactive)
        {
            builder.Append("--interactive");
        }

        if (settings.AllowInsecureConnections)
        {
            builder.Append("--allow-insecure-connections");
        }

        return builder;
    }
}
