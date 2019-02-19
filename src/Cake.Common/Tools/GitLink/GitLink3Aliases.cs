// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gittools/gitlink">GitLink</see> version 3.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="GitLink3Settings" /> class:
    /// <code>
    /// #tool "nuget:?package=gitlink"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("GitLink v3")]
    public static class GitLink3Aliases
    {
        /// <summary>
        /// Update the pdb file to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pdbFilePath">The PDB File to analyze.</param>
        /// <example>
        /// <code>
        /// GitLink3("C:/temp/solution/bin/my.pdb");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void GitLink3(this ICakeContext context, FilePath pdbFilePath)
        {
            GitLink3(context, pdbFilePath, new GitLink3Settings());
        }

        /// <summary>
        /// Update the pdb file to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pdbFilePath">The PDB File to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitLink3("C:/temp/solution/bin/my.pdb", new GitLink3Settings {
        ///     RepositoryUrl = "http://mydomain.com",
        ///     ShaHash       = "abcdef"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void GitLink3(this ICakeContext context, FilePath pdbFilePath, GitLink3Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new GitLink3Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(pdbFilePath, settings);
        }

        /// <summary>
        /// Update the pdb files to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pdbFiles">The PDB File collection to analyze.</param>
        /// <example>
        /// <code>
        /// GitLink3("C:/temp/solution/bin/**/*.pdb");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void GitLink3(this ICakeContext context, IEnumerable<FilePath> pdbFiles)
        {
            GitLink3(context, pdbFiles, new GitLink3Settings());
        }

        /// <summary>
        /// Update the pdb files to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pdbFiles">The PDB File collection to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitLink3("C:/temp/solution/bin/**/*.pdb", new GitLink3Settings {
        ///     RepositoryUrl = "http://mydomain.com",
        ///     ShaHash       = "abcdef"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void GitLink3(this ICakeContext context, IEnumerable<FilePath> pdbFiles, GitLink3Settings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new GitLink3Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(pdbFiles, settings);
        }
    }
}