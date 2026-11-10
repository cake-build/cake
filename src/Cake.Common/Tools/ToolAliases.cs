// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;

namespace Cake.Common.Tools;

/// <summary>
/// Contains functionality for installing NuGet and .NET tools on demand.
/// </summary>
/// <para>
/// Tools installed through these aliases are registered with the tool locator,
/// the same way as tools installed via the <c>#tool</c> preprocessor directive.
/// Use this when a tool is only needed inside a specific task.
/// </para>
/// <seealso href="https://github.com/cake-build/cake/issues/2471"/>
/// <seealso href="https://github.com/cake-build/generator/issues/201"/>
[CakeAliasCategory("Tools")]
public static class ToolAliases
{
    /// <summary>
    /// Gets the tool installer instance.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The tool installer.</returns>
    /// <example>
    /// <code>
    /// var paths = ToolInstaller.Install(new PackageReference("nuget:?package=xunit.runner.console&amp;version=2.9.3"));
    /// </code>
    /// </example>
    [CakePropertyAlias(Cache = true)]
    [CakeNamespaceImport("Cake.Core.Packaging")]
    [CakeNamespaceImport("Cake.Core.Tooling")]
    public static IToolInstaller ToolInstaller(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ToolInstaller;
    }

    /// <summary>
    /// Installs a tool using the specified package reference.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tool">The package reference for the tool to install.</param>
    /// <returns>An array of file paths where the tool was installed.</returns>
    /// <example>
    /// <code>
    /// Task("Load-JMeter")
    ///     .Does(() =>
    /// {
    ///     InstallTool(new PackageReference("nuget:?package=JMeter&amp;version=5.6.3"));
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeNamespaceImport("Cake.Core.Packaging")]
    public static FilePath[] InstallTool(this ICakeContext context, PackageReference tool)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tool);

        return [.. context.ToolInstaller.Install(tool)];
    }

    /// <summary>
    /// Installs a tool using the specified tool string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tool">The tool string to install.</param>
    /// <returns>An array of file paths where the tool was installed.</returns>
    /// <example>
    /// <code>
    /// Task("Load-JMeter")
    ///     .Does(() =>
    /// {
    ///     InstallTool("nuget:?package=JMeter&amp;version=5.6.3");
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeNamespaceImport("Cake.Core.Packaging")]
    public static FilePath[] InstallTool(this ICakeContext context, string tool)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tool);

        return context.InstallTool(new PackageReference(tool));
    }

    /// <summary>
    /// Installs multiple tools using the specified package references.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tools">The package references for the tools to install.</param>
    /// <returns>An array of tuples containing the package reference and installed file paths for each tool.</returns>
    /// <example>
    /// <code>
    /// Task("Restore-Tools")
    ///     .Does(() =>
    /// {
    ///     InstallTools(
    ///         new PackageReference("nuget:?package=xunit.runner.console&amp;version=2.9.3"),
    ///         new PackageReference("dotnet:?package=GitVersion.Tool&amp;version=6.8.2"));
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeNamespaceImport("Cake.Core.Packaging")]
    public static (PackageReference Tool, FilePath[] Paths)[] InstallTools(this ICakeContext context, params PackageReference[] tools)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tools);

        return [.. tools.Select(tool => (tool, context.InstallTool(tool)))];
    }

    /// <summary>
    /// Installs multiple tools using the specified tool strings.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tools">The tool strings to install.</param>
    /// <returns>An array of tuples containing the package reference and installed file paths for each tool.</returns>
    /// <example>
    /// <code>
    /// Task("Restore-Tools")
    ///     .Does(() =>
    /// {
    ///     InstallTools(
    ///         "nuget:?package=xunit.runner.console&amp;version=2.9.3",
    ///         "dotnet:?package=GitVersion.Tool&amp;version=6.8.2");
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeNamespaceImport("Cake.Core.Packaging")]
    public static (PackageReference Tool, FilePath[] Paths)[] InstallTools(this ICakeContext context, params string[] tools)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tools);

        return [.. tools.Select(tool => (new PackageReference(tool), context.InstallTool(tool)))];
    }
}
