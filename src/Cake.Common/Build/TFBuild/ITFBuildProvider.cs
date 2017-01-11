// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild.Data;

namespace Cake.Common.Build.TFBuild
{
    /// <summary>
    /// Represents a TF Build provider.
    /// </summary>
    public interface ITFBuildProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on VSTS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on VSTS; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnVSTS { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on TFS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TFS; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnTFS { get; }

        /// <summary>
        /// Gets the TF Build environment.
        /// </summary>
        /// <value>
        /// The TF Build environment.
        /// </value>
        TFBuildEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the TF Build Commands provider.
        /// </summary>
        /// <value>
        /// The TF Build commands provider.
        /// </value>
        ITFBuildCommands Commands { get; }
    }
}
