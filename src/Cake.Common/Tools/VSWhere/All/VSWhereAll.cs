// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere.All
{
    /// <summary>
    /// The VSWhere tool that finds all instances regardless if they are complete.
    /// </summary>
    public sealed class VSWhereAll : VSWhereTool<VSWhereAllSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereAll"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public VSWhereAll(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator toolLocator) : base(fileSystem, environment, processRunner, toolLocator)
        {
        }

        /// <summary>
        /// Finds all instances regardless if they are complete.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Installation paths for all instances.</returns>
        public DirectoryPathCollection All(VSWhereAllSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return RunVSWhere(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(VSWhereAllSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("-all");

            AddCommonArguments(settings, builder);

            return builder;
        }
    }
}
