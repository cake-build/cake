// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Project.Convert;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet;

/// <summary>
/// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET CLI</see>.</para>
/// <para>
/// In order to use the commands for this alias, the .NET CLI tools will need to be installed on the machine where
/// the Cake script is being executed.  See this <see href="https://www.microsoft.com/net/core">page</see> for information
/// on how to install.
/// </para>
/// </summary>
public static partial class DotNetAliases
{
    /// <summary>
    /// Converts a file-based program to a project-based program.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="file">The file-based program to convert.</param>
    /// <example>
    /// <code>
    /// // cake.cs:
    /// // #:sdk Cake.Sdk
    /// //
    /// // Information("Hello world!");
    ///
    /// DotNetProjectConvert("cake.cs");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Project")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Project.Convert")]
    public static void DotNetProjectConvert(this ICakeContext context, FilePath file)
    {
        context.DotNetProjectConvert(file, null);
    }

    /// <summary>
    /// Converts a file-based program to a project-based program.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="file">The file-based program to convert.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// // cake.cs:
    /// // #:sdk Cake.Sdk
    /// //
    /// // Information("Hello world!");
    ///
    /// DotNetProjectConvert("cake.cs", new DotNetProjectConvertSettings {
    ///     Output = "./cake"
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Project")]
    [CakeNamespaceImport("Cake.Common.Tools.DotNet.Project.Convert")]
    public static void DotNetProjectConvert(this ICakeContext context, FilePath file, DotNetProjectConvertSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        settings ??= new DotNetProjectConvertSettings();

        var converter = new DotNetProjectConverter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        converter.Convert(file, settings);
    }
}
