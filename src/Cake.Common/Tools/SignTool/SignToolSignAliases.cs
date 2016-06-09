// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool
{
    /// <summary>
    /// <para>Contains functionality related to signing assemblies with PFX certificates using <see href="https://msdn.microsoft.com/en-gb/library/windows/desktop/aa387764(v=vs.85).aspx">SignTool</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, SignTool will need to be installed on the machine where
    /// the Cake script is being executed.  This is typically achieved by installing the correct Windows SDK.
    /// </para>
    /// </summary>
    [CakeAliasCategoryAttribute("Signing")]
    public static class SignToolSignAliases
    {
        /// <summary>
        /// Signs the specified assembly.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assembly">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// Task("Sign")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var file = "Core.dll";
        ///     Sign(file, new SignToolSignSettings {
        ///             TimeStampUri = new Uri("http://timestamp.digicert.com"),
        ///             CertPath = "digitalcertificate.pfx",
        ///             Password = "TopSecret"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, string assembly, SignToolSignSettings settings)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            Sign(context, new FilePath(assembly), settings);
        }

        /// <summary>
        /// Signs the specified assembly.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assembly">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// Task("Sign")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var file = new FilePath("Core.dll");
        ///     Sign(files, new SignToolSignSettings {
        ///             TimeStampUri = new Uri("http://timestamp.digicert.com"),
        ///             CertPath = "digitalcertificate.pfx",
        ///             Password = "TopSecret"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, FilePath assembly, SignToolSignSettings settings)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            var paths = new[] { assembly };
            Sign(context, paths, settings);
        }

        /// <summary>
        /// Signs the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// Task("Sign")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var files = new string[] { "Core.dll", "Common.dll" };
        ///     Sign(files, new SignToolSignSettings {
        ///             TimeStampUri = new Uri("http://timestamp.digicert.com"),
        ///             CertPath = "digitalcertificate.pfx",
        ///             Password = "TopSecret"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, IEnumerable<string> assemblies, SignToolSignSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Sign(context, paths, settings);
        }

        /// <summary>
        /// Signs the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// Task("Sign")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var files = GetFiles(solutionDir + "/**/bin/" + configuration + "/**/*.exe");
        ///     Sign(files, new SignToolSignSettings {
        ///             TimeStampUri = new Uri("http://timestamp.digicert.com"),
        ///             CertPath = "digitalcertificate.pfx",
        ///             Password = "TopSecret"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, IEnumerable<FilePath> assemblies, SignToolSignSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var runner = new SignToolSignRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Registry);
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}
