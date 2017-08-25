// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere.Legacy
{
    /// <summary>
    /// The VSWhere tool that finds Visual Studio products.
    /// </summary>
    public sealed class VSWhereLegacy : VSWhereTool<VSWhereLegacySettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereLegacy"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public VSWhereLegacy(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator toolLocator) : base(fileSystem, environment, processRunner, toolLocator)
        {
        }

        /// <summary>
        /// Also searches Visual Studio 2015 and older products. Information is limited.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Installation paths for all instances.</returns>
        public DirectoryPathCollection Legacy(VSWhereLegacySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return RunVSWhere(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(VSWhereLegacySettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("-legacy");

            if (settings.Latest)
            {
                builder.Append("-latest");
            }

            AddCommonArguments(settings, builder);

            return builder;
        }
    }
}
