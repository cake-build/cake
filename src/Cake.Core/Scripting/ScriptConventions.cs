// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Reflection;

namespace Cake.Core.Scripting;

/// <summary>
/// The script conventions used by Cake.
/// </summary>
public sealed class ScriptConventions : IScriptConventions
{
    private readonly IFileSystem _fileSystem;
    private readonly IAssemblyLoader _loader;
    private readonly ICakeRuntime _runtime;
    private readonly IReferenceAssemblyResolver _referenceAssemblyResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptConventions"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="loader">The assembly loader.</param>
    /// <param name="runtime">The Cake runtime.</param>
    /// <param name="referenceAssemblyResolver">The reference assembly resolver.</param>
    public ScriptConventions(IFileSystem fileSystem, IAssemblyLoader loader, ICakeRuntime runtime, IReferenceAssemblyResolver referenceAssemblyResolver)
    {
        _fileSystem = fileSystem;
        _loader = loader;
        _runtime = runtime;
        _referenceAssemblyResolver = referenceAssemblyResolver;
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetDefaultNamespaces()
    {
        return new List<string>
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "System.Threading.Tasks",
            "System.IO",
            "Cake.Core",
            "Cake.Core.IO",
            "Cake.Core.Diagnostics",
            "Cake.Core.Scripting",
            "Cake.Core.Tooling",
            "Microsoft.Extensions.DependencyInjection"
        };
    }

    /// <inheritdoc/>
    public IReadOnlyList<Assembly> GetDefaultAssemblies(DirectoryPath root)
    {
        // Prepare the default assemblies.
        var result = new HashSet<Assembly>(new SimpleAssemblyComparer());
        result.Add(typeof(Action).GetTypeInfo().Assembly); // mscorlib or System.Private.Core
        result.Add(typeof(IQueryable).GetTypeInfo().Assembly); // System.Core or System.Linq.Expressions
        result.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly); // Dynamic support

        result.AddRange(_referenceAssemblyResolver.GetReferenceAssemblies());

        // Load other Cake-related assemblies that we need.
        var cakeAssemblies = LoadCakeAssemblies(root);
        result.AddRange(cakeAssemblies);

        // Load all referenced assemblies.
        foreach (var cakeAssembly in cakeAssemblies)
        {
            foreach (var reference in cakeAssembly.GetReferencedAssemblies())
            {
                result.Add(_loader.Load(reference));
            }
        }

        // Return the assemblies.
        return result.ToArray();
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> GetDefaultDefines()
    {
        var defines = new List<string>
        {
            "#define CAKE"
        };

        var cakeMajor = _runtime.CakeVersion?.Major ?? 0;
        if (cakeMajor > 0)
        {
            defines.Add($"#define CAKE_{cakeMajor}");
        }

        for (var major = 7; major <= cakeMajor; major++)
        {
            defines.Add($"#define CAKE_{major}_OR_GREATER");
        }

        defines.Add(_runtime.IsCoreClr ? "#define NETCOREAPP" : "#define NETFRAMEWORK");
        defines.Add($"#define {GetFrameworkDefine()}");
        defines.AddRange(GetImpliedFrameworkDefines());
        return defines;
    }

    private string GetFrameworkDefine()
    {
        var framework = _runtime.BuiltFramework;
        if (string.Equals(framework.Identifier, ".NETCoreApp", StringComparison.OrdinalIgnoreCase))
        {
            return GetCoreAppDefine(framework.Version);
        }

        if (string.Equals(framework.Identifier, ".NETStandard", StringComparison.OrdinalIgnoreCase))
        {
            return $"NETSTANDARD{framework.Version.Major}_{framework.Version.Minor}";
        }

        throw new InvalidOperationException($"Unknown built framework '{framework.FullName}'.");
    }

    private IEnumerable<string> GetImpliedFrameworkDefines()
    {
        var framework = _runtime.BuiltFramework;
        if (!string.Equals(framework.Identifier, ".NETCoreApp", StringComparison.OrdinalIgnoreCase))
        {
            yield break;
        }

        var version = framework.Version;
        if (version.Major >= 5)
        {
            yield return "#define NET";
        }

        foreach (var (symbol, major, minor) in CoreAppVersions)
        {
            if (version.Major > major || (version.Major == major && version.Minor >= minor))
            {
                yield return $"#define {symbol}_OR_GREATER";
            }
        }

        if (!CoreAppVersions.Any(entry => entry.Major == version.Major && entry.Minor == version.Minor))
        {
            yield return $"#define {GetCoreAppDefine(version)}_OR_GREATER";
        }
    }

    private static string GetCoreAppDefine(Version version)
    {
        return version.Major >= 5
            ? $"NET{version.Major}_{version.Minor}"
            : $"NETCOREAPP{version.Major}_{version.Minor}";
    }

    private static readonly (string Symbol, int Major, int Minor)[] CoreAppVersions =
    [
        ("NETCOREAPP1_0", 1, 0),
        ("NETCOREAPP1_1", 1, 1),
        ("NETCOREAPP2_0", 2, 0),
        ("NETCOREAPP2_1", 2, 1),
        ("NETCOREAPP2_2", 2, 2),
        ("NETCOREAPP3_0", 3, 0),
        ("NETCOREAPP3_1", 3, 1),
        ("NET5_0", 5, 0),
        ("NET6_0", 6, 0),
        ("NET7_0", 7, 0),
        ("NET8_0", 8, 0),
        ("NET9_0", 9, 0),
        ("NET10_0", 10, 0),
        ("NET11_0", 11, 0)
    ];

    private List<Assembly> LoadCakeAssemblies(DirectoryPath root)
    {
        var result = new List<Assembly>();
        var assemblyDirectory = _fileSystem.GetDirectory(root);
        foreach (var pattern in GetCakeAssemblyNames())
        {
            var cakeAssemblies = assemblyDirectory.GetFiles(pattern, SearchScope.Current);
            foreach (var cakeAssembly in cakeAssemblies)
            {
                result.Add(_loader.Load(cakeAssembly.Path, false));
            }
        }
        return result;
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    private static string[] GetCakeAssemblyNames()
        =>
        [
            "Cake.Core.dll",
            "Cake.Common.dll",
            "Spectre.Console.dll",
            "Microsoft.Extensions.DependencyInjection.dll",
            "Microsoft.Extensions.DependencyInjection.Abstractions.dll"
        ];
}
