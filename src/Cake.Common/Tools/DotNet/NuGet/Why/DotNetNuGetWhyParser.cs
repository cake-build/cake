// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cake.Common.Tools.DotNet.NuGet.Why;

/// <summary>
/// Parses English <c>dotnet nuget why</c> tree output.
/// </summary>
public static partial class DotNetNuGetWhyParser
{
    private static readonly string[] TreePrefixes =
    [
        "└── ",
        "├── ",
        "|-- ",
        "`-- ",
        "+-- "
    ];

    [GeneratedRegex(@"\x1B\[[0-9;?]*[ -/]*[@-~]", RegexOptions.CultureInvariant)]
    private static partial Regex AnsiEscape();

    [GeneratedRegex(@"^Project '(?<name>[^']+)' has the following dependency graph\(s\) for '(?<package>[^']+)':\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex ProjectGraphHeader();

    [GeneratedRegex(@"^Project '(?<name>[^']+)' does not have a dependency on '(?<package>[^']+)'\.?\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex ProjectMissingHeader();

    [GeneratedRegex(@"^\[(?<framework>[^\]]+)\]\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex FrameworkHeader();

    /// <summary>
    /// Parses redirected standard output from <c>dotnet nuget why</c>.
    /// </summary>
    /// <param name="lines">The output lines.</param>
    /// <returns>The parsed dependency graphs.</returns>
    public static DotNetNuGetWhyResult Parse(IEnumerable<string> lines)
    {
        var projects = new List<DotNetNuGetWhyProject>();
        if (lines == null)
        {
            return new DotNetNuGetWhyResult(projects);
        }

        ProjectBuilder currentProject = null;
        GraphBuilder currentGraph = null;
        var stack = new List<(int Indent, PackageBuilder Package)>();
        string pending = null;

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            var line = AnsiEscape().Replace(raw, string.Empty).TrimEnd();
            if (pending != null)
            {
                line = string.Concat(pending, " ", line.TrimStart());
                pending = null;
            }

            if (LooksLikeIncompleteProjectHeader(line))
            {
                pending = line;
                continue;
            }

            if (TryAddProjectHeader(line, projects, ref currentProject, ref currentGraph, stack))
            {
                continue;
            }

            var missingMatch = ProjectMissingHeader().Match(line);
            if (missingMatch.Success)
            {
                FlushProject(projects, ref currentProject, ref currentGraph);
                projects.Add(new DotNetNuGetWhyProject(
                    missingMatch.Groups["name"].Value,
                    missingMatch.Groups["package"].Value,
                    []));
                stack.Clear();
                continue;
            }

            var trimmed = line.Trim();
            var frameworkMatch = FrameworkHeader().Match(trimmed);
            if (frameworkMatch.Success)
            {
                if (currentProject == null)
                {
                    continue;
                }

                FlushGraph(currentProject, ref currentGraph);
                currentGraph = new GraphBuilder(frameworkMatch.Groups["framework"].Value);
                stack.Clear();
                continue;
            }

            if (!TryGetTreePackage(line, out var treeIndex, out var packageText))
            {
                continue;
            }

            if (currentProject == null)
            {
                continue;
            }

            currentGraph ??= new GraphBuilder(null);
            if (!TryParsePackage(packageText, out var package))
            {
                continue;
            }

            while (stack.Count > 0 && stack[^1].Indent >= treeIndex)
            {
                stack.RemoveAt(stack.Count - 1);
            }

            if (stack.Count == 0)
            {
                currentGraph.Roots.Add(package);
            }
            else
            {
                stack[^1].Package.Dependencies.Add(package);
            }

            stack.Add((treeIndex, package));
        }

        if (pending != null)
        {
            TryAddProjectHeader(pending, projects, ref currentProject, ref currentGraph, stack);
        }

        FlushProject(projects, ref currentProject, ref currentGraph);
        return new DotNetNuGetWhyResult(projects);
    }

    private static bool LooksLikeIncompleteProjectHeader(string line)
    {
        return line.StartsWith("Project '", StringComparison.Ordinal) &&
               !ProjectGraphHeader().IsMatch(line) &&
               !ProjectMissingHeader().IsMatch(line);
    }

    private static bool TryAddProjectHeader(
        string line,
        List<DotNetNuGetWhyProject> projects,
        ref ProjectBuilder currentProject,
        ref GraphBuilder currentGraph,
        List<(int Indent, PackageBuilder Package)> stack)
    {
        var graphMatch = ProjectGraphHeader().Match(line);
        if (!graphMatch.Success)
        {
            return false;
        }

        FlushProject(projects, ref currentProject, ref currentGraph);
        currentProject = new ProjectBuilder(graphMatch.Groups["name"].Value, graphMatch.Groups["package"].Value);
        stack.Clear();
        return true;
    }

    private static bool TryGetTreePackage(string line, out int indent, out string packageText)
    {
        indent = -1;
        packageText = null;

        var prefixIndex = -1;
        var prefixLength = 0;
        foreach (var prefix in TreePrefixes)
        {
            var index = line.IndexOf(prefix, StringComparison.Ordinal);
            if (index < 0 || (prefixIndex >= 0 && index >= prefixIndex))
            {
                continue;
            }

            prefixIndex = index;
            prefixLength = prefix.Length;
        }

        if (prefixIndex >= 0)
        {
            indent = prefixIndex;
            packageText = line[(prefixIndex + prefixLength)..].Trim();
            return !string.IsNullOrWhiteSpace(packageText);
        }

        return false;
    }

    private static bool TryParsePackage(string text, out PackageBuilder package)
    {
        package = null;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var at = text.LastIndexOf('@');
        if (at > 0 && at < text.Length - 1)
        {
            return TryCreatePackage(text[..at], text[(at + 1)..], out package);
        }

        var legacy = text.LastIndexOf(" (v", StringComparison.Ordinal);
        if (legacy > 0 && text.EndsWith(')'))
        {
            return TryCreatePackage(text[..legacy], text[(legacy + 3)..^1], out package);
        }

        return false;
    }

    private static bool TryCreatePackage(string id, string remainder, out PackageBuilder package)
    {
        package = null;
        id = id.Trim();
        remainder = remainder.Trim();

        string version;
        string constraint = null;
        var constraintStart = remainder.IndexOf('(');
        if (constraintStart >= 0)
        {
            version = remainder[..constraintStart].Trim();
            var constraintEnd = remainder.LastIndexOf(')');
            if (constraintEnd > constraintStart)
            {
                constraint = remainder[(constraintStart + 1)..constraintEnd].Trim();
            }
        }
        else
        {
            version = remainder;
        }

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(version))
        {
            return false;
        }

        package = new PackageBuilder(id, version, constraint);
        return true;
    }

    private static void FlushProject(
        List<DotNetNuGetWhyProject> projects,
        ref ProjectBuilder currentProject,
        ref GraphBuilder currentGraph)
    {
        if (currentProject == null)
        {
            return;
        }

        FlushGraph(currentProject, ref currentGraph);
        projects.Add(currentProject.Build());
        currentProject = null;
    }

    private static void FlushGraph(ProjectBuilder currentProject, ref GraphBuilder currentGraph)
    {
        if (currentGraph == null)
        {
            return;
        }

        currentProject.Graphs.Add(currentGraph.Build());
        currentGraph = null;
    }

    private sealed class ProjectBuilder(string name, string package)
    {
        public List<DotNetNuGetWhyGraph> Graphs { get; } = [];

        public DotNetNuGetWhyProject Build() => new(name, package, Graphs);
    }

    private sealed class GraphBuilder(string framework)
    {
        public List<PackageBuilder> Roots { get; } = [];

        public DotNetNuGetWhyGraph Build() =>
            new(framework, Roots.ConvertAll(root => root.Build()));
    }

    private sealed class PackageBuilder(string id, string version, string versionConstraint)
    {
        public List<PackageBuilder> Dependencies { get; } = [];

        public DotNetNuGetWhyPackage Build() =>
            new(id, version, versionConstraint, Dependencies.ConvertAll(child => child.Build()));
    }
}
