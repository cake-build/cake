// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere.Product
{
    /// <summary>
    /// The VSWhere tool that finds Visual Studio products.
    /// </summary>
    public sealed class VSWhereProduct : VSWhereTool<VSWhereProductSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereProduct"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public VSWhereProduct(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator toolLocator) : base(fileSystem, environment, processRunner, toolLocator)
        {
        }

        /// <summary>
        /// Finds one ore more products.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Installation paths for all instances.</returns>
        public DirectoryPathCollection Products(VSWhereProductSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return RunVSWhere(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(VSWhereProductSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (!string.IsNullOrWhiteSpace(settings.Products))
            {
                builder.Append("-products");
                builder.Append(settings.Products);
            }

            AddCommonArguments(settings, builder);

            return builder;
        }
    }
}
