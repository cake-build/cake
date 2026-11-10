// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// .NET project publisher.
/// </summary>
public sealed class DotNetPublisher : DotNetTool<DotNetPublishSettings>
{
    private readonly ICakeEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetPublisher" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    public DotNetPublisher(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
    {
        _environment = environment;
    }

    /// <summary>
    /// Publish the project using the specified path and settings.
    /// </summary>
    /// <param name="path">The target file path.</param>
    /// <param name="settings">The settings.</param>
    public void Publish(string path, DotNetPublishSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        RunCommand(settings, GetArguments(path, settings));
    }

    private ProcessArgumentBuilder GetArguments(string path, DotNetPublishSettings settings)
    {
        var builder = CreateArgumentBuilder(settings);

        builder.Append("publish");

        // Specific path?
        if (path != null)
        {
            builder.AppendQuoted(path);
        }

        // Output directory
        if (settings.OutputDirectory != null)
        {
            builder.Append("--output");
            builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
        }

        // Runtime
        if (!string.IsNullOrEmpty(settings.Runtime))
        {
            builder.Append("--runtime");
            builder.Append(settings.Runtime);
        }

        // Framework
        if (!string.IsNullOrEmpty(settings.Framework))
        {
            builder.Append("--framework");
            builder.Append(settings.Framework);
        }

        // Configuration
        if (!string.IsNullOrEmpty(settings.Configuration))
        {
            builder.Append("--configuration");
            builder.Append(settings.Configuration);
        }

        // Version suffix
        if (!string.IsNullOrEmpty(settings.VersionSuffix))
        {
            builder.Append("--version-suffix");
            builder.Append(settings.VersionSuffix);
        }

        // No Build
        if (settings.NoBuild)
        {
            builder.Append("--no-build");
        }

        // No Dependencies
        if (settings.NoDependencies)
        {
            builder.Append("--no-dependencies");
        }

        // No Restore
        if (settings.NoRestore)
        {
            builder.Append("--no-restore");
        }

        // No Logo
        if (settings.NoLogo)
        {
            builder.Append("--nologo");
        }

        // Force
        if (settings.Force)
        {
            builder.Append("--force");
        }

        // Self contained
        if (settings.SelfContained.HasValue)
        {
            if (settings.SelfContained.Value)
            {
                builder.Append("--self-contained");
            }
            else
            {
                builder.Append("--no-self-contained");
            }
        }

        // publish single file
        if (settings.PublishSingleFile.HasValue)
        {
            if (settings.PublishSingleFile.Value)
            {
                builder.Append("-p:PublishSingleFile=true");
            }
            else
            {
                builder.Append("-p:PublishSingleFile=false");
            }
        }

        // publish trimmed
        if (settings.PublishTrimmed.HasValue)
        {
            if (settings.PublishTrimmed.Value)
            {
                builder.Append("-p:PublishTrimmed=true");
            }
            else
            {
                builder.Append("-p:PublishTrimmed=false");
            }
        }

        // Tiered Compilation Quick Jit
        if (settings.TieredCompilationQuickJit.HasValue)
        {
            if (settings.TieredCompilationQuickJit.Value)
            {
                builder.Append("-p:TieredCompilationQuickJit=true");
            }
            else
            {
                builder.Append("-p:TieredCompilationQuickJit=false");
            }
        }

        // Tiered Compilation
        if (settings.TieredCompilation.HasValue)
        {
            if (settings.TieredCompilation.Value)
            {
                builder.Append("-p:TieredCompilation=true");
            }
            else
            {
                builder.Append("-p:TieredCompilation=false");
            }
        }

        // Publish ReadyToRun
        if (settings.PublishReadyToRun.HasValue)
        {
            if (settings.PublishReadyToRun.Value)
            {
                builder.Append("-p:PublishReadyToRun=true");
            }
            else
            {
                builder.Append("-p:PublishReadyToRun=false");
            }
        }

        // Publish ReadyToRunShowWarnings
        if (settings.PublishReadyToRunShowWarnings.HasValue)
        {
            if (settings.PublishReadyToRunShowWarnings.Value)
            {
                builder.Append("-p:PublishReadyToRunShowWarnings=true");
            }
            else
            {
                builder.Append("-p:PublishReadyToRunShowWarnings=false");
            }
        }

        // Include Native Libraries For Self-Extract
        if (settings.IncludeNativeLibrariesForSelfExtract.HasValue)
        {
            if (settings.IncludeNativeLibrariesForSelfExtract.Value)
            {
                builder.Append("-p:IncludeNativeLibrariesForSelfExtract=true");
            }
            else
            {
                builder.Append("-p:IncludeNativeLibrariesForSelfExtract=false");
            }
        }

        // Include All Content For Self-Extract
        if (settings.IncludeAllContentForSelfExtract.HasValue)
        {
            if (settings.IncludeAllContentForSelfExtract.Value)
            {
                builder.Append("-p:IncludeAllContentForSelfExtract=true");
            }
            else
            {
                builder.Append("-p:IncludeAllContentForSelfExtract=false");
            }
        }

        // Enable compression on the embedded assemblies
        if (settings.EnableCompressionInSingleFile.HasValue)
        {
            if (settings.EnableCompressionInSingleFile.Value)
            {
                builder.Append("-p:EnableCompressionInSingleFile=true");
            }
            else
            {
                builder.Append("-p:EnableCompressionInSingleFile=false");
            }
        }

        // Sources
        if (settings.Sources != null)
        {
            foreach (var source in settings.Sources)
            {
                builder.Append("--source");
                builder.AppendQuoted(source);
            }
        }

        // Os, container OS takes precedence
        var os = settings.Container?.OS?.Value is { Length: > 0 } containerOS ? containerOS : settings.OS;
        if (!string.IsNullOrEmpty(os))
        {
            builder.Append("--os");
            builder.Append(os);
        }

        // Arch
        if (settings.Container?.Arch?.Value is { Length: > 0 } arch)
        {
            builder.Append("--arch");
            builder.Append(arch);
        }

        if (settings.MSBuildSettings != null)
        {
            builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
        }

        // Container, appended last so it takes precedence over MSBuild settings
        if (settings.Container != null)
        {
            AppendContainerSettings(builder, settings.Container);
        }

        return builder;
    }

    private void AppendContainerSettings(ProcessArgumentBuilder builder, DotNetPublishContainerSettings container)
    {
        if (!string.IsNullOrEmpty(container.ImageTag) && container.ImageTags?.Count > 0)
        {
            throw new ArgumentException(
                $"Both {nameof(DotNetPublishContainerSettings.ImageTag)} and {nameof(DotNetPublishContainerSettings.ImageTags)} were provided, but only one or the other is allowed.",
                nameof(container));
        }

        builder.Append("-t:PublishContainer");

        AppendContainerProperty(builder, "ContainerBaseImage", container.BaseImage);
        AppendContainerProperty(builder, "ContainerFamily", container.Family);
        AppendContainerProperty(builder, "ContainerRegistry", container.Registry);
        AppendContainerProperty(builder, "ContainerRepository", container.Repository);
        AppendContainerProperty(builder, "ContainerImageTag", container.ImageTag);
        AppendContainerListProperty(builder, "ContainerImageTags", container.ImageTags);
        AppendContainerProperty(builder, "ContainerUser", container.User);
        AppendContainerProperty(builder, "ContainerWorkingDirectory", container.WorkingDirectory);
        AppendContainerProperty(builder, "ContainerArchiveOutputPath", container.ArchiveOutputPath?.MakeAbsolute(_environment).FullPath);
        AppendContainerProperty(builder, "LocalRegistry", container.LocalRegistry?.Value);
        AppendContainerProperty(builder, "ContainerRuntimeIdentifier", container.RuntimeIdentifier);
        AppendContainerListProperty(builder, "ContainerRuntimeIdentifiers", container.RuntimeIdentifiers);
        AppendContainerProperty(builder, "ContainerImageFormat", container.ImageFormat?.ToString());
        AppendContainerProperty(builder, "ContainerAppCommandInstruction", container.AppCommandInstruction?.ToString());
        AppendContainerProperty(builder, "ContainerGenerateLabels", container.GenerateLabels);
        AppendContainerProperty(builder, "ContainerPublishInParallel", container.PublishInParallel);
        AppendContainerProperty(builder, "ContainerPushNoCache", container.PushNoCache);
    }

    private static void AppendContainerProperty(ProcessArgumentBuilder builder, string name, bool? value)
    {
        if (value.HasValue)
        {
            AppendContainerProperty(builder, name, value.Value ? "true" : "false");
        }
    }

    private static void AppendContainerProperty(ProcessArgumentBuilder builder, string name, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            builder.Append($"-p:{name}={EscapeContainerPropertyValue(name, value)}");
        }
    }

    private static void AppendContainerListProperty(ProcessArgumentBuilder builder, string name, ICollection<string> values)
    {
        var escapedValues = values?
            .Where(value => !string.IsNullOrEmpty(value))
            .Select(value => EscapeContainerPropertyValue(name, value))
            .ToArray();

        if (escapedValues is { Length: > 0 })
        {
            // Semicolon separated lists need to reach MSBuild quoted, as an unquoted semicolon separates properties
            builder.Append($"-p:{name}=\\\"{string.Join(';', escapedValues)}\\\"");
        }
    }

    private static string EscapeContainerPropertyValue(string name, string value)
        => new KeyValuePair<string, ICollection<string>>(name, [value]).BuildMSBuildPropertyParameterString();
}
