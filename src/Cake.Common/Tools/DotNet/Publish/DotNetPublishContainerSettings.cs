// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Contains settings for publishing a .NET project as a container image using the .NET SDK <c>PublishContainer</c> target.
/// When set on <see cref="DotNetPublishSettings.Container"/>, the <c>PublishContainer</c> target is invoked and the
/// container MSBuild properties are passed, taking precedence over the same properties set via <see cref="DotNetPublishSettings.MSBuildSettings"/>.
/// </summary>
/// <remarks>
/// Container items, i.e. ports, labels and environment variables, can't be set from the command line and need to be specified in the project.
/// </remarks>
/// <example>
/// <para>Archive.</para>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings
/// {
///     Container = new DotNetPublishContainerSettings
///     {
///         Repository = "cake-it-hwapp",
///         ImageTags = ["1.0.0", "latest"]
///     }
/// }.WithContainerArchive("./artifacts/hwapp.tar.gz"));
/// </code>
/// <para>Local registry.</para>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
///     .WithContainerLocalRegistry(DotNetContainerLocalRegistry.Podman, "cake-it-hwapp", "test"));
/// </code>
/// <para>Registry.</para>
/// <code>
/// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
///     .WithContainerRegistry("localhost:5000", "cake-it/hwapp", "test"));
/// </code>
/// </example>
public sealed record DotNetPublishContainerSettings
{
    /// <summary>
    /// Gets the base image, i.e. <c>mcr.microsoft.com/dotnet/runtime:10.0</c>.
    /// If not specified, the SDK infers it from the project.
    /// </summary>
    public string BaseImage { get; init; }

    /// <summary>
    /// Gets the base image family, i.e. <c>noble-chiseled</c>, appended to the inferred base image tag.
    /// Ignored when <see cref="BaseImage"/> is set.
    /// </summary>
    public string Family { get; init; }

    /// <summary>
    /// Gets the registry the image is pushed to, i.e. <c>ghcr.io</c>.
    /// </summary>
    public string Registry { get; init; }

    /// <summary>
    /// Gets the repository (image name), i.e. <c>devlead/cake-sdk</c>.
    /// If not specified, the SDK uses the assembly name.
    /// </summary>
    public string Repository { get; init; }

    /// <summary>
    /// Gets the image tag, i.e. <c>latest</c>.
    /// Can't be combined with <see cref="ImageTags"/>.
    /// </summary>
    public string ImageTag { get; init; }

    /// <summary>
    /// Gets the image tags, i.e. <c>1.0.0</c> and <c>latest</c>.
    /// Can't be combined with <see cref="ImageTag"/>.
    /// </summary>
    public ICollection<string> ImageTags { get; init; }

    /// <summary>
    /// Gets the user the container runs as, i.e. <c>root</c>.
    /// </summary>
    public string User { get; init; }

    /// <summary>
    /// Gets the working directory in the container, i.e. <c>/app/</c>.
    /// </summary>
    public string WorkingDirectory { get; init; }

    /// <summary>
    /// Gets the path of the <c>.tar.gz</c> archive the image is written to, instead of pushing or loading it.
    /// </summary>
    public FilePath ArchiveOutputPath { get; init; }

    /// <summary>
    /// Gets the local registry (container engine) the image is loaded into.
    /// If not specified, the SDK detects an available engine.
    /// </summary>
    public DotNetContainerLocalRegistry? LocalRegistry { get; init; }

    /// <summary>
    /// Gets the runtime identifier of the container image, i.e. <c>linux-x64</c>.
    /// Normally inferred from the publish runtime identifier.
    /// </summary>
    public string RuntimeIdentifier { get; init; }

    /// <summary>
    /// Gets the runtime identifiers of a multi-architecture container image, i.e. <c>linux-x64</c> and <c>linux-arm64</c>.
    /// </summary>
    public ICollection<string> RuntimeIdentifiers { get; init; }

    /// <summary>
    /// Gets the image manifest format.
    /// If not specified, it's inherited from the base image.
    /// </summary>
    public DotNetContainerImageFormat? ImageFormat { get; init; }

    /// <summary>
    /// Gets how the app command is set in the image.
    /// </summary>
    public DotNetContainerAppCommandInstruction? AppCommandInstruction { get; init; }

    /// <summary>
    /// Gets a value indicating whether default image labels are generated.
    /// </summary>
    public bool? GenerateLabels { get; init; }

    /// <summary>
    /// Gets a value indicating whether multi-architecture images are published in parallel.
    /// </summary>
    public bool? PublishInParallel { get; init; }

    /// <summary>
    /// Gets a value indicating whether layers are pushed without checking if they already exist in the registry.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public bool? PushNoCache { get; init; }

    /// <summary>
    /// Gets the target operating system, passed as <c>--os</c>.
    /// Takes precedence over <see cref="DotNetPublishSettings.OS"/>.
    /// </summary>
    public DotNetContainerOS? OS { get; init; }

    /// <summary>
    /// Gets the target architecture, passed as <c>--arch</c>.
    /// </summary>
    public DotNetContainerArchitecture? Arch { get; init; }
}
