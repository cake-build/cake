// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Common.Tools.VSWhere.All;
using Cake.Common.Tools.VSWhere.Latest;
using Cake.Common.Tools.VSWhere.Legacy;
using Cake.Common.Tools.VSWhere.Product;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.VSWhere
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="https://github.com/Microsoft/vswhere">VSWhere</see> tool.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the settings class:
    /// <code>
    /// #tool "nuget:?package=vswhere"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("VSWhere")]
    public static class VSWhereAliases
    {
        /// <summary>
        /// Gets the legacy Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="latest">Get the latest version.</param>
        /// <returns>The Visual Studio installation path.</returns>
        /// <example>
        /// <code>
        ///     var legacyInstallationPath = VSWhereLegacy(true);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Legacy")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Legacy")]
        public static DirectoryPath VSWhereLegacy(this ICakeContext context, bool latest)
        {
            var settings = new VSWhereLegacySettings();
            settings.Latest = latest;
            return VSWhereLegacy(context, settings).FirstOrDefault();
        }

        /// <summary>
        /// Gets the legacy Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The Visual Studio installation paths.</returns>
        /// <example>
        /// <code>
        ///      var legacyInstallationPaths = VSWhereLegacy(new VSWhereLegacySettings());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Legacy")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Legacy")]
        public static DirectoryPathCollection VSWhereLegacy(this ICakeContext context, VSWhereLegacySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var legacy = new VSWhereLegacy(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return legacy.Legacy(settings);
        }

        /// <summary>
        /// Gets the latest Visual Studio product installation path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The Visual Studio installation path.</returns>
        /// <example>
        /// <code>
        ///     var latestInstallationPath = VSWhereLatest();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Latest")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Latest")]
        public static DirectoryPath VSWhereLatest(this ICakeContext context)
        {
            var settings = new VSWhereLatestSettings();
            return VSWhereLatest(context, settings);
        }

        /// <summary>
        /// Gets the latest Visual Studio product installation path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The Visual Studio installation path.</returns>
        /// <example>
        /// <code>
        ///      var latestInstallationPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "'Microsoft.Component.MSBuild" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Latest")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Latest")]
        public static DirectoryPath VSWhereLatest(this ICakeContext context, VSWhereLatestSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var latest = new VSWhereLatest(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return latest.Latest(settings);
        }

        /// <summary>
        /// Gets all Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The Visual Studio installation paths.</returns>
        /// <example>
        /// <code>
        ///     var latestInstallationPaths = VSWhereAll();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("All")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.All")]
        public static DirectoryPathCollection VSWhereAll(this ICakeContext context)
        {
            var settings = new VSWhereAllSettings();
            return VSWhereAll(context, settings);
        }

        /// <summary>
        /// Gets all Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The Visual Studio installation paths.</returns>
        /// <example>
        /// <code>
        ///     var latestInstallationPaths = VSWhereAll(new VSWhereAllSettings { Requires = "'Microsoft.Component.MSBuild" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("All")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.All")]
        public static DirectoryPathCollection VSWhereAll(this ICakeContext context, VSWhereAllSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var all = new VSWhereAll(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return all.All(settings);
        }

        /// <summary>
        /// Gets Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="products">The products to find.</param>
        /// <returns>The Visual Studio installation paths.</returns>
        /// <example>
        /// <code>
        ///     var latestInstallationPaths = VSWhereProducts("Microsoft.VisualStudio.Product.BuildTools");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Product")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Product")]
        public static DirectoryPathCollection VSWhereProducts(this ICakeContext context, string products)
        {
            var settings = new VSWhereProductSettings();
            return VSWhereProducts(context, products, settings);
        }

        /// <summary>
        /// Gets Visual Studio product installation paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="products">The products to find.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The Visual Studio installation paths.</returns>
        /// <example>
        /// <code>
        ///     var latestInstallationPaths = VSWhereProducts("Microsoft.VisualStudio.Product.BuildTools", new VSWhereProductSettings { Requires = "'Microsoft.Component.MSBuild" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Product")]
        [CakeNamespaceImport("Cake.Common.Tools.VSWhere.Product")]
        public static DirectoryPathCollection VSWhereProducts(this ICakeContext context, string products, VSWhereProductSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(products))
            {
                throw new ArgumentNullException(nameof(products));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.Products = products;
            var product = new VSWhereProduct(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return product.Products(settings);
        }
    }
}
