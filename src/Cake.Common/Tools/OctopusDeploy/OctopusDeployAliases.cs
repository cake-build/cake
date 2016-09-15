// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://octopus.com/">Octopus Deploy</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=OctopusTools"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("Octopus Deploy")]
    public static class OctopusDeployAliases
    {
        /// <summary>
        /// Creates a release for the specified Octopus Deploy Project.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     // Minimum required
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         Server = "http://octopus-deploy.example",
        ///         ApiKey = "API-XXXXXXXXXXXXXXXXXXXX"
        ///     });
        ///
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         Server = "http://octopus-deploy.example",
        ///         Username = "DeployUser",
        ///         Password = "a-very-secure-password"
        ///     });
        ///
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         ConfigurationFile = @"C:\OctopusDeploy.config"
        ///     });
        ///
        ///     // Additional Options
        ///     OctoCreateRelease(projectNameOnServer, new CreateReleaseSettings {
        ///         ToolPath = "./tools/OctopusTools/Octo.exe"
        ///         EnableDebugLogging = true,
        ///         IgnoreSslErrors = true,
        ///         EnableServiceMessages = true, // Enables teamcity services messages when logging
        ///         ReleaseNumber = "1.8.2",
        ///         DefaultPackageVersion = "1.0.0.0", // All packages in the release should be 1.0.0.0
        ///         Packages = new Dictionary&lt;string, string&gt;
        ///                     {
        ///                         { "PackageOne", "1.0.2.3" },
        ///                         { "PackageTwo", "5.2.3" }
        ///                     },
        ///         PackagesFolder = @"C:\MyOtherNugetFeed",
        ///
        ///         // One or the other
        ///         ReleaseNotes = "Version 2.0 \n What a milestone we have ...",
        ///         ReleaseNotesFile = "./ReleaseNotes.md",
        ///
        ///         IgnoreExisting = true // if this release number already exists, ignore it
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OctoCreateRelease(this ICakeContext context, string projectName, CreateReleaseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var packer = new OctopusDeployReleaseCreator(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            packer.CreateRelease(projectName, settings);
        }

        /// <summary>
        /// Pushes the specified package to the Octopus Deploy repository
        /// </summary>
        /// <param name="context">The cake context</param>
        /// /// <param name="server">The Octopus server URL</param>
        /// <param name="apiKey">The user's API key</param>
        /// <param name="packagePath">Path to the package</param>
        /// <param name="settings">The settings</param>
        [CakeMethodAlias]
        public static void OctoPush(this ICakeContext context, string server, string apiKey, FilePath packagePath, OctopusPushSettings settings)
        {
            OctoPush(context, server, apiKey, new[] { packagePath }, settings);
        }

        /// <summary>
        /// Pushes the specified packages to the Octopus Deploy repository
        /// </summary>
        /// <param name="context">The cake context</param>
        /// <param name="server">The Octopus server URL</param>
        /// <param name="apiKey">The user's API key</param>
        /// <param name="packagePaths">Paths to the packages</param>
        /// <param name="settings">The settings</param>
        [CakeMethodAlias]
        public static void OctoPush(this ICakeContext context, string server, string apiKey, IEnumerable<FilePath> packagePaths, OctopusPushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (packagePaths == null)
            {
                throw new ArgumentNullException(nameof(packagePaths));
            }

            var pusher = new OctopusDeployPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            pusher.PushPackage(server, apiKey, packagePaths.ToArray(), settings);
        }
    }
}