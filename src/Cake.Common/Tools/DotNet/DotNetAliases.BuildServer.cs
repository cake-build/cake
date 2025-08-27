// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.BuildServer;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.DotNet
{
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
        /// Shuts down build servers that are started from dotnet.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetBuildServerShutdown();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build Server")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.BuildServer")]
        public static void DotNetBuildServerShutdown(this ICakeContext context)
        {
            context.DotNetBuildServerShutdown(null);
        }

        /// <summary>
        /// Shuts down build servers that are started from dotnet.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetBuildServerShutdownSettings
        /// {
        ///     MSBuild = true
        /// };
        ///
        /// DotNetBuildServerShutdown(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Build Server")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.BuildServer")]
        public static void DotNetBuildServerShutdown(this ICakeContext context, DotNetBuildServerShutdownSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var buildServer = new DotNetBuildServer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            buildServer.Shutdown(settings ?? new DotNetBuildServerShutdownSettings());
        }
    }
}
