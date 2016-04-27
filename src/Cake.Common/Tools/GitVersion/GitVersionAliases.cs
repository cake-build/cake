using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Contains functionality related to GitVersion.
    /// <see href="http://gitversion.readthedocs.org/en/latest/">GitVersion Documentation</see>
    /// <see href="https://www.nuget.org/packages/GitVersion.CommandLine/">GitVersion NuGet Package</see>
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
