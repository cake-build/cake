// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gittools/gitlink">GitLink</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="GitLinkSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=gitlink"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("GitTools")]
    public static class GitLinkAliases
    {
        /// <summary>
        /// Update pdb files to link all sources.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryRootPath">The Solution File to analyze.</param>
        /// <example>
        /// <code>
        /// GitLink("C:/temp/solution");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("GitLink")]
        public static void GitLink(this ICakeContext context, DirectoryPath repositoryRootPath)
        {
            GitLink(context, repositoryRootPath, new GitLinkSettings());
        }

        /// <summary>
        /// Update pdb files to link all sources, using specified settings.
        /// This will allow anyone to step through the source code while debugging without a symbol source server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryRootPath">The path to the Root of the Repository to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitLink("C:/temp/solution", new GitLinkSettings {
        ///     RepositoryUrl = "http://mydomain.com",
        ///     Branch        = "master",
        ///     ShaHash       = "abcdef",
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("GitLink")]
        public static void GitLink(this ICakeContext context, DirectoryPath repositoryRootPath, GitLinkSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new GitLinkRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(repositoryRootPath, settings);
        }
    }
}
