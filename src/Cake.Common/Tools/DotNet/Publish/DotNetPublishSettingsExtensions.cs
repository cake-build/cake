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
    public static DotNetPublishSettings WithContainerArchive(this DotNetPublishSettings settings, FilePath archiveOutputPath)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(archiveOutputPath);

        settings.Container = (settings.Container ?? new DotNetPublishContainerSettings()) with
        {
            ArchiveOutputPath = archiveOutputPath,
            LocalRegistry = null,
            Registry = null
        };

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
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
    ///     .WithContainerLocalRegistry(DotNetContainerLocalRegistry.Podman, "cake-it-hwapp", "test"));
    /// </code>
    /// </example>
    public static DotNetPublishSettings WithContainerLocalRegistry(this DotNetPublishSettings settings, DotNetContainerLocalRegistry localRegistry, string repository, string imageTag = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(localRegistry.Value, nameof(localRegistry));
        ArgumentException.ThrowIfNullOrWhiteSpace(repository);

        settings.Container = WithImageTag(settings.Container ?? new DotNetPublishContainerSettings(), imageTag) with
        {
            LocalRegistry = localRegistry,
            Repository = repository,
            ArchiveOutputPath = null,
            Registry = null
        };

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
    /// <returns>The same <see cref="DotNetPublishSettings"/> instance so that multiple calls can be chained.</returns>
    /// <example>
    /// <code>
    /// DotNetPublish("./src/hwapp/hwapp.csproj", new DotNetPublishSettings()
    ///     .WithContainerRegistry("localhost:5000", "cake-it/hwapp", "test"));
    /// </code>
    /// </example>
    public static DotNetPublishSettings WithContainerRegistry(this DotNetPublishSettings settings, string registry, string repository, string imageTag = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(registry);
        ArgumentException.ThrowIfNullOrWhiteSpace(repository);

        settings.Container = WithImageTag(settings.Container ?? new DotNetPublishContainerSettings(), imageTag) with
        {
            Registry = registry,
            Repository = repository,
            ArchiveOutputPath = null,
            LocalRegistry = null
        };

        return settings;
    }

    private static DotNetPublishContainerSettings WithImageTag(DotNetPublishContainerSettings container, string imageTag)
        => string.IsNullOrWhiteSpace(imageTag)
            ? container
            : container with { ImageTag = imageTag, ImageTags = null };
}
