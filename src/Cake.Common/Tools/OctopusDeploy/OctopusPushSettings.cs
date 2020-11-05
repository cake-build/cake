// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains settings used by <see cref="OctopusDeployPusher.PushPackage"/>.
    /// </summary>
    public sealed class OctopusPushSettings : OctopusDeployCommonToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to overwrite an existing package.
        /// </summary>
        public bool ReplaceExisting { get; set; }
    }
}