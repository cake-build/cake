// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Polyfill;

/// <summary>
/// Resolves an assembly file or directory when <see cref="Assembly.Location"/> is empty
/// (single-file publish, Native AOT).
/// </summary>
internal static class AssemblyPathResolver
{
    public static string ResolveFilePath(string assemblyLocation, string processPath)
    {
        if (!string.IsNullOrWhiteSpace(assemblyLocation))
        {
            return assemblyLocation;
        }

        if (!string.IsNullOrWhiteSpace(processPath))
        {
            return processPath;
        }

        return null;
    }

    public static string ResolveDirectory(string assemblyLocation, string processPath, string baseDirectory)
    {
        var filePath = ResolveFilePath(assemblyLocation, processPath);
        if (!string.IsNullOrWhiteSpace(filePath))
        {
            return new FilePath(filePath).GetDirectory().FullPath;
        }

        return string.IsNullOrWhiteSpace(baseDirectory) ? null : baseDirectory;
    }

    public static string GetAssemblyFilePath(Assembly assembly)
    {
        return ResolveFilePath(GetLocation(assembly), Environment.ProcessPath);
    }

    public static DirectoryPath GetAssemblyDirectory(Assembly assembly, DirectoryPath fallbackDirectory)
    {
        var directory = ResolveDirectory(GetLocation(assembly), Environment.ProcessPath, fallbackDirectory?.FullPath);
        return directory != null ? new DirectoryPath(directory) : fallbackDirectory;
    }

    private static string GetLocation(Assembly assembly)
    {
        if (assembly is null)
        {
            return null;
        }

#pragma warning disable IL3000 // Avoid accessing Assembly file path when publishing as a single file
        return assembly.Location;
#pragma warning restore IL3000
    }
}
