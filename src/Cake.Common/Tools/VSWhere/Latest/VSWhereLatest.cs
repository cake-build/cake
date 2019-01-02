// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere.Latest
{
    /// <summary>
    /// The VSWhere tool that returns only the newest version and last installed.
    /// </summary>
    public sealed class VSWhereLatest : VSWhereTool<VSWhereLatestSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereLatest"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public VSWhereLatest(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator toolLocator) : base(fileSystem, environment, processRunner, toolLocator)
        {
        }

        /// <summary>
        /// Returns only the newest version and last installed.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Installation path of the newest or last install.</returns>
        public DirectoryPath Latest(VSWhereLatestSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return RunVSWhere(settings, GetArguments(settings)).FirstOrDefault();
        }

        private ProcessArgumentBuilder GetArguments(VSWhereLatestSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (!string.IsNullOrWhiteSpace(settings.Products))
            {
                builder.Append("-products");
                builder.AppendQuoted(settings.Products);
            }

            builder.Append("-latest");

            AddCommonArguments(settings, builder);

            return builder;
        }
    }
}
