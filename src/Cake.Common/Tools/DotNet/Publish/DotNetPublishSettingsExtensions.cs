// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Contains functionality related to .NET publish settings.
/// </summary>
public static class DotNetPublishSettingsExtensions
{
    /// <summary>
    /// Publishes the project as a container image written to a <c>.tar.gz</c> archive.
    /// Creates <see cref="DotNetPublishSettings.Container"/> if not set, otherwise keeps its other settings,
    /// and clears any local registry or registry destination.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="archiveOutputPath">The path of the <c>.tar.gz</c> archive.</param>
    /// <param name="baseImage">The optional base image, i.e. <c>mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot</c>, replaces any base image when set.</param>
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
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
    /// </example>
    public static DotNetPublishSettings WithContainerArchive(this DotNetPublishSettings settings, FilePath archiveOutputPath, string baseImage = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(archiveOutputPath);

        settings.Container = (
                                (settings.Container ??= new()) with
                                {
                                    ArchiveOutputPath = archiveOutputPath,
                                    LocalRegistry = null,
                                    Registry = null
                                }
                            )
                                .WithBaseImage(baseImage);

        return settings;
    }

    /// <summary>
    /// Publishes the project as a container image loaded into a local registry (container engine), i.e. Docker or Podman.
    /// Creates <see cref="DotNetPublishSettings.Container"/> if not set, otherwise keeps its other settings,
    /// and clears any archive or registry destination.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="localRegistry">The local registry.</param>
    /// <param name="repository">The repository (image name).</param>
    /// <param name="imageTag">The optional image tag, replaces any image tags when set.</param>
    /// <param name="baseImage">The optional base image, i.e. <c>mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot</c>, replaces any base image when set.</param>
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
    ///     .WithContainerLocalRegistry(DotNetContainerLocalRegistry.Podman, "cake-it-hwapp", "test"));
    /// </code>
    /// </example>
    public static DotNetPublishSettings WithContainerLocalRegistry(this DotNetPublishSettings settings, DotNetContainerLocalRegistry localRegistry, string repository, string imageTag = null, string baseImage = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(localRegistry.Value, nameof(localRegistry));
        ArgumentException.ThrowIfNullOrWhiteSpace(repository);

        settings.Container = (
                                (settings.Container ??= new()) with
                                {
                                    LocalRegistry = localRegistry,
                                    Repository = repository,
                                    ArchiveOutputPath = null,
                                    Registry = null
                                }
                            )
                                .WithBaseImage(baseImage)
                                .WithImageTag(imageTag);

        return settings;
    }

    /// <summary>
    /// Publishes the project as a container image pushed to a registry, i.e. <c>ghcr.io</c>.
    /// Creates <see cref="DotNetPublishSettings.Container"/> if not set, otherwise keeps its other settings,
    /// and clears any archive or local registry destination.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="registry">The registry.</param>
    /// <param name="repository">The repository (image name).</param>
    /// <param name="imageTag">The optional image tag, replaces any image tags when set.</param>
    /// <param name="baseImage">The optional base image, i.e. <c>mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot</c>, replaces any base image when set.</param>
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
    ///     .WithContainerRegistry("localhost:5000", "cake-it/hwapp", "test"));
    /// </code>
    /// </example>
    public static DotNetPublishSettings WithContainerRegistry(this DotNetPublishSettings settings, string registry, string repository, string imageTag = null, string baseImage = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(registry);
        ArgumentException.ThrowIfNullOrWhiteSpace(repository);

        settings.Container = (
                                (settings.Container ??= new()) with
                                {
                                    Registry = registry,
                                    Repository = repository,
                                    ArchiveOutputPath = null,
                                    LocalRegistry = null
                                }
                            )
                                .WithBaseImage(baseImage)
                                .WithImageTag(imageTag);

        return settings;
    }

    /// <summary>
    /// Sets the base image the container image is built from.
    /// Creates <see cref="DotNetPublishSettings.Container"/> if not set, otherwise keeps its other settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="baseImage">The base image, i.e. <c>mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot</c>.</param>
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
    ///     .WithContainerBaseImage("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot")
    ///     .WithContainerRegistry("localhost:5000", "cake-it/hwapp", "test"));
    /// </code>
    /// </example>
    public static DotNetPublishSettings WithContainerBaseImage(this DotNetPublishSettings settings, string baseImage)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseImage);

        settings.Container = (settings.Container ??= new()).WithBaseImage(baseImage);

        return settings;
    }

    private static DotNetPublishContainerSettings WithImageTag(this DotNetPublishContainerSettings container, string imageTag)
    {
        return string.IsNullOrWhiteSpace(imageTag)
                ? container
                : container with { ImageTag = imageTag, ImageTags = null };
    }

    private static DotNetPublishContainerSettings WithBaseImage(this DotNetPublishContainerSettings container, string baseImage)
    {
        return string.IsNullOrWhiteSpace(baseImage)
                ? container
                : container with { BaseImage = baseImage };
    }

}
