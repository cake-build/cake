// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Specifies the package's assemblies that the target project should reference when being used.
    /// </summary>
    public class NuSpecReference
    {
        /// <summary>
        /// Gets or sets file for the reference.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the target framework for the reference.
        /// </summary>
        public string TargetFramework { get; set; }
    }
}
