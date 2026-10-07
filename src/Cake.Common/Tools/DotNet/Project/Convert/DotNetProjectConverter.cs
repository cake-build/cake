// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Project.Convert;

/// <summary>
/// .NET project converter.
/// </summary>
public sealed class DotNetProjectConverter : DotNetTool<DotNetProjectConvertSettings>
{
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetProjectConverter" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetProjectConverter(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
        _environment = environment;
    }

    /// <summary>
    /// Converts a file-based program to a project-based program.
    /// </summary>
    /// <param name="file">The file-based program to convert.</param>
    /// <param name="settings">The settings.</param>
    public void Convert(FilePath file, DotNetProjectConvertSettings settings)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(settings);

        RunCommand(settings, GetArguments(file, settings));
    }

    private ProcessArgumentBuilder GetArguments(FilePath file, DotNetProjectConvertSettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("project");
        builder.Append("convert");
        builder.AppendQuoted(file.MakeAbsolute(_environment).FullPath);

        if (settings.DeleteSource)
        {
            builder.Append("--delete-source");
        }

        if (settings.DryRun)
        {
            builder.Append("--dry-run");
        }

        if (settings.Force)
        {
            builder.Append("--force");
        }

        if (settings.Interactive)
        {
            builder.Append("--interactive");
        }

        if (settings.Output != null)
        {
            builder.AppendSwitchQuoted("--output", settings.Output.MakeAbsolute(_environment).FullPath);
        }

        return builder;
    }
}
