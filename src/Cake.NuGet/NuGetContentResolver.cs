// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;

namespace Cake.NuGet;

internal sealed class NuGetContentResolver : INuGetContentResolver
{
    private readonly IFileSystem _fileSystem;
    private readonly ICakeEnvironment _environment;
    private readonly IGlobber _globber;
    private readonly ICakeLog _log;
    private readonly ICakeConfiguration _configuration;
    private readonly Lazy<RuntimeGraph> _runtimeGraph;

    internal string RuntimeIdentifierOverride { get; set; }

    public NuGetContentResolver(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IGlobber globber,
        ICakeLog log,
        ICakeConfiguration configuration)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _globber = globber ?? throw new ArgumentNullException(nameof(globber));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _runtimeGraph = new Lazy<RuntimeGraph>(LoadRuntimeGraph);
    }

    private RuntimeGraph LoadRuntimeGraph()
    {
        if (_configuration.GetBoolValue(Constants.NuGet.UseLegacyRidGraph))
        {
            var location = typeof(NuGetContentResolver).Assembly.Location;
            if (!string.IsNullOrEmpty(location))
            {
                var sidecar = _fileSystem.GetFile(
                    new FilePath(location).GetDirectory().CombineWithFilePath("runtime.json"));
                if (sidecar.Exists)
                {
                    using var fileStream = sidecar.OpenRead();
                    return JsonRuntimeFormat.ReadRuntimeGraph(fileStream);
                }
            }
        }

        var assembly = typeof(NuGetContentResolver).Assembly;
        var resourceName = $"{assembly.GetName().Name}.PortableRuntimeIdentifierGraph.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Missing embedded RID graph '{resourceName}'.");
        return JsonRuntimeFormat.ReadRuntimeGraph(stream);
    }

    public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (type == PackageType.Addin || type == PackageType.Module)
        {
            return GetAddinAssemblies(path, package);
        }
        if (type == PackageType.Tool)
        {
            return GetToolFiles(path, package);
        }

        throw new InvalidOperationException("Unknown resource type.");
    }

    private IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path, PackageReference package)
    {
        if (!_fileSystem.Exist(path))
        {
            _log.Debug("Path not found at {0}.", path);
            return Array.Empty<IFile>();
        }

        // Get current framework.
        var tfm = NuGetFramework.Parse(_environment.Runtime.BuiltFramework.FullName, DefaultFrameworkNameProvider.Instance);

        // Get current runtime identifier.
        var rid = _environment.Runtime.IsCoreClr
            ? RuntimeIdentifierOverride ?? System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier
            : null;

        // Get all candidate files.
        var pathComparer = PathComparer.Default;

        var assemblies = GetFiles(path, package, [path.FullPath + "/**/*.{dll,so,dylib}"])
            .Where(file => !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => path.GetRelativePath(x.Path).FullPath);
        if (assemblies.Count == 0)
        {
            _log.Debug("Assemblies not found at {0}.", path);
        }

        var conventions = new ManagedCodeConventions(_runtimeGraph.Value);
        var collection = new ContentItemCollection();
        collection.Load(assemblies.Keys);
        var criteria = conventions.Criteria.ForFrameworkAndRuntime(tfm, rid);

        var managedAssemblies = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

        var files = managedAssemblies?.Items.Select(x => assemblies[x.Path]).Where(x => x.IsClrAssembly()).ToArray() ?? [];
        if (_environment.Runtime.IsCoreClr)
        {
            var nativeAssemblies = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            files = [.. (nativeAssemblies?.Items.Select(x => assemblies[x.Path]) ?? Array.Empty<IFile>()), .. files];
        }
        if (files.Length == 0)
        {
            _log.Debug("Assemblies not found for tfm {0} and rid {1}.", tfm, rid);
        }

        return files;
    }

    private IReadOnlyCollection<IFile> GetToolFiles(DirectoryPath path, PackageReference package)
    {
        var result = new List<IFile>();
        var toolDirectory = _fileSystem.GetDirectory(path);
        if (toolDirectory.Exists)
        {
            result.AddRange(GetFiles(path, package));
        }
        return result;
    }

    private IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference package, string[] patterns = null)
    {
        var collection = new FilePathCollection(new PathComparer(_environment));

        // Get default files (exe and dll).

        patterns = patterns ?? [path.FullPath + "/**/*.exe", path.FullPath + "/**/*.dll"];
        foreach (var pattern in patterns)
        {
            collection.Add(_globber.GetFiles(pattern));
        }

        // Include files.
        if (package.Parameters.TryGetValue("include", out var includes))
        {
            foreach (var include in includes)
            {
                var includePath = string.Concat(path.FullPath, "/", include.TrimStart('/'));
                collection.Add(_globber.GetFiles(includePath));
            }
        }

        // Exclude files.
        if (package.Parameters.TryGetValue("exclude", out var excludes))
        {
            foreach (var exclude in excludes)
            {
                var excludePath = string.Concat(path.FullPath, "/", exclude.TrimStart('/'));
                collection.Remove(_globber.GetFiles(excludePath));
            }
        }

        // Return the files.
        return collection.Select(p => _fileSystem.GetFile(p)).ToArray();
    }
}
