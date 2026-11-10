// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using Cake.Core.Polyfill;

namespace Cake.Cli;

/// <summary>
/// Represents a version resolver.
/// </summary>
public interface IVersionResolver
{
    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns>The version.</returns>
    string GetVersion();

    /// <summary>
    /// Gets the product version.
    /// </summary>
    /// <returns>The product version.</returns>
    string GetProductVersion();
}

/// <summary>
/// The Cake version resolver.
/// </summary>
public sealed class VersionResolver : IVersionResolver
{
    /// <inheritdoc/>
    public string GetVersion()
    {
        var info = TryGetFileVersionInfo();
        if (info != null)
        {
            return NonEmptyOrUnknown(info.Comments);
        }

        return NonEmptyOrUnknown(GetInformationalVersion());
    }

    /// <inheritdoc/>
    public string GetProductVersion()
    {
        var info = TryGetFileVersionInfo();
        if (info != null)
        {
            return NonEmptyOrUnknown(info.ProductVersion);
        }

        return NonEmptyOrUnknown(GetInformationalVersion());
    }

    private static FileVersionInfo TryGetFileVersionInfo()
    {
        var assembly = Assembly.GetEntryAssembly();
        var filePath = AssemblyPathResolver.GetAssemblyFilePath(assembly);
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            return null;
        }

        return FileVersionInfo.GetVersionInfo(filePath);
    }

    private static string GetInformationalVersion()
    {
        return Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;
    }

    private static string NonEmptyOrUnknown(string version)
    {
        return string.IsNullOrWhiteSpace(version) ? "Unknown" : version;
    }
}
