// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gittools/gitversion">GitVersion</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="GitVersionSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=GitVersion.CommandLine"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("GitVersion")]
    public static class GitVersionAliases
    {
        /// <summary>
        /// Retrives the GitVersion output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The git version info.</returns>
        /// <example>
        /// <para>Update the assembly info files for the project.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("UpdateAssemblyInfo")
        ///     .Does(() =>
        /// {
        ///     GitVersion(new GitVersionSettings {
        ///         UpdateAssemblyInfo = true
        ///     });
        /// });
        /// ]]>
        /// </code>
        /// <para>Get the git version info for the project using a dynamic repository.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("GetVersionInfo")
        ///     .Does(() =>
        /// {
        ///     var result = GitVersion(new GitVersionSettings {
        ///         UserName = "MyUser",
        ///         Password = "MyPassword,
        ///         Url = "http://git.myhost.com/myproject.git"
        ///         Branch = "develop"
        ///         Commit = EnviromentVariable("MY_COMMIT")
        ///     });
        ///     // Use result for building nuget packages, setting build server version, etc...
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static GitVersion GitVersion(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return GitVersion(context, new GitVersionSettings());
        }

        /// <summary>
        /// Retrives the GitVersion output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The GitVersion settings.</param>
        /// <returns>The git version info.</returns>
        /// <example>
        /// <para>Update the assembly info files for the project.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("UpdateAssemblyInfo")
        ///     .Does(() =>
        /// {
        ///     GitVersion(new GitVersionSettings {
        ///         UpdateAssemblyInfo = true
        ///     });
        /// });
        /// ]]>
        /// </code>
        /// <para>Get the git version info for the project using a dynamic repository.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("GetVersionInfo")
        ///     .Does(() =>
        /// {
        ///     var result = GitVersion(new GitVersionSettings {
        ///         UserName = "MyUser",
        ///         Password = "MyPassword,
        ///         Url = "http://git.myhost.com/myproject.git"
        ///         Branch = "develop"
        ///         Commit = EnviromentVariable("MY_COMMIT")
        ///     });
        ///     // Use result for building nuget packages, setting build server version, etc...
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static GitVersion GitVersion(this ICakeContext context, GitVersionSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var gitVersionRunner = new GitVersionRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            return gitVersionRunner.Run(settings);
        }
    }
}
