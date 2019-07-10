// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.MSBuild;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreMSBuildBuilder" />.
    /// </summary>
    public sealed class DotNetCoreMSBuildSettings : MSBuildSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to validate the project file and, if validation succeeds, build the project.
        /// </summary>
        public bool ValidateProjectFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not enable diagnostic output.
        /// </summary>
        public bool DiagnosticOutput { get; set; }
    }
}
