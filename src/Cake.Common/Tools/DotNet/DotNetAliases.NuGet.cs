// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.NuGet.Delete;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Common.Tools.DotNet.NuGet.Source;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

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
        /// Delete a NuGet Package from a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDelete();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context)
        {
            context.DotNetNuGetDelete(null, null, null);
        }

        /// <summary>
        /// Deletes a package from nuget.org.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName)
        {
            context.DotNetNuGetDelete(packageName, null, null);
        }

        /// <summary>
        /// Deletes a specific version of a package from nuget.org.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="packageVersion">Version of package to delete.</param>
        /// <example>
        /// <code>
        /// DotNetRestore("Microsoft.AspNetCore.Mvc", "1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, string packageVersion)
        {
            context.DotNetNuGetDelete(packageName, packageVersion, null);
        }

        /// <summary>
        /// Deletes a package from a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, DotNetNuGetDeleteSettings settings)
        {
            context.DotNetNuGetDelete(packageName, null, settings);
        }

        /// <summary>
        /// Deletes a package from a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, DotNetNuGetDeleteSettings settings)
        {
            context.DotNetNuGetDelete(null, null, settings);
        }

        /// <summary>
        /// Deletes a package from a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageName">Name of package to delete.</param>
        /// <param name="packageVersion">Version of package to delete.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetDeleteSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     NonInteractive = true
        /// };
        ///
        /// DotNetNuGetDelete("Microsoft.AspNetCore.Mvc", "1.0", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Delete")]
        public static void DotNetNuGetDelete(this ICakeContext context, string packageName, string packageVersion, DotNetNuGetDeleteSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetNuGetDeleteSettings();
            }

            var nugetDeleter = new DotNetNuGetDeleter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            nugetDeleter.Delete(packageName, packageVersion, settings);
        }

        /// <summary>
        /// Pushes one or more packages to a server.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath"><see cref="FilePath"/> of the package to push.</param>
        /// <example>
        /// <code>
        /// // With FilePath instance
        /// var packageFilePath = GetFiles("*.nupkg").Single();
        /// DotNetNuGetPush(packageFilePath);
        /// // With string parameter
        /// DotNetNuGetPush("foo*.nupkg");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Push")]
        public static void DotNetNuGetPush(this ICakeContext context, FilePath packageFilePath)
        {
            context.DotNetNuGetPush(packageFilePath, null);
        }

        /// <summary>
        /// Pushes one or more packages to a server using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="packageFilePath"><see cref="FilePath"/> of the package to push.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetPushSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     ApiKey = "4003d786-cc37-4004-bfdf-c4f3e8ef9b3a"
        /// };
        /// // With FilePath instance
        /// var packageFilePath = GetFiles("foo*.nupkg").Single();
        /// DotNetNuGetPush(packageFilePath);
        /// // With string parameter
        /// DotNetNuGetPush("foo*.nupkg", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Push")]
        public static void DotNetNuGetPush(this ICakeContext context, FilePath packageFilePath, DotNetNuGetPushSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (settings is null)
            {
                settings = new DotNetNuGetPushSettings();
            }

            var restorer = new DotNetNuGetPusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Push(packageFilePath?.FullPath, settings);
        }

        /// <summary>
        /// Add the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     UserName = "username",
        ///     Password = "password",
        ///     StorePasswordInClearText = true,
        ///     ValidAuthenticationTypes = "basic,negotiate"
        /// };
        ///
        /// DotNetNuGetAddSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetAddSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.AddSource(name, settings);
        }

        /// <summary>
        /// Disable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetDisableSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetDisableSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetDisableSource(name, null);
        }

        /// <summary>
        /// Disable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetDisableSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetDisableSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.DisableSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Enable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetEnableSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetEnableSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetEnableSource(name, null);
        }

        /// <summary>
        /// Enable the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetEnableSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetEnableSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.EnableSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Determines whether the specified NuGet source exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <returns>Whether the specified NuGet source exists.</returns>
        /// <example>
        /// <code>
        /// var exists = DotNetNuGetHasSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static bool DotNetNuGetHasSource(this ICakeContext context, string name)
        {
            return context.DotNetNuGetHasSource(name, null);
        }

        /// <summary>
        /// Determines whether the specified NuGet source exists.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Whether the specified NuGet source exists.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// var exists = DotNetNuGetHasSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static bool DotNetNuGetHasSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return sourcer.HasSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Remove the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <example>
        /// <code>
        /// DotNetNuGetRemoveSource("example");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetRemoveSource(this ICakeContext context, string name)
        {
            context.DotNetNuGetRemoveSource(name, null);
        }

        /// <summary>
        /// Remove the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     ConfigFile = "NuGet.config"
        /// };
        ///
        /// DotNetNuGetRemoveSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetRemoveSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.RemoveSource(name, settings ?? new DotNetNuGetSourceSettings());
        }

        /// <summary>
        /// Update the specified NuGet source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetNuGetSourceSettings
        /// {
        ///     Source = "https://www.example.com/nugetfeed",
        ///     UserName = "username",
        ///     Password = "password",
        ///     StorePasswordInClearText = true,
        ///     ValidAuthenticationTypes = "basic,negotiate"
        /// };
        ///
        /// DotNetNuGetUpdateSource("example", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("NuGet")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.NuGet.Source")]
        public static void DotNetNuGetUpdateSource(this ICakeContext context, string name, DotNetNuGetSourceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);

            var sourcer = new DotNetNuGetSourcer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            sourcer.UpdateSource(name, settings);
        }
    }
}
